namespace Chess
{
    public abstract class Builder
    {
        protected ChessBoard _board;
        public Builder(ChessBoard board)
        {
            _board = board; 
        }

        public MoveBuilder Move => new MoveBuilder(_board);
        public PieceBuilder Place => new PieceBuilder(_board);
        public Builder Get(out ChessBoard board)
        {
            board = _board;
            return this;
        }
        public Builder BlackGoesFirst
        {
            get
            {
                _board.PlayerTurn = ChessPieceColor.Black;
                return this;
            }
        }
        
        public Builder StartGame()
        {
            _board.GameStart();
            return this;
        }

    }
}