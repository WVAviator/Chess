using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public class Pawn : ChessPiece
    {
        int _startingRow;
        int _movementDirection;
        
        public override string PieceName => "Pawn";
        public Pawn(ChessPieceColor color, Vector2Int position = default) : base(color, position)
        {
            _startingRow = color == ChessPieceColor.Black ? 6 : 1;
            _movementDirection = color == ChessPieceColor.Black ? -1 : 1;
        }
        
        public override bool IsLegalMove(Move moveToCheck)
        {
            
            if (CanMoveForward(moveToCheck)) return true;
            if (CanTakeOpponentPiece(moveToCheck)) return true;
            if (CanJumpTwoSpacesOnFirstTurn(moveToCheck)) return true;
            
            return false;
        }

        bool CanJumpTwoSpacesOnFirstTurn(Move move) =>
            MoveForwardBy(2, move) && MoveLaterallyBy(0, move) && FirstMove() &&
            !AnyPieceInPosition(move.NewPosition) && !AnyPieceBlocking();

        bool CanTakeOpponentPiece(Move move) =>
            MoveForwardBy(1, move) && MoveLaterallyBy(1, move) &&
            OpponentInPosition(move.NewPosition);

        bool CanMoveForward(Move move) => 
            MoveForwardBy(1, move) && MoveLaterallyBy(0, move) && !AnyPieceBlocking();

        bool MoveForwardBy(int spaces, Move move) => move.YDifference == _movementDirection * spaces;
        bool MoveLaterallyBy(int spaces, Move move) => move.XDifference == -spaces || move.XDifference == spaces;
        bool FirstMove() => Position.y == _startingRow;
        bool AnyPieceBlocking() => AnyPieceInPosition(new Vector2Int(Position.x, Position.y + _movementDirection));
    }
}