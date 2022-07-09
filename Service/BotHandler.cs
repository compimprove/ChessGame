using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChessGame.Models;
using ChessGame.Models.Chess;
using ChessGame.Signal;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace ChessGame.Service
{
    public class BotHandler : IBotHandler
    {
        private const int MaxDept = 4;
        private readonly IHubContext<GameHub> _hubContext;
        private readonly ILogger _logger;
        private long _loop = 0;
        private int[] _values;

        public BotHandler(
            IHubContext<GameHub> hubContext,
            ILogger<BotHandler> logger)
        {
            _hubContext = hubContext;
            _logger = logger;
            _values = new int[MaxDept];
        }

        private Color getBotColor(Board board)
        {
            return board.User1Identifier == "bot" ? board.User1Color : board.User2Color;
        }

        private string getUserIdentifier(Board board)
        {
            return board.User1Identifier == "bot" ? board.User2Identifier : board.User1Identifier;
        }

        private (Coord, Coord) Process(GameBoard gameBoard, Board board)
        {
            _logger.LogTrace("----------------------------");
            _logger.LogTrace($"Dept: 0 -------------");
            _loop = 0;
            var bestValue = int.MinValue;
            var results = new List<(Coord, Coord)>();
            var botColor = getBotColor(board);
            var botPieces = gameBoard.getColorPieces(getBotColor(board));
            foreach (var piece in botPieces)
            {
                piece.GeneratePossibleMove();
                var pieceOriginCord = piece.square.coord;
                var possibleMoves = piece.possibleMoves.Select(square => square.coord).ToArray();
                foreach (var possibleMove in possibleMoves)
                {
                    _logger.LogTrace($"--------------- {piece} ------------------");
                    gameBoard.MovePiece(pieceOriginCord, possibleMove);
                    _logger.LogTrace(gameBoard.ToString());
                    var value = GetValue(gameBoard, 1, false, botColor);
                    if (bestValue < value)
                    {
                        bestValue = value;
                        results.Clear();
                        results.Add((pieceOriginCord, possibleMove));
                    }
                    else if (bestValue == value)
                    {
                        results.Add((pieceOriginCord, possibleMove));
                    }

                    gameBoard.undo();
                    _logger.LogTrace($"------------------------------------------");
                }
            }

            if (results.Count == 0) return results[0];
            return results[new Random().Next(results.Count)];
        }

        private int GetValue(GameBoard gameBoard, int dept, bool botTurn, Color botColor)
        {
            _loop++;
            if (dept == MaxDept) return gameBoard.getTotalValue(botColor);
            var allPieces = botTurn
                ? gameBoard.getColorPieces(botColor)
                : gameBoard.getOpponentPieces(botColor);
            var bestValue = botTurn ? int.MinValue : int.MaxValue;

            _logger.LogTrace($"Dept: {dept} -------------");
            foreach (var piece in allPieces)
            {
                piece.GeneratePossibleMove();
                _logger.LogTrace($" {piece} ");
                var possibleMoves = piece.possibleMoves.Select(square => square.coord).ToArray();
                foreach (var possibleMove in possibleMoves)
                {
                    gameBoard.MovePiece(piece.square.coord, possibleMove);
                    var currentValue = GetValue(gameBoard, dept + 1, !botTurn, botColor);
                    if (botTurn && currentValue > bestValue || !botTurn && currentValue < bestValue)
                    {
                        bestValue = currentValue;
                    }

                    gameBoard.undo();
                }
            }

            _logger.LogTrace($"{bestValue}-----------------------");

            return bestValue;
        }

        public async Task HandleMove(GameBoard gameBoard, Board board)
        {
            try
            {
                await Task.Delay(100);
                _logger.LogInformation("Input-----------------------\n{board}", gameBoard.ToString());
                var start = DateTime.Now;
                var (coordChosen, coordMoveTo) = Process(gameBoard, board);
                gameBoard.MovePiece(coordChosen, coordMoveTo);
                var end = DateTime.Now;
                _logger.LogInformation("Loop: {loop}-Time: {time}---\n{board}", _loop, (end - start).ToString(),
                    gameBoard.ToString());
                MovePiece(board, coordChosen, coordMoveTo);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
        }

        private void MovePiece(Board board, Coord coordFrom, Coord coordTo)
        {
            _hubContext.Clients.Client(getUserIdentifier(board)).SendAsync("MovingPiece", "bot", coordFrom, coordTo);
        }
    }
}