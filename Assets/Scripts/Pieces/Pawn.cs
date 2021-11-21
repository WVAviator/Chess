using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public class Pawn : ChessPiece
    {
        readonly int _startingRow;
        readonly int _movementDirection;
        
        public override string PieceName => "Pawn";
        
        public override char PieceChar => Color == ChessPieceColor.Black ? 'p' : 'P';

        public Pawn(ChessPieceColor color) : base(color, default)
        {
            _startingRow = color == ChessPieceColor.Black ? 6 : 1;
            _movementDirection = color == ChessPieceColor.Black ? -1 : 1;
        }

        public override int GetScore() => 1;

        protected override List<Vector2Int> GetPotentialPositions()
        {
            int x = Position.x;
            int y = Position.y;
            List<Vector2Int> positions = new List<Vector2Int>();

            if (!AnyPieceInPosition(x, y + _movementDirection))
            {
                positions.Add(new Vector2Int(x, y + _movementDirection));
                if (y == _startingRow && !AnyPieceInPosition(x, y + 2 * _movementDirection))
                    positions.Add(new Vector2Int(x, y + 2 * _movementDirection));
            }
            if (x + 1 < 8 && (OpponentInPosition(x + 1, y + _movementDirection) 
                              || Board.EnPassant == new Vector2Int(x + 1, y + _movementDirection)))
                positions.Add(new Vector2Int(x + 1, y + _movementDirection));
            
            if (x - 1 >= 0 && (OpponentInPosition(x - 1, y + _movementDirection) 
                               || Board.EnPassant == new Vector2Int(x - 1, y + _movementDirection)))
                positions.Add(new Vector2Int(x - 1, y + _movementDirection));


            return positions;
        }

        public override bool IsLegalMove(Vector2Int newPosition)
        {
            if (!base.IsLegalMove(newPosition)) return false;
            if (!LegalPawnMove(newPosition)) return false;
            return !PutsKingInCheck(newPosition);
        }

        public bool LegalPawnMove(Vector2Int newPosition)
        {
            if (newPosition.x == Position.x)
            {
                if (newPosition.y == Position.y + _movementDirection)
                    return !AnyPieceInPosition(newPosition);
                if (newPosition.y == Position.y + 2 * _movementDirection && Position.y == _startingRow)
                    return !AnyPieceInPosition(newPosition) && !AnyPieceInPosition(newPosition.x, Position.y + _movementDirection);
            }
            
            if (Mathf.Abs(newPosition.x - Position.x) == 1 && newPosition.y == Position.y + _movementDirection)
                return OpponentInPosition(newPosition) || Board.EnPassant == newPosition;

            return false;
        }

    }
}