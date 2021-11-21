using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public class Knight : ChessPiece
    {
        public override string PieceName => "Knight";
        
        public override char PieceChar => Color == ChessPieceColor.Black ? 'n' : 'N';

        public Knight(ChessPieceColor color) : base(color, default)
        {
        }

        public override int GetScore() => 3;

        protected override List<Vector2Int> GetPotentialPositions()
        {
            int x = Position.x;
            int y = Position.y;
            List<Vector2Int> positions = new List<Vector2Int>();
            
            if (x + 1 < 8 && y + 2 < 8 && !AllyInPosition(x+1, y + 2)) positions.Add(new Vector2Int(x + 1, y + 2));
            if (x + 1 < 8 && y - 2 >= 0 && !AllyInPosition(x+1, y - 2)) positions.Add(new Vector2Int(x + 1, y - 2));
            if (x - 1 >= 0 && y + 2 < 8 && !AllyInPosition(x-1, y + 2)) positions.Add(new Vector2Int(x - 1, y + 2));
            if (x - 1 >= 0 && y - 2 >= 0 && !AllyInPosition(x-1, y - 2)) positions.Add(new Vector2Int(x - 1, y - 2));
            if (x + 2 < 8 && y + 1 < 8 && !AllyInPosition(x+2, y + 1)) positions.Add(new Vector2Int(x + 2, y + 1));
            if (x + 2 < 8 && y - 1 >= 0 && !AllyInPosition(x+2, y -1)) positions.Add(new Vector2Int(x + 2, y - 1));
            if (x - 2 >= 0 && y + 1 < 8 && !AllyInPosition(x-2, y + 1)) positions.Add(new Vector2Int(x - 2, y + 1));
            if (x - 2 >= 0 && y - 1 >= 0 && !AllyInPosition(x-2, y - 1)) positions.Add(new Vector2Int(x - 2, y - 1));

            return positions;
        }
        
        public override bool IsLegalMove(Vector2Int newPosition)
        {
            return base.IsLegalMove(newPosition) 
                   && IsLShaped(newPosition) 
                   && !PutsKingInCheck(newPosition);
        }

        bool IsLShaped(Vector2Int newPosition)
        {
            int xDifference = Math.Abs(newPosition.x - Position.x);
            int yDifference = Math.Abs(newPosition.y - Position.y);

            return xDifference == 2 && yDifference == 1
                   || xDifference == 1 && yDifference == 2;
        }
    }
}