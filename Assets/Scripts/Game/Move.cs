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
        
        public ChessBoard Board { get; }
        
        public ChessPiece TargetOpponent { get; set; }
        public bool IsPromotion { get; set; }

        ChessPiece _promotionPiece;
        public ChessPiece PromotionPiece
        {
            get => _promotionPiece;
            set
            {
                if (_isExecuted)
                {
                    Undo();
                    _promotionPiece = value;
                    Execute();
                }
                else _promotionPiece = value;
            }
        }

        bool _isExecuted;
        Vector2Int _castleRookStartPosition;

        public Move(ChessPiece chessPiece, Vector2Int newPosition)
        {
            _newPosition = newPosition;
            ChessPiece = chessPiece;
            Board = ChessPiece.Board;
            _oldPosition = chessPiece.Position;

            PromotionPiece = new Queen(ChessPiece.Color);
            
            if (!IsValidPosition(_newPosition)) _newPosition = chessPiece.Position;

            YDifference = _newPosition.y - _oldPosition.y;
            XDifference = _newPosition.x - _oldPosition.x;
            YDirection = Math.Sign(YDifference);
            XDirection = Math.Sign(XDifference);

            TargetOpponent = Board?[NewPosition.x, NewPosition.y];
            if (TargetOpponent?.Color == ChessPiece.Color) TargetOpponent = null;
        }

        public int Score => TargetOpponent?.GetScore() ?? 0;
        public bool IsLegal()
        {
            return !PutsKingInCheck() && ChessPiece.IsLegalMove(this) && IsMyTurn();
        }

        bool IsMyTurn() => Board.PlayerTurn == ChessPiece.Color;
        

        public void Execute(bool quiet = false)
        {
            if (!IsLegal())
            {
                Debug.LogError($"Attempted to execute illegal move: {ChessPiece.GetType().Name} at {ChessPiece.Position} cannot move to {NewPosition}");
                return;
            }
            ChessPiece.MoveTo(NewPosition);
            if (_isCastle) _castleRook.MoveTo(NewPosition + new Vector2Int(-XDirection, 0));
            

            if (TargetOpponent != null) Board.RemovePiece(TargetOpponent);
            
            if (!quiet) Board.AddToMoveHistory(this);
            
            if (IsPromotion)
            {
                Board.RemovePiece(ChessPiece);
                PromotionPiece.Position = NewPosition;
                Board.AddPiece(PromotionPiece);
            }

            if (quiet) Board.PlayerTurn = Board.PlayerTurn.Opponent();
            
            _isExecuted = true;
        }

        public void Undo(bool quiet = false)
        {
            if (!_isExecuted) return;
            _isExecuted = false;
            
            if (!quiet && Board.MoveHistory.Peek() != this)
            {
                Debug.LogError($"Attempted to undo moves out of order! Move {ChessPiece.GetType().Name} to {NewPosition} is not the most recent move.");
                return;
            }
            ChessPiece.MoveTo(_oldPosition);
            if (_isCastle) _castleRook.MoveTo(_castleRookStartPosition);
            if (TargetOpponent != null) Board.AddPiece(TargetOpponent);
            if (!quiet) Board.RemoveFromMoveHistory();

            if (IsPromotion)
            {
                Board.AddPiece(ChessPiece);
                Board.RemovePiece(PromotionPiece);
            }
            
            if (quiet) Board.PlayerTurn = Board.PlayerTurn.Opponent();
        }
        
        static bool IsValidPosition(Vector2Int potentialPosition)
        {
            bool isValid = potentialPosition.x >= 0 && potentialPosition.y >= 0 
                           && potentialPosition.x <= 7 && potentialPosition.y <= 7;
            return isValid;
        }

        bool PutsKingInCheck()
        {
            King king = FindKing();
            if (king == null) return false;
            
            bool inCheck = false;
            
            ChessPiece.MoveTo(NewPosition);
            if (TargetOpponent != null) Board.RemovePiece(TargetOpponent);
            
            foreach (ChessPiece piece in Board.ChessPiecesByColor(ChessPiece.Color.Opponent()))
            {
                if (piece == TargetOpponent) continue;
                if (piece.IsLegalMove(king.Position)) inCheck = true;
            }
            
            if (TargetOpponent != null) Board.AddPiece(TargetOpponent);
            ChessPiece.MoveTo(_oldPosition);

            return inCheck;
        }

        King FindKing()
        {
            King king = null;
            foreach (ChessPiece piece in Board.ChessPiecesByColor(ChessPiece.Color))
            {
                if (piece is King)
                {
                    king = (King) piece;
                    break;
                }
            }
            return king;
        }

        public void IsCastle(Rook rook)
        {
            _isCastle = true;
            _castleRook = rook;
            _castleRookStartPosition = rook.Position;
        }

        public override string ToString() => ChessPiece.Color + " " + ChessPiece.GetType().Name + " to " + NewPosition;
    }
}