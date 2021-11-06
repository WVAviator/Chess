using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chess
{
    public class ChessBoard
    {
        public List<ChessPiece> ChessPieces => _whitePieces.Concat(_blackPieces).ToList();

        public ChessPieceColor PlayerTurn;

        List<ChessPiece> _blackPieces;
        List<ChessPiece> _whitePieces;

        public Stack<Move> MoveHistory => _moveHistory;
        Stack<Move> _moveHistory;

        public event Action<ChessPiece> OnPieceAdded;
        public event Action<ChessPiece> OnPieceRemoved;
        public event Action<ChessPieceColor> OnNewPlayerTurn;

        public ChessPiece this[int x, int y]
        {
            get => GetPieceAt(new Vector2Int(x, y));
            set
            {
                value.Position = new Vector2Int(x, y);
                AddPiece(value);
            }
        }

        public ChessBoard(params ChessPiece[] pieces)
        {
            _blackPieces = new List<ChessPiece>();
            _whitePieces = new List<ChessPiece>();
            _moveHistory = new Stack<Move>();

            PlayerTurn = ChessPieceColor.White;
            
            foreach (ChessPiece p in pieces) AddPiece(p);
        }

        public bool Contains(ChessPiece piece)
        {
            return ChessPieces.Contains(piece);
        }

        public List<ChessPiece> ChessPiecesByColor(ChessPieceColor color)
        {
            return color == ChessPieceColor.Black ? _blackPieces : _whitePieces;
        }

        void NextPlayerTurn()
        {
            PlayerTurn = PlayerTurn.Opponent();
            OnNewPlayerTurn?.Invoke(PlayerTurn);
        }

        public bool IsInCheck(ChessPieceColor color)
        {
            King king = (King)ChessPiecesByColor(color).FirstOrDefault(p => p is King);
            if (king == null) return false;
            
            foreach (ChessPiece piece in ChessPiecesByColor(color.Opponent()))
            {
                if (piece.IsLegalMove(king.Position)) return true;
            }

            return false;
        }

        public bool IsCheckmate(ChessPieceColor color) => AllPossibleMoves(color).Count == 0;

        public void AddToMoveHistory(Move move)
        {
            MoveHistory.Push(move);
            NextPlayerTurn();
            if (IsCheckmate(PlayerTurn)) Debug.Log($"Checkmate! {PlayerTurn.Opponent()} wins!");
        }

        public Move RemoveFromMoveHistory()
        {
            NextPlayerTurn();
            return MoveHistory.Pop();
        }
        
        public Move MostRecentMove() => MoveHistory.Peek();

        public Setup Setup() => new Setup(this);
        
            
        

        void SetupRoyalRow(int row, ChessPieceColor color)
        {
            this[0, row] = new Rook(color);
            this[7, row] = new Rook(color);
            this[1, row] = new Knight(color);
            this[6, row] = new Knight(color);
            this[2, row] = new Bishop(color);
            this[5, row] = new Bishop(color);
            this[3, row] = new Queen(color);
            this[4, row] = new King(color);
        }

        void SetupPawnRow(int row, ChessPieceColor color)
        {
            for (int i = 0; i < 8; i++)
            {
                this[i, row] = new Pawn(color);
            }
        }

        public List<Move> AllPossibleMoves(ChessPieceColor color)
        {
            List<Move> moves = new List<Move>();
            foreach (ChessPiece piece in ChessPiecesByColor(color))
            {
                moves.AddRange(piece.GetPossibleMoves());
            }

            return moves;
        }

        public void AddPiece(ChessPiece newPiece)
        {
            newPiece.Board = this;
            if (newPiece.Color == ChessPieceColor.Black) _blackPieces.Add(newPiece);
            else _whitePieces.Add(newPiece);
            OnPieceAdded?.Invoke(newPiece);
        }

        public void RemovePiece(ChessPiece pieceToRemove)
        {
            pieceToRemove.Board = null;
            if (pieceToRemove.Color == ChessPieceColor.Black) _blackPieces.Remove(pieceToRemove);
            else _whitePieces.Remove(pieceToRemove);
            OnPieceRemoved?.Invoke(pieceToRemove);
        }

        public ChessPiece GetPieceAt(Vector2Int position)
        {
            return ChessPieces.FirstOrDefault(p => p.Position == position);
        }

        public bool HasMoved(ChessPiece piece)
        {
            return MoveHistory.Any(m => m.ChessPiece == piece);
        }

        public int EvaluateScore(ChessPieceColor color)
        {
            int score = 0;
            foreach (ChessPiece piece in ChessPiecesByColor(color))
            {
                score += piece.GetScore();
            }

            return score;
        }
    }
}