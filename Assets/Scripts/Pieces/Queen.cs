using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chess
{
    public class Queen : GlidingPiece
    {
        public override char PieceChar => Color == ChessPieceColor.Black ? 'q' : 'Q';
        public override string PieceName => "Queen";

        public Queen(ChessPieceColor color) : base(color, default)
        {
        }

        public override int GetScore() => 9;

        protected override List<Vector2Int> GetPotentialPositions()
        {
            List<Vector2Int> positions = new List<Vector2Int>();
            positions.AddRange(GetPotentialDiagonalPositions());
            positions.AddRange(GetPotentialVerticalHorizontalPositions());
            return positions;
        }
        
        public override bool IsLegalMove(Vector2Int newPosition)
        {
            return base.IsLegalMove(newPosition) 
                   && Diagonal(newPosition) || VerticalOrHorizontal(newPosition)
                   && !Blocked(newPosition) 
                   && !PutsKingInCheck(newPosition);
        }
    }
}