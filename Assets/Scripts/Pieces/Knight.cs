using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public class Knight : ChessPiece
    {
        public override string PieceName => "Knight";
        
        public override char PieceChar
        {
            get
            {
                return Color == ChessPieceColor.Black ? 'n' : 'N';
            }
        }

        public Knight(ChessPieceColor color, Vector2Int position = default) : base(color, position)
        {
        }

        public Knight(ChessPieceColor color) : base(color, default)
        {
        }

        public override int GetScore() => 3;

        public override bool IsLegalMove(Move move)
        {
            if (AllyInPosition(move.NewPosition)) return false;
            if (IsLShaped(move)) return true;
            return false;
        }

        protected override HashSet<Move> GetPotentialMoves()
        {
            int x = Position.x;
            int y = Position.y;
            HashSet<Move> moves = new HashSet<Move>();
            
            if (x + 1 < 8 && y + 2 < 8) moves.Add(To(x + 1, y + 2));
            if (x + 1 < 8 && y - 2 >= 0) moves.Add(To(x + 1, y - 2));
            if (x - 1 >= 0 && y + 2 < 8) moves.Add(To(x - 1, y + 2));
            if (x - 1 >= 0 && y - 2 >= 0) moves.Add(To(x - 1, y - 2));
            if (x + 2 < 8 && y + 1 < 8) moves.Add(To(x + 2, y + 1));
            if (x + 2 < 8 && y - 1 >= 0) moves.Add(To(x + 2, y - 1));
            if (x - 2 >= 0 && y + 1 < 8) moves.Add(To(x - 2, y + 1));
            if (x - 2 >= 0 && y - 1 >= 0) moves.Add(To(x - 2, y - 1));

            return moves;
        }

        static bool IsLShaped(Move move) =>
            (Math.Abs(move.XDifference) == 2 && Math.Abs(move.YDifference) == 1) ||
            (Math.Abs(move.XDifference) == 1 && Math.Abs(move.YDifference) == 2);
    }
}