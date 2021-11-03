using System;
using UnityEngine;

namespace Chess
{
    public class King : ChessPiece
    {
        public King(ChessPieceColor color, Vector2Int position = default) : base(color, position)
        {
        }

        public override string PieceName => "King";

        public override bool IsLegalMove(Move move)
        {
            if (move.NewPosition == Position) return false;
            if (AllyInPosition(move.NewPosition)) return false;
            return IsCastle(move) || OneSpace(move);
        }

        bool IsCastle(Move move)
        {
            if (Board.HasMoved(this)) return false;
            if (Math.Abs(move.XDifference) != 2 || move.YDifference != 0) return false;
            if (!(Board.GetPieceAt(new Vector2Int(GetCorner(move), Position.y)) is Rook rook)) return false;
            if (Board.HasMoved(rook)) return false;
            if (AnyPieceBetweenThisAnd(rook.Position)) return false;
            move.IsCastle(rook);
            return true;
        }

        bool AnyPieceBetweenThisAnd(Vector2Int rookPosition)
        {
            int xDirection = Math.Sign(rookPosition.x - Position.x);
            int x = Position.x + xDirection;
            while (x != rookPosition.x)
            {
                if (AnyPieceInPosition(new Vector2Int(x, Position.y))) return true;
                x += xDirection;
            }
            return false;
        }

        int GetCorner(Move move) => move.XDirection == 1 ? 7 : 0;

        static bool OneSpace(Move move) => Math.Abs(move.XDifference) <= 1 && Math.Abs(move.YDifference) <= 1;
    }
}