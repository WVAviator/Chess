using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine;

namespace Chess
{
    public abstract class ChessPiece
    {
        public ChessPieceColor Color { get; }
        
        public ChessBoard Board;

        public abstract string PieceName { get; }
        public event Action<Vector2Int> OnPieceMoved;
        public Vector2Int Position
        {
            get => _position;
            set
            {
                _position = value;
                if (!IsValidPosition(_position)) _position = Vector2Int.zero;
            }
        }
        Vector2Int _position;

        public abstract int GetScore();

        public ChessPiece(ChessPieceColor color, Vector2Int position)
        {
            Color = color;
            Position = position;
        }
        public void MoveTo(Vector2Int newPosition)
        {
            if (!IsValidPosition(newPosition))
            {
                Debug.LogError($"Attempted to move chess piece to an invalid location at {newPosition.ToString()}");
                return;
            }
            Position = newPosition;
            OnPieceMoved?.Invoke(newPosition);
        }
        static bool IsValidPosition(Vector2Int potentialPosition)
        {
            bool isValid = potentialPosition.x >= 0 && potentialPosition.y >= 0 
                          && potentialPosition.x <= 7 && potentialPosition.y <= 7;
            return isValid;
        }

        public bool IsLegalMove(Vector2Int newPosition) => IsLegalMove(new Move(this, newPosition));

        public bool IsMyTurn() => Board.PlayerTurn == Color;

        public abstract bool IsLegalMove(Move move);
        
        public List<Move> GetPossibleMoves()
        {
            List<Move> moves = new List<Move>();

            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    Vector2Int possiblePosition = new Vector2Int(x, y);

                    Move move = new Move(this, possiblePosition);

                    if (move.IsLegal()) moves.Add(move);
                }
            }
            return moves;
        }

        protected bool OpponentInPosition(Vector2Int position)
        {
            if (Board?.ChessPiecesByColor(Color.Opponent()).FirstOrDefault(p => p. Position == position) != null) return true;
            return false;
        }
        protected bool AllyInPosition(Vector2Int position)
        {
            if (Board?.ChessPiecesByColor(Color).FirstOrDefault(p => p.Position == position) != null) return true;
            return false;
        }
        protected bool AnyPieceInPosition(Vector2Int position)
        {
            if (Board?.ChessPieces.FirstOrDefault(p => p.Position == position) != null) return true;
            return false;
        }
        
        public Move To(int x, int y) => new Move(this, new Vector2Int(x, y));
    }
}