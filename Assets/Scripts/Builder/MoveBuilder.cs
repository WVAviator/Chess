using UnityEngine;

namespace Chess
{
    public class MoveBuilder : Builder
    {
        ChessPiece _piece;
        Vector2Int _newPosition;
        Move _move;
        
        public MoveBuilder(ChessBoard board) : base(board)
        {
        }
        
        Move GetMove() => _move ??= new Move(_piece, _newPosition);

        public MoveBuilder From(int x, int y)
        {
            _piece = _board[x, y];
            return this;
        }
        
        public MoveBuilder To(int x, int y)
        {
            _newPosition = new Vector2Int(x, y);
            GetMove();
            return this;
        }

        public MoveBuilder Piece(ChessPiece piece)
        {
            _piece = piece;
            return this;
        }

        public MoveBuilder AndGet(out Move move)
        {
            move = GetMove();
            return this;
        }
        
        public MoveBuilder Execute()
        {
            GetMove().Execute();
            return this;
        }
        
        public MoveBuilder ThenUndo()
        {
            GetMove().Undo();
            return this;
        }
    }
}