using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chess
{
    public class ChessBoard
    {
        public List<ChessPiece> ChessPieces => _chessPieces;
        List<ChessPiece> _chessPieces;

        public Stack<Move> MoveHistory => _moveHistory;
        Stack<Move> _moveHistory;

        public ChessBoard()
        {
            _chessPieces = new List<ChessPiece>();
            _moveHistory = new Stack<Move>();
        }

        public void AddToMoveHistory(Move move)
        {
            MoveHistory.Push(move);
        }

        public Move RemoveFromMoveHistory()
        {
            return MoveHistory.Pop();
        }

        public void SetupStandard()
        {
            SetupRoyalRow(7, ChessPieceColor.Black);
            SetupPawnRow(6, ChessPieceColor.Black);
            
            SetupRoyalRow(0, ChessPieceColor.White);
            SetupPawnRow(1, ChessPieceColor.White);
        }

        void SetupRoyalRow(int row, ChessPieceColor color)
        {
            AddPiece(new Rook(color, new Vector2Int(0, row)));
            AddPiece(new Knight(color, new Vector2Int(1, row)));
            AddPiece(new Bishop(color, new Vector2Int(2, row)));
            AddPiece(new Queen(color, new Vector2Int(3, row)));
            AddPiece(new King(color, new Vector2Int(4, row)));
            AddPiece(new Bishop(color, new Vector2Int(5, row)));
            AddPiece(new Knight(color, new Vector2Int(6, row)));
            AddPiece(new Rook(color, new Vector2Int(7, row)));
        }

        void SetupPawnRow(int row, ChessPieceColor color)
        {
            for (int i = 0; i < 8; i++)
            {
                AddPiece(new Pawn(color, new Vector2Int(i, row)));
            }
        }

        public List<Move> AllPossibleMoves(ChessPieceColor color)
        {
            List<Move> moves = new List<Move>();
            foreach (ChessPiece piece in ChessPieces.FindAll(p => p.Color == color))
            {
                moves.AddRange(piece.GetPossibleMoves());
            }

            return moves;
        }

        public void AddPiece(ChessPiece newPiece)
        {
            newPiece.Board = this;
            _chessPieces.Add(newPiece);
        }

        public void RemovePiece(ChessPiece pieceToRemove)
        {
            pieceToRemove.Board = null;
            _chessPieces.Remove(pieceToRemove);
        }

        public ChessPiece GetPieceAt(Vector2Int position)
        {
            return ChessPieces.FirstOrDefault(p => p.Position == position);
        }

    }
}