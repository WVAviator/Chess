using System;
using UnityEngine;

namespace Chess
{
    public class King : ChessPiece
    {
        public King(ChessPieceColor color, Vector2Int position = default) : base(color, position)
        {
        }

        public override string PieceName => "King";

        public override bool IsLegalMove(Move move)
        {
            if (move.NewPosition == Position) return false;
            if (!OneSpace(move)) return false;
            if (AllyInPosition(move.NewPosition)) return false;
            
            return true;
        }

        static bool OneSpace(Move move) => Math.Abs(move.XDifference) <= 1 && Math.Abs(move.YDifference) <= 1;
    }
}