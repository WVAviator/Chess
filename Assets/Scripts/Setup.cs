namespace Chess
{
    public class Setup : Builder
    {
        public Setup(ChessBoard board) : base(board)
        {
        }

        public static Setup Board => new Setup(new ChessBoard());
    }
}