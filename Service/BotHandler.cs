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
        private const int MaxDept = 5;
        private readonly IHubContext<GameHub> _hubContext;
        private readonly ILogger _logger;
        private long _loop = 0;
        private int[] _bestValues;

        public BotHandler(
            IHubContext<GameHub> hubContext,
            ILogger<BotHandler> logger)
        {
            _hubContext = hubContext;
            _logger = logger;
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
            _bestValues = new int[MaxDept];
            _bestValues[0] = int.MinValue;
            var results = new List<(Coord, Coord)>();
            var botColor = getBotColor(board);
            var botPieces = gameBoard.getColorPieces(getBotColor(board));
            foreach (var piece in botPieces)
            {
                piece.GeneratePossibleMove();
                var pieceOriginCoord = piece.square.coord;
                var possibleMoves = piece.possibleMoves.Select(square => square.coord).ToArray();
                foreach (var possibleMove in possibleMoves)
                {
                    _logger.LogTrace($"--------------- {piece} ------------------");
                    gameBoard.MovePiece(pieceOriginCoord, possibleMove);
                    _logger.LogTrace(gameBoard.ToString());
                    var value = GetValue(gameBoard, 1, false, botColor);
                    if (_bestValues[0] < value)
                    {
                        _bestValues[0] = value;
                        results.Clear();
                        results.Add((pieceOriginCoord, possibleMove));
                    }
                    else if (_bestValues[0] == value)
                    {
                        results.Add((pieceOriginCoord, possibleMove));
                    }

                    gameBoard.undo();
                    _logger.LogTrace($"------------------------------------------");
                }
            }

            var logMessage = $"Best Value: {string.Join(" ", _bestValues.Select((v, i) => $" {i}:{v} ").ToArray())}";
            _logger.LogInformation(logMessage);
            if (results.Count == 0) return results[0];
            return results[new Random().Next(results.Count)];
        }

        private int GetValue(GameBoard gameBoard, int dept, bool botTurn, Color botColor)
        {
            _loop++;
            if (dept == MaxDept) return gameBoard.GetTotalValue(botColor);
            var allPieces = botTurn
                ? gameBoard.getColorPieces(botColor)
                : gameBoard.getOpponentPieces(botColor);
            _bestValues[dept] = botTurn ? int.MinValue : int.MaxValue;
            var willBreak = false;
            _logger.LogTrace($"Dept: {dept} -------------");
            foreach (var piece in allPieces)
            {
                piece.GeneratePossibleMove();
                _logger.LogTrace($" {piece} ");
                var possibleMoves = piece.possibleMoves.Select(square => square.coord).ToArray();
                foreach (var possibleMove in possibleMoves)
                {
                    gameBoard.MovePiece(piece.square.coord, possibleMove);
                    var calculatedValue = GetValue(gameBoard, dept + 1, !botTurn, botColor);
                    if (botTurn && calculatedValue > _bestValues[dept - 1])
                    {
                        willBreak = true;
                    }
                    else if (!botTurn && calculatedValue < _bestValues[dept - 1])
                    {
                        willBreak = true;
                    }

                    if (botTurn && calculatedValue > _bestValues[dept] ||
                        !botTurn && calculatedValue < _bestValues[dept])
                    {
                        _bestValues[dept] = calculatedValue;
                    }

                    gameBoard.undo();
                    if (willBreak) break;
                }

                if (willBreak) break;
            }

            _logger.LogTrace($"{_bestValues[dept]}-----------------------");

            return _bestValues[dept];
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
                MovePiece(gameBoard, board, coordChosen, coordMoveTo);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
        }

        private void MovePiece(GameBoard gameBoard, Board board, Coord coordFrom, Coord coordTo)
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

            _hubContext.Clients.Client(getUserIdentifier(board))
                .SendAsync("MovingPiece", "bot", coordFrom, coordTo, kingDangerMove);
        }
    }
}