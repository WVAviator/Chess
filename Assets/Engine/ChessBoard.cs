namespace Chess.Engine
{
    public class ChessBoard
    {
        public byte[] Board { get;}
        public byte Turn { get;}
        public byte CastleRights { get; }
        public BoardPosition? EnPassant { get; }
        public short HalfMoveClock { get; }
        public short FullMoveNumber { get; }
        
        public ChessBoard()
        {
            Board = new byte[]
            {
                20, 18, 19, 21, 22, 19, 18, 20,
                17, 17, 17, 17, 17, 17, 17, 17,
                 0,  0,  0,  0,  0,  0,  0,  0,
                 0,  0,  0,  0,  0,  0,  0,  0,
                 0,  0,  0,  0,  0,  0,  0,  0,
                 0,  0,  0,  0,  0,  0,  0,  0,
                 9,  9,  9,  9,  9,  9,  9,  9,
                12, 10, 11, 13, 14, 11, 10, 12
            };
            Turn = 8;
            CastleRights = 15;
            EnPassant = null;
            HalfMoveClock = 0;
            FullMoveNumber = 1;
        }
        
        public byte this[int index]
        {
            get => Board[index];
            set => Board[index] = value;
        }
        
    }
}