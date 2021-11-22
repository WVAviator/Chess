using System;
using System.Linq;
using UnityEngine;

namespace Chess
{
    public class Move
    {
        public Vector2Int NewPosition { get; }
        public ChessPiece ChessPiece { get; }

        public Vector2Int OldPosition { get; }

        bool _isLegal;

        Rook _castleRook;
        bool _isCastle;

        int _yDirection;
        int _xDirection;

        public static event Action OnMoveExecutedOrUndone;
        public static event Action<Move> OnMoveExecutedOrUndoneWithMove;


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
            
            OldPosition = chessPiece.Position;
            
            _yDirection = Math.Sign(NewPosition.y - OldPosition.y);
            _xDirection = Math.Sign(NewPosition.x - OldPosition.x);

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
            OnMoveExecutedOrUndoneWithMove?.Invoke(this);
        }

        void SetEnPassantOpening()
        {
            _board.EnPassant = null;
            if (ChessPiece is Pawn && Math.Abs(NewPosition.y - OldPosition.y) == 2)
            {
                Vector2Int enPassantSquare = new Vector2Int(OldPosition.x, OldPosition.y + _yDirection);
                _board.EnPassant = enPassantSquare;
            }
        }

        void SetCastling()
        {
            if (ChessPiece is Rook || ChessPiece is King)
            {
                _disabledCastling = _board.TryDisableCastling(OldPosition);
            }
        }

        void UndoSetCastling()
        {
            if (_disabledCastling) _board.UndoDisableCastling(OldPosition);
        }

        public void Undo(bool quiet = false)
        {
            if (!_isExecuted) return;
            _isExecuted = false;
            
            ChessPiece.MoveTo(OldPosition);
            
            if (_isCastle) _castleRook.MoveTo(_castleRookStartPosition);
            if (_targetOpponent != null) _board[NewPosition.x, NewPosition.y] = _targetOpponent;
            
            
            UndoSetCastling();

            _board.EnPassant = _formerEnPassantOpening;


            if (IsPromotion)
            {
                _board.RemovePiece(PromotionPiece);
                _board[OldPosition.x, OldPosition.y] = ChessPiece;
            }
            
            if (!quiet) _board.RemoveFromMoveHistory();
            if (quiet) _board.PlayerTurn = _board.PlayerTurn.Opponent();
            
            OnMoveExecutedOrUndone?.Invoke();
            OnMoveExecutedOrUndoneWithMove?.Invoke(this);
        }

        void CheckCastle()
        {
            if (!(ChessPiece is King king)) return;
            if (Math.Abs(NewPosition.x - OldPosition.x) != 2) return;
            if (!_board.CanCastle(king, _xDirection)) return;
            
            _isCastle = true;
            Vector2Int rookPosition = new Vector2Int(_xDirection == 1 ? 7 : 0, OldPosition.y);
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
            _targetOpponent = _board[NewPosition.x, OldPosition.y];
        }

        public override string ToString() => ChessPiece.Color + " " + ChessPiece.GetType().Name + " to " + NewPosition;
    }
}