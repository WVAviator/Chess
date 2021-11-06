using System;
using UnityEngine;

namespace Chess
{
    public class PieceBuilder : Builder
    {
        ChessPiece _piece;

        public PieceBuilder(ChessBoard board) : base(board)
        {
        }

        public PieceBuilder At(int x, int y)
        {
            _board[x, y] = _piece;
            return this;
        }

        public PieceBuilder AndGet(out ChessPiece piece)
        {
            piece = _piece;
            return this;
        }

        public PieceBuilder Black<T>() where T : ChessPiece
        {
            _piece = (T) Activator.CreateInstance(typeof(T), ChessPieceColor.Black);
            return this;
        }

        public PieceBuilder White<T>() where T : ChessPiece
        {
            _piece = (T) Activator.CreateInstance(typeof(T), ChessPieceColor.White);
            return this;
        }
    }
}