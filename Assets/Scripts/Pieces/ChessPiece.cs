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


        public ChessBoard Board { get; set; }

        static bool checkForCheck = false;

        public abstract string PieceName { get; }
        public event Action<ChessPiece, Vector2Int> OnPieceMoved;
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
            Move.OnMoveExecutedOrUndone += ClearPotentialPositions;
        }
        public void MoveTo(Vector2Int newPosition)
        {
            if (!IsValidPosition(newPosition))
            {
                Debug.LogError($"Attempted to move chess piece to an invalid location at {newPosition.ToString()}");
                return;
            }
            Position = newPosition;
            OnPieceMoved?.Invoke(this, newPosition);
        }
        static bool IsValidPosition(Vector2Int potentialPosition)
        {
            bool isValid = potentialPosition.x >= 0 && potentialPosition.y >= 0 
                          && potentialPosition.x <= 7 && potentialPosition.y <= 7;
            return isValid;
        }

        public virtual bool IsLegalMove(Vector2Int newPosition)
        {
            if (!IsValidPosition(newPosition)) return false;
            if (newPosition == Position) return false;
            if (AllyInPosition(newPosition)) return false;
            
            return true;
        }

        public bool IsMyTurn() => Board.PlayerTurn == Color;

        public HashSet<Move> GetPossibleMoves()
        {
            HashSet<Move> legalMoves = new HashSet<Move>();
            if (!IsMyTurn()) return legalMoves;
            
            foreach (Vector2Int pos in PotentialPositions)
            {
                if (!PutsKingInCheck(pos)) legalMoves.Add(new Move(this, pos, true));
            }
            return legalMoves;
        }

        List<Vector2Int> _potentialPositions;
        protected List<Vector2Int> PotentialPositions
        {
            get { return _potentialPositions ??= GetPotentialPositions(); }
        }
        
        public void ClearPotentialPositions()
        {
            _potentialPositions = null;
        }
        protected abstract List<Vector2Int> GetPotentialPositions();

        protected bool OpponentInPosition(Vector2Int position) => PieceInPosition(Color.Opponent(), position.x, position.y);
        protected bool OpponentInPosition(int x, int y) => PieceInPosition(Color.Opponent(), x, y);
        protected bool AllyInPosition(Vector2Int position) => PieceInPosition(Color, position.x, position.y);
        protected bool AllyInPosition(int x, int y) => PieceInPosition(Color, x, y);
        protected bool AnyPieceInPosition(Vector2Int position) => Board?[position.x, position.y] != null;
        protected bool AnyPieceInPosition(int x, int y) => Board?[x, y] != null;
        bool PieceInPosition(ChessPieceColor color, int x, int y)
        {
            ChessPiece piece = Board?[x, y];
            if (piece == null) return false;
            return piece.Color == color;
        }

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

        protected bool PutsKingInCheck(Vector2Int newPosition)
        {
            King king = Board.GetKing(Color);
            if (king == null) return false;

            if (checkForCheck) return false;
            checkForCheck = true;

            Vector2Int oldPosition = Position;
            
            bool inCheck = false;
            
            ChessPiece targetOpponent = Board[newPosition.x, newPosition.y];
            
            if (targetOpponent != null) Board.RemovePiece(targetOpponent);
            MoveTo(newPosition);
            Board.PlayerTurn = Board.PlayerTurn.Opponent();

            foreach (ChessPiece piece in Board.ChessPiecesByColor(Color.Opponent()))
            {
                if (piece.IsLegalMove(king.Position)) inCheck = true;
            }
            
            Board.PlayerTurn = Board.PlayerTurn.Opponent();
            MoveTo(oldPosition);
            if (targetOpponent != null) Board[newPosition.x, newPosition.y] = targetOpponent;

            checkForCheck = false;
            return inCheck;
        }
    }
}