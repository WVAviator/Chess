using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public class Rook : GlidingPiece
    {
        public override string PieceName => "Rook";

        public Rook(ChessPieceColor color) : base(color, default)
        {
        }
        
        public Rook(ChessPieceColor color, Vector2Int position = default) : base(color, position)
        {
        }

        public override bool IsLegalMove(Move move)
        {
            if (move.NewPosition == Position) return false;
            if (!VerticalOrHorizontal(move)) return false;
            if (Blocked(move)) return false;
            if (AllyInPosition(move.NewPosition)) return false;

            return true;
        }
    }
}