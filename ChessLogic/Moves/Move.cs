using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic
{
    public abstract class Move
    {
        public abstract MoveType Type { get; }
        public abstract Position FromPos { get; }
        public abstract Position ToPos { get; }  

        public abstract bool Execute(Board board);

        public virtual bool IsLegal(Board board)
        {
            Player player = board[FromPos].Color;
            Board boradCopy = board.copy();
            Execute(boradCopy);
            return !boradCopy.IsInCheck(player);
        }
    }
}
