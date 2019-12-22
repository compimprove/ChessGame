using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChessGame.Data;
using ChessGame.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace ChessGame.Signal
{
    public class GameHub : Hub
    {

        public const int MaxBoards = 10;
        private SqlServerDbContext _context;

        public GameHub(SqlServerDbContext context)
        {
            this._context = context;
        }

        public async Task JoinBoard(long? boardId, string userName)
        {
            if (boardId != null)
            {
                Board board = await _context.boards.FindAsync(boardId);
                board.User2Name = userName;
                board.User2Identifier = Context.ConnectionId;
                try
                {
                    _context.Update(board);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                }
                await Clients.Caller.SendAsync("JoinedBoard", board.Id);
            }
        }
        public async Task CreateBoard(string userName)
        {
            Board board = new Board();
            board.User1Name = userName;
            board.User1Identifier = Context.ConnectionId;
            _context.Add(board);
            await _context.SaveChangesAsync();
            await Clients.Caller.SendAsync("JoinedBoard", board.Id);
        }

        public async Task Disconnect(long? boardId)
        {
            if (boardId != null)
            {
                Board board = await _context.boards.FindAsync(boardId);
                if (board.User1Identifier == Context.ConnectionId)
                {
                    if (board.User2Identifier == null)
                    {
                        _context.boards.Remove(board);
                    }
                    else
                    {
                        board.User1Identifier = null;
                        board.User1Name = null;
                        _context.Update(board);
                    }
                    
                    await _context.SaveChangesAsync();
                }
                else if (board.User2Identifier == Context.ConnectionId)
                {
                    if (board.User1Identifier == null)
                    {
                        _context.boards.Remove(board);
                    }
                    else
                    {
                        board.User2Identifier = null;
                        board.User2Name = null;
                        _context.Update(board);
                    }
                    await _context.SaveChangesAsync();
                }
            }
        }
        public async Task GetBoards()
        {
            List<Board> boards = await _context.boards.ToListAsync();
            await Clients.Caller.SendAsync("GetBoards", boards);
        }
    }
}
