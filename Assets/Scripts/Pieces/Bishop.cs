using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public class Bishop : GlidingPiece
    {
        public Bishop(ChessPieceColor color) : base(color, default)
        {
        }

        public override char PieceChar => Color == ChessPieceColor.Black ? 'b' : 'B';

        public override string PieceName => "Bishop";

        public override int GetScore() => 3;

        protected override List<Vector2Int> GetPotentialPositions() => GetPotentialDiagonalPositions();

        public override bool IsLegalMove(Vector2Int newPosition)
        {
            return base.IsLegalMove(newPosition) 
                   && Diagonal(newPosition) 
                   && !Blocked(newPosition) 
                   && !PutsKingInCheck(newPosition);
        }
    }
}