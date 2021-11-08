using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public class OffensiveAI : AI
    {
        public OffensiveAI(ChessPieceColor color, ChessBoard board) : base(color, board)
        {
        }

        protected override void BeginTurn(ChessPieceColor color)
        {
            if (color != _color) return;

            HashSet<Move> possibleMoves = _board.AllPossibleMoves(_color);
            if (possibleMoves.Count == 0) return;
            
            List<Move> bestMoves = new List<Move>();
            int highestScore = int.MinValue;
            
            foreach (Move move in possibleMoves)
            {
                move.Execute(true);
                
                int score = _board.EvaluateScore(_color) - _board.EvaluateScore(_color.Opponent());
                if (score > highestScore)
                {
                    bestMoves.Clear();
                    bestMoves.Add(move);
                    highestScore = score;
                }
                else if (score == highestScore)
                {
                    bestMoves.Add(move);
                }
                move.Undo(true);
                
                
            }
            int randomIndex = Random.Range(0, bestMoves.Count);
            ExecuteMove(bestMoves[randomIndex]);
        }
    }
}