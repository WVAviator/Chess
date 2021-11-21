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
        
        public King(ChessPieceColor color, Vector2Int position) : base(color, position)
        {
        }
        
        public override char PieceChar
        {
            get
            {
                return Color == ChessPieceColor.Black ? 'k' : 'K';
            }
        }

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
                if (Board.CanCastle(this, -1) && !AnyPieceBetweenThisAnd(new Vector2Int(0, y))) positions.Add(new Vector2Int(x - 2, y));
                if (Board.CanCastle(this, 1) && !AnyPieceBetweenThisAnd(new Vector2Int(7, y))) positions.Add(new Vector2Int(x + 2, y));
            }
            
            return positions;
        }

        bool IsCastle(Vector2Int movePosition)
        {
            if (Math.Abs(movePosition.x - Position.x) != 2 || movePosition.y - Position.y != 0) return false;
            if (!(Board.GetPieceAt(new Vector2Int(GetCorner(movePosition), Position.y)) is Rook rook)) return false;
            if (AnyPieceBetweenThisAnd(rook.Position)) return false;

            return true;
        }

        bool AnyPieceBetweenThisAnd(Vector2Int rookPosition)
        {
            int xDirection = Math.Sign(rookPosition.x - Position.x);
            int x = Position.x + xDirection;
            while (x != rookPosition.x)
            {
                Vector2Int position = new Vector2Int(x, Position.y);
                if (AnyPieceInPosition(position)) return true;
                if (PutsKingInCheck(position)) return true;
                x += xDirection;
            }
            return false;
        }

        bool PutsInCheck(Vector2Int position)
        {
            return Board.ChessPiecesByColor(Color.Opponent())
                .Any(piece => piece.IsLegalMove(position));
        }

        int GetCorner(Vector2Int movePosition) => Math.Sign(movePosition.x - Position.x) == 1 ? 7 : 0;

        bool OneSpace(Vector2Int movePosition) => Math.Abs(movePosition.x - Position.x) <= 1 && Math.Abs(movePosition.y - Position.y) <= 1;
    }
}