using UnityEngine;

namespace Chess
{
    public class Setup : Builder
    {
        public Setup(ChessBoard board) : base(board)
        {
        }

        public static Setup Board => new Setup(new ChessBoard());


        public Setup WithRandomAI(ChessPieceColor color)
        {
            RandomAI ai = new RandomAI(color, _board);
            return this;
        }

        public Setup WithOffensiveAI(ChessPieceColor color)
        {
            OffensiveAI ai = new OffensiveAI(color, _board);
            return this;
        }

        public Setup WithMinimaxAI(ChessPieceColor color, int depth)
        {
            MinimaxAI ai = new MinimaxAI(color, _board, depth);
            return this;
        }

        public Setup WithString(string textBoard)
        {
            textBoard = textBoard.Replace("\n", "").Replace("\r", "");
            
            if (textBoard.Length != 64) Debug.LogError($"Invalid board string - requires 64, has {textBoard.Length}");
            for (int i = 0; i < textBoard.Length; i++)
            {
                char c = textBoard[i];
                if (c == '-') continue;
                
                int x = i % 8;
                int y = 7 - i / 8;
                
                ChessPiece piece = ChessPiece.FromChar(c);
                _board[x, y] = piece;
            }

            return this;
        }
        
        public Setup Standard()
        {
            AddPawns();
            AddRoyal();

            return this;
        }
        void AddPawns()
        {
            for (int i = 0; i < 8; i++)
            {
                _board[i, 1] = new Pawn(ChessPieceColor.White);
                _board[i, 6] = new Pawn(ChessPieceColor.Black);
            }
        }
        
        void AddRoyal()
        {
            SetupRoyalRow(0, ChessPieceColor.White);
            SetupRoyalRow(7, ChessPieceColor.Black);
        }
        
        void SetupRoyalRow(int row, ChessPieceColor color)
        {
            _board[0, row] = new Rook(color);
            _board[7, row] = new Rook(color);
            _board[1, row] = new Knight(color);
            _board[6, row] = new Knight(color);
            _board[2, row] = new Bishop(color);
            _board[5, row] = new Bishop(color);
            _board[3, row] = new Queen(color);
            _board[4, row] = new King(color);
        }
        
    }
}