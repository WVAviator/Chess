using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chess
{
    public class Queen : GlidingPiece
    {
        
        public override char PieceChar
        {
            get
            {
                return Color == ChessPieceColor.Black ? 'q' : 'Q';
            }
        }
        public override string PieceName => "Queen";
        public Queen(ChessPieceColor color, Vector2Int position = default) : base(color, position)
        {
        }
        
        public Queen(ChessPieceColor color) : base(color, default)
        {
        }

        public override int GetScore() => 9;

        public override bool IsLegalMove(Move move)
        {
            if (move.NewPosition == Position) return false;
            if (!Diagonal(move) && !VerticalOrHorizontal(move)) return false;
            if (Blocked(move)) return false;
            if (AllyInPosition(move.NewPosition)) return false;
            
            return true;
        }

        protected override HashSet<Move> GetPotentialMoves()
        {
            HashSet<Move> moves = new HashSet<Move>();
            moves.UnionWith(GetPotentialDiagonalMoves());
            moves.UnionWith(GetPotentialVerticalHorizontalMoves());
            return moves;
        }
    }
}