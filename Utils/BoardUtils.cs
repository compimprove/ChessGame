using System.Collections.Generic;
using System.Diagnostics;
using ChessGame.Models;
using ChessGame.Models.Chess;

namespace ChessGame.Utils
{
    public static class BoardUtils
    {
        public static List<Coord> GetKingDangerMoves(GameBoard gameBoard)
        {
            var kingDangerMove = new List<Coord>();

            if (gameBoard.blackKing.InDanger())
            {
                kingDangerMove.Add(gameBoard.blackKing.Coord);
            }

            if (gameBoard.whiteKing.InDanger())
            {
                kingDangerMove.Add(gameBoard.whiteKing.Coord);
            }

            return kingDangerMove;
        }

        public static string GetWinnerName(GameBoard gameBoard)
        {
            var board = gameBoard.boardInfo;
            var winner = "";
            if (!gameBoard.IsKingLive(board.User1Color))
            {
                winner = board.User2Name;
            }
            else if (!gameBoard.IsKingLive(board.User2Color))
            {
                winner = board.User1Name;
            }

            return winner;
        }
    }
}