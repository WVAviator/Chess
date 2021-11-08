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

        HashSet<ChessPiece> _blackPieces;
        HashSet<ChessPiece> _whitePieces;

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
            _blackPieces = new HashSet<ChessPiece>();
            _whitePieces = new HashSet<ChessPiece>();
            _moveHistory = new Stack<Move>();

            PlayerTurn = ChessPieceColor.White;
            
            foreach (ChessPiece p in pieces) AddPiece(p);
        }
        
        public void GameStart() => OnNewPlayerTurn?.Invoke(PlayerTurn);

        public bool Contains(ChessPiece piece)
        {
            return ChessPieces.Contains(piece);
        }

        public HashSet<ChessPiece> ChessPiecesByColor(ChessPieceColor color)
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

        public HashSet<Move> AllPossibleMoves(ChessPieceColor color)
        {
            HashSet<Move> moves = new HashSet<Move>();
            foreach (ChessPiece piece in ChessPiecesByColor(color))
            {
                moves.UnionWith(piece.GetPossibleMoves());
            }

            //moves.OrderByDescending(m => m.Score);

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
            foreach (ChessPiece piece in ChessPieces)
            {
                if (piece.Position == position) return piece;
            }
            return null;
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

        public string ConvertToBoardString()
        {
            string boardString = "";
            for (int y = 7; y >= 0; y--)
            {
                for (int x = 0; x < 8; x++)
                {
                    boardString += this[x, y]?.PieceChar.ToString() ?? "-";
                }
            }

            return boardString;
        }
    }
}