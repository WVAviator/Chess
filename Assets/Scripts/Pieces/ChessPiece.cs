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
        public abstract char PieceChar { get; }
        
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

        protected ChessPiece(ChessPieceColor color, Vector2Int position)
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
        
        public HashSet<Move> GetPossibleMoves()
        {
            HashSet<Move> legalMoves = new HashSet<Move>();
            HashSet<Move> potentialMoves = GetPotentialMoves();
            foreach (Move move in potentialMoves)
            {
                if (move.IsLegal()) legalMoves.Add(move);
            }
            return legalMoves;
        }

        protected abstract HashSet<Move> GetPotentialMoves();

        protected bool OpponentInPosition(Vector2Int position) => PieceInPosition(Color.Opponent(), position.x, position.y);
        protected bool AllyInPosition(Vector2Int position) => PieceInPosition(Color, position.x, position.y);
        protected bool AnyPieceInPosition(Vector2Int position) => Board?[position.x, position.y] != null;
        bool PieceInPosition(ChessPieceColor color, int x, int y) => Board?[x, y]?.Color == color;
        
        public Move To(int x, int y) => new Move(this, new Vector2Int(x, y));

        public static ChessPiece FromChar(char c)
        {
            switch (c)
            {
                case 'P':
                    return new Pawn(ChessPieceColor.White);
                case 'p':
                    return new Pawn(ChessPieceColor.Black);
                case 'R':
                    return new Rook(ChessPieceColor.White);
                case 'r':
                    return new Rook(ChessPieceColor.Black);
                case 'N':
                    return new Knight(ChessPieceColor.White);
                case 'n':
                    return new Knight(ChessPieceColor.Black);
                case 'B':
                    return new Bishop(ChessPieceColor.White);
                case 'b':
                    return new Bishop(ChessPieceColor.Black);
                case 'Q':
                    return new Queen(ChessPieceColor.White);
                case 'q':
                    return new Queen(ChessPieceColor.Black);
                case 'K':
                    return new King(ChessPieceColor.White);
                case 'k':
                    return new King(ChessPieceColor.Black);
                default:
                    Debug.LogError($"Invalid piece character: \"{c}\"");
                    return null;
            }
        }
    }
}