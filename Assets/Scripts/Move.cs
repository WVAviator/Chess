﻿using System;
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
        public int YDifference { get; }
        public int XDifference { get; }
        public int YDirection { get; }
        public int XDirection { get; }

        ChessPiece _chessPiece;
        ChessPiece _targetOpponent;

        public Move(ChessPiece piece, Vector2Int newPosition)
        {
            _newPosition = newPosition;
            _chessPiece = piece;
            _oldPosition = piece.Position;
            
            if (!IsValidPosition(_newPosition)) _newPosition = piece.Position;

            YDifference = _newPosition.y - piece.Position.y;
            XDifference = _newPosition.x - piece.Position.x;
            YDirection = Math.Sign(YDifference);
            XDirection = Math.Sign(XDifference);
            
            _targetOpponent = _chessPiece.Board?.ChessPieces.FirstOrDefault(p => p.Position == NewPosition);
        }
        public bool IsLegal() => _chessPiece.IsLegalMove(this);

        public void Execute()
        {
            if (!IsLegal())
            {
                Debug.LogError($"Attempted to execute illegal move: {_chessPiece.GetType().Name} at {_chessPiece.Position} cannot move to {NewPosition}");
                return;
            }
            _chessPiece.MoveTo(NewPosition);

            if (_targetOpponent != null) _chessPiece.Board.RemovePiece(_targetOpponent);
            
            _chessPiece.Board.AddToMoveHistory(this);

        }

        public void Undo()
        {
            if (_chessPiece.Board.MoveHistory.Peek() != this)
            {
                Debug.LogError($"Attempted to undo moves out of order! Move {_chessPiece.GetType().Name} to {NewPosition} is not the most recent move.");
                return;
            }
            _chessPiece.MoveTo(OldPosition);
            if (_targetOpponent != null) _chessPiece.Board.AddPiece(_targetOpponent);
            _chessPiece.Board.RemoveFromMoveHistory();
        }
        
        static bool IsValidPosition(Vector2Int potentialPosition)
        {
            bool isValid = potentialPosition.x >= 0 && potentialPosition.y >= 0 
                                                    && potentialPosition.x <= 7 && potentialPosition.y <= 7;
            return isValid;
        }
    }
}