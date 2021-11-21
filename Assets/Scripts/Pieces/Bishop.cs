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

        protected override List<Vector2Int> GetPotentialPositions() => GetPotentialDiagonalPositions();
    }
}