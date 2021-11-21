namespace Chess.Engine
{
    public struct BoardPosition
    {
        public byte x { get; }
        public byte y { get; }
        public byte xy => (byte)(x + (y << 4));
        
        public BoardPosition(byte x, byte y)
        {
            this.x = x;
            this.y = y;
        }

        public BoardPosition(byte xy)
        {
            this.x = (byte)(xy & 7);
            this.y = (byte)(xy >> 3);
        }
    }
}