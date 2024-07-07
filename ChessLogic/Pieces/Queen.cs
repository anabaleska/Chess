﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic
{
    public class Queen : Piece
    {
        public override PieceType pieceType => PieceType.Queen;

        public override Player Color { get; }
        private static readonly Direction[] dirs = new Direction[]
        {
            Direction.East,
            Direction.South,
            Direction.West,
            Direction.North,
            Direction.SouthWest,
            Direction.NorthEast,
            Direction.NorthWest,
            Direction.SouthEast,
        };
       
        public Queen(Player color)
        {
            Color = color;
        }
        public override Piece Copy()
        {
            Queen copy = new Queen(Color);
            copy.HasMoved = HasMoved;
            return copy;
        }

        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            return MovePositionInDirs(from, board, dirs).Select(to => new NormalMove(from, to));
        }
    }
}
