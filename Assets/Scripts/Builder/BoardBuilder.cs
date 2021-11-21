using System;
using UnityEngine;

namespace Chess
{
    public class BoardBuilder : Builder
    {
        public BoardBuilder(ChessBoard board) : base(board)
        {
        }

        public static BoardBuilder BuildBoard => new BoardBuilder(new ChessBoard());


        public BoardBuilder WithRandomAI(ChessPieceColor color)
        {
            RandomAI ai = new RandomAI(color, _board);
            return this;
        }

        public BoardBuilder WithOffensiveAI(ChessPieceColor color)
        {
            OffensiveAI ai = new OffensiveAI(color, _board);
            return this;
        }

        public BoardBuilder WithMinimaxAI(ChessPieceColor color, int depth)
        {
            MinimaxAI ai = new MinimaxAI(color, _board, depth);
            return this;
        }
        
        public BoardBuilder SetCastle(string castle)
        {
            _board.SetCastling(castle);
            return this;
        }

        public BoardBuilder WithString(string textBoard)
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

        public BoardBuilder WithFENString(string FENString)
        {
            string[] parts = FENString.Split(' ');
            if (parts.Length != 6) Debug.LogError($"Invalid FEN string - requires 6, has {parts.Length}");
            
            string[] rows = parts[0].Split('/');
            if (rows.Length != 8) Debug.LogError($"Invalid FEN string - requires 8 rows, has {rows.Length}");
            
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    if (int.TryParse(rows[y][x].ToString(), out int i)) x += i - 1;
                    else
                    {
                        ChessPiece piece = ChessPiece.FromChar(rows[y][x]);
                        _board[x, 7 - y] = piece;
                    }
                }
            }
            
            _board.PlayerTurn = parts[1] == "w" ? ChessPieceColor.White : ChessPieceColor.Black;
            _board.SetCastling(parts[2]);
            _board.SetEnPassant(parts[3]);
            _board.HalfMoveClock = int.Parse(parts[4]);
            _board.FullMoveNumber = int.Parse(parts[5]);
            
            return this;
            
        }
        
        public BoardBuilder StandardSetup()
        {
            return WithFENString("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
        }
    }
}