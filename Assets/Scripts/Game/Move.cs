using System;
using System.Linq;
using UnityEngine;

namespace Chess
{
    public class Move
    {
        public Vector2Int NewPosition { get; }
        public ChessPiece ChessPiece { get; }

        Vector2Int _oldPosition;

        bool _isLegal;

        Rook _castleRook;
        bool _isCastle;

        int _yDirection;
        int _xDirection;

        public static event Action OnMoveExecutedOrUndone;


        readonly ChessBoard _board;

        ChessPiece _targetOpponent;
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
        bool _disabledCastling;
        readonly Vector2Int? _formerEnPassantOpening;

        public Move(ChessPiece chessPiece, Vector2Int newPosition, bool preverified = false)
        {
            NewPosition = newPosition;

            ChessPiece = chessPiece;
            _isLegal = preverified || ChessPiece.IsLegalMove(NewPosition);

            if (!_isLegal) return;
            
            _board = ChessPiece.Board;
            _formerEnPassantOpening = _board.EnPassant;
            
            _oldPosition = chessPiece.Position;
            
            _yDirection = Math.Sign(NewPosition.y - _oldPosition.y);
            _xDirection = Math.Sign(NewPosition.x - _oldPosition.x);

            PromotionPiece = new Queen(ChessPiece.Color);
            CheckCastle();
            CheckPromotion();
            
            _targetOpponent = _board?[NewPosition.x, NewPosition.y];

            CheckEnPassant();
        }

        public int Score => _targetOpponent?.GetScore() ?? 0;


        public bool IsLegal()
        {
            return _isLegal;
        }
        public void Execute(bool quiet = false)
        {
            if (!IsLegal())
            {
                Debug.LogError($"Attempted to execute illegal move: {ChessPiece.GetType().Name} at {ChessPiece.Position} cannot move to {NewPosition}");
                return;
            }
            ChessPiece.MoveTo(NewPosition);
            if (_isCastle) _castleRook.MoveTo(NewPosition + new Vector2Int(-_xDirection, 0));
            
            SetEnPassantOpening();
            SetCastling();

            if (_targetOpponent != null) _board.RemovePiece(_targetOpponent);
            
            
            
            if (IsPromotion)
            {
                _board.RemovePiece(ChessPiece);
                _board[NewPosition.x, NewPosition.y] = PromotionPiece;
            }
            
            if (!quiet) _board.AddToMoveHistory(this);
            if (quiet) _board.PlayerTurn = _board.PlayerTurn.Opponent();

            _isExecuted = true;
            
            OnMoveExecutedOrUndone?.Invoke();
        }

        void SetEnPassantOpening()
        {
            _board.EnPassant = null;
            if (ChessPiece is Pawn && Math.Abs(NewPosition.y - _oldPosition.y) == 2)
            {
                Vector2Int enPassantSquare = new Vector2Int(_oldPosition.x, _oldPosition.y + _yDirection);
                _board.EnPassant = enPassantSquare;
            }
        }

        void SetCastling()
        {
            if (ChessPiece is Rook || ChessPiece is King)
            {
                _disabledCastling = _board.TryDisableCastling(_oldPosition);
            }
        }

        void UndoSetCastling()
        {
            if (_disabledCastling) _board.UndoDisableCastling(_oldPosition);
        }

        public void Undo(bool quiet = false)
        {
            if (!_isExecuted) return;
            _isExecuted = false;
            
            ChessPiece.MoveTo(_oldPosition);
            
            if (_isCastle) _castleRook.MoveTo(_castleRookStartPosition);
            if (_targetOpponent != null) _board[NewPosition.x, NewPosition.y] = _targetOpponent;
            
            
            UndoSetCastling();

            _board.EnPassant = _formerEnPassantOpening;


            if (IsPromotion)
            {
                _board.RemovePiece(PromotionPiece);
                _board[_oldPosition.x, _oldPosition.y] = ChessPiece;
            }
            
            if (!quiet) _board.RemoveFromMoveHistory();
            if (quiet) _board.PlayerTurn = _board.PlayerTurn.Opponent();
            
            OnMoveExecutedOrUndone?.Invoke();
        }

        void CheckCastle()
        {
            if (!(ChessPiece is King king)) return;
            if (Math.Abs(NewPosition.x - _oldPosition.x) != 2) return;
            if (!_board.CanCastle(king, _xDirection)) return;
            
            _isCastle = true;
            Vector2Int rookPosition = new Vector2Int(_xDirection == 1 ? 7 : 0, _oldPosition.y);
            _castleRook = (Rook)_board[rookPosition.x, rookPosition.y];
            _castleRookStartPosition = rookPosition;
        }

        void CheckPromotion()
        {
            if (!(ChessPiece is Pawn pawn)) return;
            if (NewPosition.y != 0 && NewPosition.y != 7) return;
            IsPromotion = true;
        }

        void CheckEnPassant()
        {
            if (!(ChessPiece is Pawn)) return;
            if (_board.EnPassant != NewPosition) return;
            _targetOpponent = _board[NewPosition.x, _oldPosition.y];
        }

        public override string ToString() => ChessPiece.Color + " " + ChessPiece.GetType().Name + " to " + NewPosition;
    }
}