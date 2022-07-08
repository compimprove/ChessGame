using System;
using ChessGame.Models.Chess;

namespace ChessGame.Models
{
    public class Board
    {
        public long Id { get; set; }

        public string? User1Identifier { get; set; }
        public string? User1Name { get; set; }
        public Color User1Color { get; set; }
        public string? User2Identifier { get; set; }
        public string? User2Name { get; set; }
        public Color User2Color { get; set; }

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
                User1Color = User2Color == Color.White ? Color.Black : Color.White;
            }
            else if (String.IsNullOrEmpty(User2Name))
            {
                User2Name = name;
                User2Identifier = identifier;
                User2Color = User1Color == Color.White ? Color.Black : Color.White;
            }
            else throw new Exception("addPlayer when board have 2 players on it");
        }

        public Color getUserColor(string userName)
        {
            if (User1Name == userName)
            {
                return User1Color;
            }

            if (User2Name == userName)
            {
                return User2Color;
            }
            
            throw new Exception("userName have wrong param");
        }
        public string getOpponentIdentifier(string userName)
        {
            if (User1Name == userName)
            {
                return User2Identifier;
            }

            if (User2Name == userName)
            {
                return User1Identifier;
            }

            throw new Exception("getOpponentIdentifier have wrong param");
        }

        public object getOpponentName(string userName)
        {
            if (User1Name == userName)
            {
                return User2Name;
            }

            if (User2Name == userName)
            {
                return User1Name;
            }

            throw new Exception("getOpponentIdentifier have wrong param");
        }
    }
}
