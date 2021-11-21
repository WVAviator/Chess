using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public abstract class GlidingPiece : ChessPiece
    {
        protected GlidingPiece(ChessPieceColor color, Vector2Int position) : base(color, position)
        {
        }
        
        protected bool Diagonal(Vector2Int movePosition) => Math.Abs(movePosition.y - Position.y) == Math.Abs(movePosition.x - Position.x);
        protected bool VerticalOrHorizontal(Vector2Int movePosition) => movePosition.x - Position.x == 0 || movePosition.y - Position.y == 0;
        protected bool Blocked(Vector2Int movePosition)
        {
            int xDifference = movePosition.x - Position.x;
            int yDifference = movePosition.y - Position.y;
            int xDirection = Math.Sign(xDifference);
            int yDirection = Math.Sign(yDifference);
            
            int positionsToCheck = Math.Max(Math.Abs(xDifference), Math.Abs(yDifference));
            for (int i = 1; i < positionsToCheck; i++)
            {
                Vector2Int positionToCheck = new
                    Vector2Int(Position.x + xDirection * i, Position.y + yDirection * i);
                if (AnyPieceInPosition(positionToCheck)) return true;
            }

            return false;
        }

        protected List<Vector2Int> GetPotentialDiagonalPositions()
        {
            int x = Position.x;
            int y = Position.y;
            List<Vector2Int> positions = new List<Vector2Int>();

            int blocked = 0;
            
            for (int i = 1; i < 8; i++)
            {
                if ((blocked & 1) != 1 && x + i < 8 && y + i < 8)
                {
                    if (!AllyInPosition(x+i, y+i)) positions.Add(new Vector2Int(x + i, y + i));
                    if (AnyPieceInPosition(x + i, y + i)) blocked |= 1;
                }

                if ((blocked & 2) != 2 && x - i >= 0 && y - i >= 0)
                {
                    if (!AllyInPosition(x-i, y-i)) positions.Add(new Vector2Int(x - i, y - i));
                    if (AnyPieceInPosition(x - i, y - i)) blocked |= 2;
                }

                if ((blocked & 4) != 4 && x + i < 8 && y - i >= 0)
                {
                    if (!AllyInPosition(x+i, y-i)) positions.Add(new Vector2Int(x + i, y - i));
                    if (AnyPieceInPosition(x + i, y - i)) blocked |= 4;
                }

                if ((blocked & 8) != 8 && x - i >= 0 && y + i < 8)
                {
                    if (!AllyInPosition(x-i, y+i)) positions.Add(new Vector2Int(x - i, y + i));
                    if (AnyPieceInPosition(x - i, y + i)) blocked |= 8;
                }
            }

            return positions;
        }

        protected List<Vector2Int> GetPotentialVerticalHorizontalPositions()
        {
            int x = Position.x;
            int y = Position.y;
            List<Vector2Int> positions = new List<Vector2Int>();

            int blocked = 0;
            
            for (int i = 1; i < 8; i++)
            {
                if ((blocked & 1) != 1 && x + i < 8)
                {
                    if (!AllyInPosition(x + i, y)) positions.Add(new Vector2Int(x + i, y));
                    if (AnyPieceInPosition(x + i, y)) blocked |= 1;
                }

                if ((blocked & 2) != 2 && x - i >= 0)
                {
                    if (!AllyInPosition(x - i, y)) positions.Add(new Vector2Int(x - i, y));
                    if (AnyPieceInPosition(x - i, y)) blocked |= 2;
                }

                if ((blocked & 4) != 4 && y + i < 8)
                {
                    if (!AllyInPosition(x, y + i)) positions.Add(new Vector2Int(x, y + i));
                    if (AnyPieceInPosition(x, y + i)) blocked |= 4;
                }

                if ((blocked & 8) != 8 && y - i >= 0)
                {
                    
                    if (!AllyInPosition(x, y- i)) positions.Add(new Vector2Int(x, y - i));
                    if (AnyPieceInPosition(x, y - i)) blocked |= 8;
                }
            }
            
            return positions;
        }
    }
}