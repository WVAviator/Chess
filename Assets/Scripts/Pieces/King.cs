using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chess
{
    public class King : ChessPiece
    {
        public King(ChessPieceColor color) : base(color, default)
        {
        }

        public override char PieceChar => Color == ChessPieceColor.Black ? 'k' : 'K';

        public override string PieceName => "King";

        public override int GetScore() => 100;

        protected override List<Vector2Int> GetPotentialPositions()
        {
            int x = Position.x;
            int y = Position.y;
            List<Vector2Int> positions = new List<Vector2Int>();

            if (!IsMyTurn()) return positions;

            if (x + 1 < 8)
            {
                if (y + 1 < 8 && !AllyInPosition(x+1,y+1)) positions.Add(new Vector2Int(x+1, y+1));
                if (y - 1 >= 0 && !AllyInPosition(x+1,y-1)) positions.Add(new Vector2Int(x+1, y-1));
                if (!AllyInPosition(x+1, y)) positions.Add(new Vector2Int(x+1, y));
            }
            if (x - 1 >= 0)
            {
                if (y + 1 < 8 && !AllyInPosition(x-1,y+1)) positions.Add(new Vector2Int(x-1, y+1));
                if (y - 1 >= 0 && !AllyInPosition(x-1,y-1)) positions.Add(new Vector2Int(x-1, y-1));
                if (!AllyInPosition(x-1,y)) positions.Add(new Vector2Int(x-1, y));
            }
            if (y + 1 < 8 && !AllyInPosition(x,y+1)) positions.Add(new Vector2Int(x, y+1));
            if (y - 1 >= 0 && !AllyInPosition(x,y-1)) positions.Add(new Vector2Int(x, y-1));

            if (x == 4 && (y == 0 || y == 7))
            {
                if (Board.CanCastle(this, -1) && !CastleMoveBlocked(-1)) positions.Add(new Vector2Int(x - 2, y));
                if (Board.CanCastle(this, 1) && !CastleMoveBlocked(1)) positions.Add(new Vector2Int(x + 2, y));
            }
            
            return positions;
        }

        bool CastleMoveBlocked(int xDirection)
        {
            Vector2Int transitPosition = new Vector2Int(Position.x + xDirection, Position.y);
            if (AnyPieceInPosition(transitPosition)) return true;
            if (OpponentInPosition(transitPosition.x + xDirection, Position.y)) return true;
            if (PutsKingInCheck(transitPosition)) return true;

            return false;
        }
        
        public override bool IsLegalMove(Vector2Int newPosition)
        {
            return base.IsLegalMove(newPosition) 
                   && (IsOneSpaceMove(newPosition) || IsCastle(newPosition))
                   && !PutsKingInCheck(newPosition);
        }

        bool IsCastle(Vector2Int newPosition)
        {
            int xDifference = newPosition.x - Position.x;
            int yDifference = newPosition.y - Position.y;

            if (Math.Abs(xDifference) != 2 || yDifference != 0) return false;
            
            int xDirection = Math.Sign(xDifference);

            return Board.CanCastle(this, xDirection) &&
                   !CastleMoveBlocked(xDirection);
        }

        bool IsOneSpaceMove(Vector2Int newPosition)
        {
            return Math.Abs(newPosition.x - Position.x) <= 1 && Math.Abs(newPosition.y - Position.y) <= 1;
        }
    }
}