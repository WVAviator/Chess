namespace Chess
{
    public abstract class AI
    {
        protected ChessPieceColor _color;
        protected ChessBoard _board;

        public AI(ChessPieceColor color, ChessBoard board)
        {
            _color = color;
            _board = board;
            
            _board.OnNewPlayerTurn += BeginTurn;
        }

        protected abstract void BeginTurn(ChessPieceColor color);
        protected void ExecuteMove(Move move)
        {
            move.Execute();
        }
    }
}