using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ChessGame.Models.Chess;
using ChessGame.Data;
using ChessGame.Service;
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
        private readonly IBotHandler _botHandler;
        public BoardController(
            IBotHandler botHandler,
            InMemoryDbContext context,
            IHubContext<GameHub> hubContext)
        {
            _botHandler = botHandler;
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
    }
}