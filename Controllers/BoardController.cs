using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ChessGame.Models;
using ChessGame.Models.Chess;
using ChessGame.Models.Chess.piece;
using ChessGame.Data;
using ChessGame.Signal;
using Microsoft.AspNetCore.SignalR;

namespace ChessGame.Controllers
{
    public struct BoardRequest
    {
        public long boardId { get; set; }
        public string[][] board { get; set; }
        public Coord coordChosen { get; set; }
        public Coord coordClick { get; set; }
        public string direction { get; set; }

        public string userName { get; set; }
    }
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BoardController : ControllerBase
    {
        private readonly InMemoryDbContext _context;
        private readonly IHubContext<GameHub> _hubContext;
        public BoardController(
            InMemoryDbContext context,
            IHubContext<GameHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [HttpPost]
        public ActionResult<Coord[]> GeneratePossibleMove(BoardRequest request)
        {
            Direction direction = request.direction.ToLower() == "whitegodown" ? Direction.WhiteGodown : Direction.WhiteGoup;
            GameBoard board = GameBoard.GetBoard(request.board, direction);

            Piece piece = board.GetSquare(request.coordClick).piece;
            piece.GeneratePossibleMove();
            return piece.possibleMoves.Select(square => square.coord).ToArray();

        }

        [HttpPost]
        public async Task<ActionResult<bool>> Move(BoardRequest request)
        {
            Direction direction = request.direction.ToLower() == "whitegodown" ? Direction.WhiteGodown : Direction.WhiteGoup;
            GameBoard gameBoard = GameBoard.GetBoard(request.board, direction);

            Piece piece = gameBoard.GetSquare(request.coordChosen).piece;
            piece.GeneratePossibleMove();
            if (piece.possibleMoves.Contains(gameBoard.GetSquare(request.coordClick)))
            {
                Board board = await _context.boards.FindAsync(request.boardId);
                string opponentConnId = board.getOpponentIdentifier(request.userName);
                _hubContext.Clients.Client(opponentConnId)
                    .SendAsync(
                    "OpponentMoving",
                    request.coordChosen.getMindSymmetry(),
                    request.coordClick.getMindSymmetry()
                    );
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}