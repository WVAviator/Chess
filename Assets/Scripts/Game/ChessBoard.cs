using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chess
{
    public class ChessBoard
    {
        public List<ChessPiece> ChessPieces => _whitePieces.Concat(_blackPieces).ToList();

        public ChessPiece[] ChessPieceArray = new ChessPiece[64];

        public ChessPieceColor PlayerTurn;

        HashSet<ChessPiece> _blackPieces;
        HashSet<ChessPiece> _whitePieces;

        King _whiteKing;
        King _blackKing;

        int _castleRights = 15;

        public bool CanCastle(King king, int direction)
        {
            
            if (direction == -1 && king.Color == ChessPieceColor.White) return (_castleRights & 2) > 0;
            if (direction == 1 && king.Color == ChessPieceColor.White) return (_castleRights & 1) > 0;
            if (direction == -1 && king.Color == ChessPieceColor.Black) return (_castleRights & 8) > 0;
            if (direction == 1 && king.Color == ChessPieceColor.Black) return (_castleRights & 4) > 0;
            return false;
        }

        public Stack<Move> MoveHistory { get; }

        public event Action<ChessPiece> OnPieceAdded;
        public event Action<ChessPiece> OnPieceRemoved;
        public event Action<ChessPieceColor> OnNewPlayerTurn;
        public event Action OnBoardUpdated;

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
            MoveHistory = new Stack<Move>();

            PlayerTurn = ChessPieceColor.White;
            
            foreach (ChessPiece p in pieces) AddPiece(p);
        }
        
        public void StartGame() => OnNewPlayerTurn?.Invoke(PlayerTurn);

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
            King king = GetKing(color);
            if (king == null) return false;
            
            bool isInCheck = false;
            ChessPieceColor currentTurn = PlayerTurn;

            PlayerTurn = color.Opponent();
            foreach (ChessPiece piece in ChessPiecesByColor(color.Opponent()))
            {
                if (piece.IsLegalMove(king.Position)) isInCheck = true;
            }

            PlayerTurn = currentTurn;
            
            return isInCheck;
        }

        public bool IsCheckmate(ChessPieceColor color) => AllPossibleMoves(color).Count == 0;

        public void AddToMoveHistory(Move move)
        {
            MoveHistory.Push(move);
            NextPlayerTurn();
            if (IsCheckmate(PlayerTurn)) Debug.Log($"Checkmate! {PlayerTurn.Opponent()} wins!");
        }

        public void RemoveFromMoveHistory()
        {
            NextPlayerTurn();
            MoveHistory.Pop();
        }
        
        public Move MostRecentMove() => MoveHistory.Peek();

        public BoardBuilder Setup() => new BoardBuilder(this);

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

        void AddPiece(ChessPiece newPiece)
        {
            newPiece.Board = this;
            if (newPiece.Color == ChessPieceColor.Black) _blackPieces.Add(newPiece);
            else _whitePieces.Add(newPiece);
            
            
            
            OnPieceAdded?.Invoke(newPiece);
            OnBoardUpdated?.Invoke();
            
            if (newPiece is King) 
            {
                if (newPiece.Color == ChessPieceColor.White) _whiteKing = (King)newPiece;
                else _blackKing = (King)newPiece;
            }
        }

        public King GetKing(ChessPieceColor color)
        {
            return color == ChessPieceColor.White ? _whiteKing : _blackKing;
        }

        public void RemovePiece(ChessPiece pieceToRemove)
        {
            pieceToRemove.Board = null;
            if (pieceToRemove.Color == ChessPieceColor.Black) _blackPieces.Remove(pieceToRemove);
            else _whitePieces.Remove(pieceToRemove);
            OnPieceRemoved?.Invoke(pieceToRemove);
            OnBoardUpdated?.Invoke();
        }

        public ChessPiece GetPieceAt(Vector2Int position)
        {
            foreach (ChessPiece piece in ChessPieces)
            {
                if (piece.Position == position) return piece;
            }
            return null;
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

        public void SetCastling(string castle)
        {
            _castleRights = 0;
            _castleRights += castle.Contains('K') ? 1 : 0;
            _castleRights += castle.Contains('Q') ? 2 : 0;
            _castleRights += castle.Contains('k') ? 4 : 0;
            _castleRights += castle.Contains('q') ? 8 : 0;
        }

        public bool TryDisableCastling(Vector2Int piecePosition)
        {
            int originalCastleRights = _castleRights;
            
            if (piecePosition.x == 0)
            {
                switch (piecePosition.y)
                {
                    case 0:
                        _castleRights &= ~2;
                        return _castleRights != originalCastleRights;
                    case 7:
                        _castleRights &= ~1;
                        return _castleRights != originalCastleRights;
                    case 4:
                        _castleRights &= ~3;
                        return _castleRights != originalCastleRights;
                }
            }
            else if (piecePosition.x == 7)
            {
                switch (piecePosition.y)
                {
                    case 0:
                        _castleRights &= ~8;
                        return _castleRights != originalCastleRights;
                    case 7:
                        _castleRights &= ~4;
                        return _castleRights != originalCastleRights;
                    case 4:
                        _castleRights &= ~12;
                        return _castleRights != originalCastleRights;
                }
            }

            return false;
        }

        public void UndoDisableCastling(Vector2Int piecePosition)
        {
            if (piecePosition.x == 0)
            {
                switch (piecePosition.y)
                {
                    case 0:
                        _castleRights |= 2;
                        break;
                    case 7:
                        _castleRights |= 1;
                        break;
                    case 4:
                        _castleRights |= 3;
                        break;
                }
            }
            else if (piecePosition.x == 7)
            {
                switch (piecePosition.y)
                {
                    case 0:
                        _castleRights |= 8;
                        break;
                    case 7:
                        _castleRights |= 4;
                        break;
                    case 4:
                        _castleRights |= 12;
                        break;
                }
            }
        }
        
        public void SetEnPassant(string enPassant)
        {
            if (enPassant == "-")
            {
                EnPassant = null;
                return;
            }
            EnPassant = ANtoXY(enPassant);
        }
        
        public Vector2Int ANtoXY(string algebraicNotation)
        {
            int x = algebraicNotation[0] - 'a';
            int y = algebraicNotation[1] - '1';

            return new Vector2Int(x, y);
        }

        public Vector2Int? EnPassant { get; set; }
        public int HalfMoveClock { get; set; }
        public int FullMoveNumber { get; set; }
    }
}