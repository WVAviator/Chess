namespace Chess
{
    public static class Extensions
    {
        public static ChessPieceColor Opponent(this ChessPieceColor color)
        {
            if (color == ChessPieceColor.Black) return ChessPieceColor.White;
            return ChessPieceColor.Black;
        }
    }
}