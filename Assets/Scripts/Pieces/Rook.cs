using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public class Rook : GlidingPiece
    {
        public override string PieceName => "Rook";
        public override char PieceChar => Color == ChessPieceColor.Black ? 'r' : 'R';

        public Rook(ChessPieceColor color) : base(color, default)
        {
        }
        
        public Rook(ChessPieceColor color, Vector2Int position = default) : base(color, position)
        {
        }

        public override int GetScore() => 5;

        protected override List<Vector2Int> GetPotentialPositions() => GetPotentialVerticalHorizontalPositions();
    }
}