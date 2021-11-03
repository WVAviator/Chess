using System;
using System.Linq;
using UnityEngine;

namespace Chess
{
    public class Move
    {
        public Vector2Int NewPosition => _newPosition;
        readonly Vector2Int _newPosition;
        public Vector2Int OldPosition => _oldPosition;
        readonly Vector2Int _oldPosition;

        Rook _castleRook;
        bool _isCastle;

        public int YDifference { get; }
        public int XDifference { get; }
        public int YDirection { get; }
        public int XDirection { get; }

        public ChessPiece ChessPiece { get; }
        
        public ChessPiece TargetOpponent { get; set; }

        bool _isExecuted;
        Vector2Int _castleRookStartPosition;

        public Move(ChessPiece chessPiece, Vector2Int newPosition)
        {
            _newPosition = newPosition;
            ChessPiece = chessPiece;
            _oldPosition = chessPiece.Position;
            
            if (!IsValidPosition(_newPosition)) _newPosition = chessPiece.Position;

            YDifference = _newPosition.y - _oldPosition.y;
            XDifference = _newPosition.x - _oldPosition.x;
            YDirection = Math.Sign(YDifference);
            XDirection = Math.Sign(XDifference);
            
            TargetOpponent = ChessPiece.Board?
                .ChessPiecesByColor(ChessPiece.Color.Opponent())
                .FirstOrDefault(p => p.Position == NewPosition);
        }

        public bool IsLegal()
        {
            return !PutsKingInCheck() && ChessPiece.IsLegalMove(this) && IsMyTurn();
        }

        bool IsMyTurn() => ChessPiece.Board.PlayerTurn == ChessPiece.Color;
        

        public void Execute()
        {
            if (!IsLegal())
            {
                Debug.LogError($"Attempted to execute illegal move: {ChessPiece.GetType().Name} at {ChessPiece.Position} cannot move to {NewPosition}");
                return;
            }
            ChessPiece.MoveTo(NewPosition);
            if (_isCastle) _castleRook.MoveTo(NewPosition + new Vector2Int(-XDirection, 0));
            

            if (TargetOpponent != null) ChessPiece.Board.RemovePiece(TargetOpponent);
            
            ChessPiece.Board.AddToMoveHistory(this);
            _isExecuted = true;
        }

        public void Undo()
        {
            if (!_isExecuted) return;
            _isExecuted = false;
            
            if (ChessPiece.Board.MoveHistory.Peek() != this)
            {
                Debug.LogError($"Attempted to undo moves out of order! Move {ChessPiece.GetType().Name} to {NewPosition} is not the most recent move.");
                return;
            }
            ChessPiece.MoveTo(_oldPosition);
            if (_isCastle) _castleRook.MoveTo(_castleRookStartPosition);
            if (TargetOpponent != null) ChessPiece.Board.AddPiece(TargetOpponent);
            ChessPiece.Board.RemoveFromMoveHistory();
        }
        
        static bool IsValidPosition(Vector2Int potentialPosition)
        {
            bool isValid = potentialPosition.x >= 0 && potentialPosition.y >= 0 
                           && potentialPosition.x <= 7 && potentialPosition.y <= 7;
            return isValid;
        }

        bool PutsKingInCheck()
        {
            King king = (King)ChessPiece.Board.ChessPieces.Find(p => p is King && p.Color == ChessPiece.Color);
            if (king == null) return false;
            
            bool inCheck = false;
            
            ChessPiece.MoveTo(NewPosition);
            
            foreach (ChessPiece piece in ChessPiece.Board.ChessPiecesByColor(ChessPiece.Color.Opponent()))
            {
                if (piece == TargetOpponent) continue;
                if (piece.IsLegalMove(king.Position)) inCheck = true;
            }
            ChessPiece.MoveTo(_oldPosition);

            return inCheck;
        }

        public void IsCastle(Rook rook)
        {
            _isCastle = true;
            _castleRook = rook;
            _castleRookStartPosition = rook.Position;
        }
    }
}