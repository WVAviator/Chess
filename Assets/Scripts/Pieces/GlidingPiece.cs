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
        
        protected static bool Diagonal(Move move) => Math.Abs(move.YDifference) == Math.Abs(move.XDifference);
        
        protected static bool VerticalOrHorizontal(Move move) => move.XDifference == 0 || move.YDifference == 0;
        protected bool Blocked(Move move)
        {
            int positionsToCheck = Math.Max(Math.Abs(move.XDifference), Math.Abs(move.YDifference));
            for (int i = 1; i < positionsToCheck; i++)
            {
                Vector2Int positionToCheck = new
                    Vector2Int(Position.x + move.XDirection * i, Position.y + move.YDirection * i);
                if (AnyPieceInPosition(positionToCheck)) return true;
            }

            return false;
        }
        
        protected HashSet<Move> GetPotentialDiagonalMoves()
        {
            int x = Position.x;
            int y = Position.y;
            HashSet<Move> moves = new HashSet<Move>();
            
            for (int i = 1; i < 8; i++)
            {
                if (x + i < 8 && y + i < 8) moves.Add(To(x + i, y + i));
                if (x - i >= 0 && y - i >= 0) moves.Add(To(x - i, y - i));
                if (x + i < 8 && y - i >= 0) moves.Add(To(x + i, y - i));
                if (x - i >= 0 && y + i < 8) moves.Add(To(x - i, y + i));
            }

            return moves;
        }

        protected HashSet<Move> GetPotentialVerticalHorizontalMoves()
        {
            int x = Position.x;
            int y = Position.y;
            HashSet<Move> moves = new HashSet<Move>();
            
            for (int i = 1; i < 8; i++)
            {
                if (x + i < 8) moves.Add(To(x + i, y));
                if (x - i >= 0) moves.Add(To(x - i, y));
                if (y + i < 8) moves.Add(To(x, y + i));
                if (y - i >= 0) moves.Add(To(x, y - i));
            }
            
            return moves;
        }
    }
}