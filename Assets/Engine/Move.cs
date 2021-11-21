namespace Chess.Engine
{
    public struct Move
    {
        public BoardPosition From { get;}
        public BoardPosition To { get;}
        public byte Piece { get;}
        public byte CapturedPiece { get;}
        public ChessBoard Board;

        public Move(ChessBoard board, BoardPosition from, BoardPosition to)
        {
            Board = board;
            From = from;
            To = to;
            Piece = board[from.xy];
            CapturedPiece = board[to.xy];
        }
        
        public Move(ChessBoard board, int from, int to)
        {
            Board = board;
            From = new BoardPosition((byte)from);
            To = new BoardPosition((byte)to);
            Piece = board[from];
            CapturedPiece = board[to];
        }

    }
}