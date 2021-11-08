using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public class Bishop : GlidingPiece
    {
        public Bishop(ChessPieceColor color, Vector2Int position = default) : base(color, position)
        {
        }
        
        public Bishop(ChessPieceColor color) : base(color, default)
        {
        }

        public override char PieceChar
        {
            get
            {
                return Color == ChessPieceColor.Black ? 'b' : 'B';
            }
        }

        public override string PieceName => "Bishop";

        public override int GetScore() => 3;

        public override bool IsLegalMove(Move move)
        {
            if (move.NewPosition == Position) return false;
            if (!Diagonal(move)) return false;
            if (Blocked(move)) return false;
            if (AllyInPosition(move.NewPosition)) return false;

            return true;
        }

        protected override HashSet<Move> GetPotentialMoves() => GetPotentialDiagonalMoves();
    }
}