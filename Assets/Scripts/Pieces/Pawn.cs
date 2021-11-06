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
        
        public Pawn(ChessPieceColor color) : base(color, default)
        {
            _startingRow = color == ChessPieceColor.Black ? 6 : 1;
            _movementDirection = color == ChessPieceColor.Black ? -1 : 1;
        }

        public override int GetScore() => 1;

        public override bool IsLegalMove(Move moveToCheck)
        {
            if (IsPromotionMove(moveToCheck) && CanMoveForward(moveToCheck))
            {
                moveToCheck.IsPromotion = true;
                return true;
            }
            
            
            if (CanMoveForward(moveToCheck)) return true;
            if (CanTakeOpponentEnPassant(moveToCheck)) return true;
            if (CanTakeOpponentDiagonally(moveToCheck)) return true;
            if (CanMoveTwoSpacesOnFirstTurn(moveToCheck)) return true;
            
            return false;
        }

        bool IsPromotionMove(Move moveToCheck) => moveToCheck.NewPosition.y == _startingRow + _movementDirection * 6;

        bool CanTakeOpponentEnPassant(Move move)
        {
            if (Board.MoveHistory.Count == 0) return false;
            Move previousMove = Board.MostRecentMove();
            if (previousMove.ChessPiece is Pawn &&
                MovedByTwoSpaces(previousMove) &&
                MoveForwardBy(1, move) &&
                MoveLaterallyBy(1, move) &&
                MoveInBehind(move, previousMove))
            {
                move.TargetOpponent = previousMove.ChessPiece;
                return true;
            }
            return false;
        }

        bool MoveInBehind(Move move, Move previousMove) => move.NewPosition == previousMove.NewPosition + new Vector2Int(0, _movementDirection);

        static bool MovedByTwoSpaces(Move previousMove) => Mathf.Abs(previousMove.YDifference) == 2;

        bool CanMoveTwoSpacesOnFirstTurn(Move move) =>
            MoveForwardBy(2, move) && MoveLaterallyBy(0, move) && FirstMove() &&
            !AnyPieceInPosition(move.NewPosition) && !AnyPieceBlocking();

        bool CanTakeOpponentDiagonally(Move move) =>
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