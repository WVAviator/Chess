using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public class Pawn : ChessPiece
    {
        int _startingRow;
        int _movementDirection;
        
        public override string PieceName => "Pawn";
        
        public override char PieceChar
        {
            get
            {
                return Color == ChessPieceColor.Black ? 'p' : 'P';
            }
        }
        public Pawn(ChessPieceColor color, Vector2Int position = default) : base(color, position)
        {
            _startingRow = color == ChessPieceColor.Black ? 6 : 1;
            _movementDirection = color == ChessPieceColor.Black ? -1 : 1;
        }
        
        public Pawn(ChessPieceColor color) : base(color, default)
        {
            _startingRow = color == ChessPieceColor.Black ? 6 : 1;
            _movementDirection = color == ChessPieceColor.Black ? -1 : 1;
        }

        public override int GetScore() => 1;

        bool PawnMovementIsLegal(Vector2Int movePosition)
        {
            if (MovingDiagonally(movePosition))
                return OpponentInPosition(movePosition) || Board.EnPassant == movePosition;
            if (MovingForward(movePosition)) return !AnyPieceInPosition(movePosition);
            if (MovingTwoSpaces(movePosition)) return 
                !AnyPieceInPosition(new Vector2Int(Position.x, Position.y + _movementDirection)) &&
                !AnyPieceInPosition(movePosition);
            
            return false;
        }

        bool MovingTwoSpaces(Vector2Int movePosition)
        {
            return Position.y == _startingRow && Mathf.Abs(movePosition.y - Position.y) == 2;
        }
        bool MovingForward(Vector2Int movePosition)
        {
            return movePosition.y == Position.y + _movementDirection;
        }
        bool MovingDiagonally(Vector2Int movePosition)
        {
            return Mathf.Abs(movePosition.x - Position.x) == 1 && movePosition.y - Position.y == _movementDirection;
        }
   
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
    }
}