using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chess
{
    public class Queen : GlidingPiece
    {
        
        public override char PieceChar
        {
            get
            {
                return Color == ChessPieceColor.Black ? 'q' : 'Q';
            }
        }
        public override string PieceName => "Queen";
        public Queen(ChessPieceColor color, Vector2Int position = default) : base(color, position)
        {
        }
        
        public Queen(ChessPieceColor color) : base(color, default)
        {
        }

        public override int GetScore() => 9;

        public override bool IsLegalMove(Vector2Int movePosition)
        {
            return base.IsLegalMove(movePosition) &&
                   (Diagonal(movePosition) || VerticalOrHorizontal(movePosition)) &&
                   !Blocked(movePosition) &&
                   !PutsKingInCheck(movePosition);
        }

        protected override List<Vector2Int> GetPotentialPositions()
        {
            List<Vector2Int> positions = new List<Vector2Int>();
            positions.AddRange(GetPotentialDiagonalPositions());
            positions.AddRange(GetPotentialVerticalHorizontalPositions());
            return positions;
        }
    }
}