using UnityEditor.UIElements;
using UnityEngine;

namespace Chess
{
    public class PieceFactory
    {
        ChessPieceColor _color;
        public PieceFactory(ChessPieceColor color)
        {
            _color = color;
        }

        public Knight Knight => new Knight(_color);

        public Bishop Bishop => new Bishop(_color);
        
        public Rook Rook => new Rook(_color);
        
        public Queen Queen => new Queen(_color);
        
        public King King => new King(_color);
        
        public Pawn Pawn => new Pawn(_color);

    }
}