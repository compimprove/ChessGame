using System;
using System.ComponentModel.DataAnnotations;

namespace ChessGame.Models
{
    public class Board
    {
        public long Id { get; set; }
        
        public string? User1Identifier { get; set; }
        public string? User1Name { get; set; }
        public string? User2Identifier { get; set; }
        public string? User2Name { get; set; }

        public bool haveOnePlayer()
        {
            return ((String.IsNullOrEmpty(User1Name) && !String.IsNullOrEmpty(User2Name)) || (!String.IsNullOrEmpty(User1Name) && String.IsNullOrEmpty(User2Name)));
        }

        public void addPlayer(string name, string identifier)
        {
            if (String.IsNullOrEmpty(User1Name))
            {
                User1Name = name;
                User1Identifier = identifier;
            }
            else if (String.IsNullOrEmpty(User2Name))
            {
                User2Name = name;
                User2Identifier = identifier;
            }
            else throw new Exception("addPlayer when board have 2 players on it");
        }
    }
}
