using System;
using UnityEngine;

namespace Chess
{
    public class Queen : GlidingPiece
    {
        
        public override string PieceName => "Queen";
        public Queen(ChessPieceColor color, Vector2Int position = default) : base(color, position)
        {
        }
        
        public Queen(ChessPieceColor color) : base(color, default)
        {
        }

        public override bool IsLegalMove(Move move)
        {
            if (move.NewPosition == Position) return false;
            if (!Diagonal(move) && !VerticalOrHorizontal(move)) return false;
            if (Blocked(move)) return false;
            if (AllyInPosition(move.NewPosition)) return false;
            
            return true;
        }
    }
}