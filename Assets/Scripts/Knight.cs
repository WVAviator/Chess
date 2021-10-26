using System;
using UnityEngine;

namespace Chess
{
    public class Knight : ChessPiece
    {
        
        public override string PieceName => "Knight";
        public Knight(ChessPieceColor color, Vector2Int position = default) : base(color, position)
        {
        }

        public override bool IsLegalMove(Move move)
        {

            if (AllyInPosition(move.NewPosition)) return false;
            if (IsLShaped(move)) return true;
            return false;

        }

        static bool IsLShaped(Move move) => 
            (Math.Abs(move.XDifference) == 2 && Math.Abs(move.YDifference) == 1) || 
            (Math.Abs(move.XDifference) == 1 && Math.Abs(move.YDifference) == 2);
    }
}