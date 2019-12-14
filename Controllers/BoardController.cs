using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ChessGame.Models.Chess;
using ChessGame.Models.Chess.piece;

namespace ChessGame.Controllers
{
    public struct BoardRequest
    {
        public string[][] board { get; set; }
        public Coord coordChosen { get; set; }
        public Coord coordClick { get; set; }
        public string direction { get; set; }
    }
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BoardController : ControllerBase
    {
        [HttpPost]
        public ActionResult<Coord[]> GeneratePossibleMove(BoardRequest request)
        {
            Direction direction = request.direction.ToLower() == "whitegodown" ? Direction.WhiteGodown : Direction.WhiteGoup;
            Board board = Board.GetBoard(request.board, direction);

            Piece piece = board.GetSquare(request.coordClick).piece;
            piece.GeneratePossibleMove();
            return piece.possibleMoves.Select(square => square.coord).ToArray();

        }

        [HttpPost]
        public ActionResult<bool> Move(BoardRequest request)
        {
            Direction direction = request.direction.ToLower() == "whitegodown" ? Direction.WhiteGodown : Direction.WhiteGoup;
            Board board = Board.GetBoard(request.board, direction);

            Piece piece = board.GetSquare(request.coordChosen).piece;
            piece.GeneratePossibleMove();
            if (piece.possibleMoves.Contains(board.GetSquare(request.coordClick)))
            {
                return true;
            }
            else return false;
        }
    }
}