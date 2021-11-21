namespace Chess.Engine
{
    public class MoveExecutor
    {
        ChessBoard _board;
        
        public MoveExecutor(ChessBoard board)
        {
            _board = board;
        }
        public void ExecuteMove(Move move)
        {
            _board[move.From.xy] = 0;
            _board[move.To.xy] = move.Piece;
        }

        public void UndoMove(Move move)
        {
            _board[move.To.xy] = move.CapturedPiece;
            _board[move.From.xy] = move.Piece;
        }
        
    }
}