using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Text.Json;
using System;

namespace ChessGame.Signal
{
    public class GameHub : Hub
    {
        public struct Board
        {
            public string name { get; set; }
            public string User1Identifier { get; set; }
            public string NameUser1 { get; set; }
            public string User2Identifier { get; set; }
            public string NameUser2 { get; set; }
            
        }
        public const int MaxBoards = 10;

        public async Task AddToBoard(string message)
        {

        }
        public async Task CreateBoard(string boardName, string userName)
        {
            if (true)
            {
            }
            else
            {
                Board board = new Board();
                board.name = boardName;
                board.NameUser1 = userName;
                board.User1Identifier = Context.UserIdentifier;
            }
        }

        public async Task GetBoards()
        {
            await Clients.Caller.SendAsync("GetBoards");
        }
    }
}
