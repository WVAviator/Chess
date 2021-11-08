using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chess
{
    public class RandomAI : AI
    {
        public RandomAI(ChessPieceColor color, ChessBoard board) : base(color, board)
        {
        }


        protected override void BeginTurn(ChessPieceColor color)
        {
            if (color != _color) return;
            
            HashSet<Move> possibleMoves = _board.AllPossibleMoves(_color);

            if (possibleMoves.Count == 0) return;
            
            int randomIndex = Random.Range(0, possibleMoves.Count);
            Move move = possibleMoves.ElementAt(randomIndex);
            ExecuteMove(move);
        }
    }
}