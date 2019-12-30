using System;
using System.ComponentModel.DataAnnotations;
using ChessGame.Models.Chess;

namespace ChessGame.Models
{
    public class Board
    {
        public long Id { get; set; }

        public string? User1Identifier { get; set; }
        public string? User1Name { get; set; }
        public Color? User1Color { get; set; }
        public string? User2Identifier { get; set; }
        public string? User2Name { get; set; }
        public Color? User2Color { get; set; }

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
                if (User2Color == Color.White) User1Color = Color.Black;
                else User1Color = Color.White;
            }
            else if (String.IsNullOrEmpty(User2Name))
            {
                User2Name = name;
                User2Identifier = identifier;
                if (User1Color == Color.White) User2Color = Color.Black;
                else User2Color = Color.White;
            }
            else throw new Exception("addPlayer when board have 2 players on it");
        }
        public string getOpponentIdentifier(string userName)
        {
            if (this.User1Name == userName)
            {
                return this.User2Identifier;
            }
            else if (this.User2Name == userName)
            {
                return this.User1Identifier;
            }
            else throw new Exception("getOpponentIdentifier have wrong param");
        }
    }
}
