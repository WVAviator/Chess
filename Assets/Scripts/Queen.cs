﻿using System;
using UnityEngine;

namespace Chess
{
    public class Queen : GlidingPiece
    {
        public Queen(ChessPieceColor color, Vector2Int position = default) : base(color, position)
        {
        }

        public override bool IsLegalMove(Move move)
        {
            if (move.NewPosition == Position) return false;
            if (!Diagonal(move) && !VerticalOrHorizontal(move)) return false;
            if (Blocked(move)) return false;
            if (AllyInPosition(move.NewPosition)) return false;
            
            return true;
        }
    }
}