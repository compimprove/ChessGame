using System.Threading.Tasks;
using ChessGame.Models;
using ChessGame.Models.Chess;

namespace ChessGame.Service
{
    public interface IBotHandler
    {
        public Task HandleMove(GameBoard gameBoard);
    }
}