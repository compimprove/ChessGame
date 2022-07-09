using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChessGame.Data;
using ChessGame.Models;
using Microsoft.EntityFrameworkCore;
using ChessGame.Controllers;
using ChessGame.Models.Chess;
using ChessGame.Service;

namespace ChessGame.Signal
{
    public class GameHub : Hub
    {
        public const int MaxBoards = 100;
        private readonly InMemoryDbContext _context;
        private readonly IBotHandler _botHandler;

        public GameHub(InMemoryDbContext context, IBotHandler botHandler)
        {
            _context = context;
            _botHandler = botHandler;
        }

        public async Task JoinBoard(long? boardId, string userName)
        {
            if (boardId != null)
            {
                Board board = await _context.boards.FindAsync(boardId);
                if (board.haveOnePlayer())
                    board.addPlayer(userName, Context.ConnectionId);
                try
                {
                    _context.Update(board);
                    await _context.SaveChangesAsync();
                    bool userTurn = false;
                    await Clients.Caller.SendAsync("JoinedBoard", board, userTurn);

                    List<Board> boards = await _context.boards.ToListAsync();
                    await Clients.All.SendAsync("GetBoards", boards);
                }
                catch (DbUpdateConcurrencyException)
                {
                }
            }
        }

        public async Task CreateBoard(string userName)
        {
            int length = await _context.boards.CountAsync();
            if (length > MaxBoards)
            {
                return;
            }
            else
            {
                Board board = new Board
                {
                    User1Name = userName,
                    User1Identifier = Context.ConnectionId,
                    User1Color = Color.White
                };
                bool userTurn = true;

                _context.Add(board);
                await _context.SaveChangesAsync();
                await Clients.Caller.SendAsync("JoinedBoard", board, userTurn);
                List<Board> boards = await _context.boards.ToListAsync();
                await Clients.All.SendAsync("GetBoards", boards);
            }
        }

        public async Task CreateBotBoard(string userName)
        {
            int length = await _context.boards.CountAsync();
            if (length > MaxBoards)
            {
                return;
            }
            else
            {
                Board board = new Board
                {
                    User1Name = userName,
                    User1Identifier = Context.ConnectionId,
                    User1Color = Color.White,
                    User2Name = "bot",
                    User2Identifier = "bot",
                    User2Color = Color.Black
                };
                bool userTurn = true;

                _context.Add(board);
                await _context.SaveChangesAsync();
                await Clients.Caller.SendAsync("JoinedBoard", board, userTurn);
                List<Board> boards = await _context.boards.ToListAsync();
                await Clients.All.SendAsync("GetBoards", boards);
            }
        }

        public async Task MovingPiece(BoardRequest request)
        {
            Direction direction = request.direction.ToLower() == "whitegodown"
                ? Direction.WhiteGoDown
                : Direction.WhiteGoUp;
            GameBoard gameBoard = GameBoard.GetBoard(request.board, direction);

            Piece piece = gameBoard.GetSquare(request.coordChosen).piece;
            piece.GeneratePossibleMove();
            if (piece.possibleMoves.Contains(gameBoard.GetSquare(request.coordClick)))
            {
                Board board = await _context.boards.FindAsync(request.boardId);
                gameBoard.boardInfo = board;
                var opponentConnId = board.getOpponentIdentifier(request.userName);
                var opponentName = board.getOpponentName(request.userName);
                Clients.Caller.SendAsync("MovingPiece",
                    request.userName,
                    request.coordChosen,
                    request.coordClick);
                if (opponentConnId == "bot")
                {
                    gameBoard.MovePiece(request.coordChosen, request.coordClick);
                    _botHandler.HandleMove(gameBoard, board);
                    return;
                }
                Clients.Client(opponentConnId)
                    .SendAsync(
                        "MovingPiece",
                        opponentName,
                        request.coordChosen.getMindSymmetry(),
                        request.coordClick.getMindSymmetry()
                    );
            }
        }

        public async Task Disconnect(long boardId)
        {
            if (boardId != null)
            {
                Board board = await _context.boards.FindAsync(boardId);
                if (board.User1Identifier == Context.ConnectionId)
                {
                    if (board.User2Identifier == null || board.User2Identifier == "bot")
                    {
                        _context.boards.Remove(board);
                    }
                    else
                    {
                        board.User1Identifier = null;
                        board.User1Name = null;
                        _context.Update(board);
                    }
                }
                else if (board.User2Identifier == Context.ConnectionId)
                {
                    if (board.User1Identifier == null || board.User1Identifier == "bot")
                    {
                        _context.boards.Remove(board);
                    }
                    else
                    {
                        board.User2Identifier = null;
                        board.User2Name = null;
                        _context.Update(board);
                    }
                }

                await _context.SaveChangesAsync();
                List<Board> boards = await _context.boards.ToListAsync();
                await Clients.All.SendAsync("GetBoards", boards);
            }
        }

        public async Task GetBoards()
        {
            List<Board> boards = await _context.boards.ToListAsync();
            await Clients.Caller.SendAsync("GetBoards", boards);
        }

        public async Task GetBoard(long boardId)
        {
            Board board = await _context.boards.FindAsync(boardId);
            await Clients.Caller.SendAsync("GetBoard", board);
        }

        #region When player join the board

        public async Task JoinedBoard()
        {
        }

        #endregion

        #region Game Logic

        //public struct BoardRequest
        //{
        //    public string[][] board { get; set; }
        //    public Coord coordChosen { get; set; }
        //    public Coord coordClick { get; set; }
        //    public string direction { get; set; }
        //}
        //public async Task GeneratePossibleMove(BoardRequest request)
        //{
        //    Direction direction = request.direction.ToLower() == "whitegodown" ? Direction.WhiteGoDown : Direction.WhiteGoUp;
        //    GameBoard board = GameBoard.GetBoard(request.board, direction);

        //    Piece piece = board.GetSquare(request.coordClick).piece;
        //    piece.GeneratePossibleMove();
        //}

        //public async Task Move(BoardRequest request)
        //{
        //    Direction direction = request.direction.ToLower() == "whitegodown" ? Direction.WhiteGoDown : Direction.WhiteGoUp;
        //    GameBoard board = GameBoard.GetBoard(request.board, direction);

        //    Piece piece = board.GetSquare(request.coordChosen).piece;
        //    piece.GeneratePossibleMove();
        //    if (piece.possibleMoves.Contains(board.GetSquare(request.coordClick)))
        //    {
        //        return true;
        //    }
        //    else return false;
        //}

        #endregion
    }
}