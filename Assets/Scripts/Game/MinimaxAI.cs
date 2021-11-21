using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Chess
{
    public class MinimaxAI : AI
    {
        readonly int _depth;
        int _minimaxCount;
        bool _useAlphaBetaPruning = true;
        Dictionary<string, int> _previouslyCheckedBoards;
        public MinimaxAI(ChessPieceColor color, ChessBoard board, int depth) : base(color, board)
        {
            _depth = depth;
        }

        protected override void BeginTurn(ChessPieceColor color)
        {
            if (color != _color) return;

            List<Move> possibleMoves = _board.AllPossibleMoves(_color).ToList();
            if (possibleMoves.Count == 0) return;

            possibleMoves.OrderByDescending(m => m.Score);

            Stopwatch minimaxTimer = new Stopwatch();
            minimaxTimer.Start();
            _minimaxCount = 0;

            _previouslyCheckedBoards = new Dictionary<string, int>();
            
            List<Move> bestMoves = new List<Move>();
            int highestScore = int.MinValue;

            foreach (Move move in possibleMoves)
            {
                move.Execute(true);

                int score = Minimax(move.Score ,_depth, false, highestScore, int.MaxValue);
                
                move.Undo(true);

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
            }
            
            minimaxTimer.Stop();
            Debug.Log($"Minimax with a depth of {_depth} took {minimaxTimer.ElapsedMilliseconds}ms to evaluate {_minimaxCount} leaf nodes.");
            
            int randomIndex = Random.Range(0, bestMoves.Count);

            Debug.Log($"Selected move: {bestMoves[randomIndex]}");
            
            ExecuteMove(bestMoves[randomIndex]);
        }

        int Minimax(int moveScore, int depth, bool max, int alpha, int beta)
        {
            LogWithDepth($"||| MoveScore: {moveScore}, AI Turn: {max}, Alpha: {alpha}, Beta: {beta} |||", depth);

            string boardString = _board.ConvertToBoardString();
            
            if (_previouslyCheckedBoards.ContainsKey(boardString))
            {
                LogWithDepth($"~~ Returning cached value: {_previouslyCheckedBoards[boardString]} ~~", depth);
                return _previouslyCheckedBoards[boardString];
            }

            if (depth == 0)
            {
                _minimaxCount++;
                LogWithDepth($"Reached leaf node. Returning {moveScore}.", depth);
                _previouslyCheckedBoards.Add(boardString, moveScore);
                return moveScore;
            }
            
            if (max)
            {
                int highestScore = int.MinValue;

                HashSet<Move> possibleMoves = _board.AllPossibleMoves(_color);
                if (possibleMoves.Count == 0) return -1000;
                
                foreach (Move move in possibleMoves)
                {
                    LogWithDepth($"Checking {move}", depth);
                    
                    move.Execute(true);
                    int score = Minimax(moveScore + move.Score,depth - 1, false, alpha, beta);
                    move.Undo(true);
                    
                    LogWithDepth($"{move} scored {score}", depth);

                    highestScore = Mathf.Max(highestScore, score);
                    alpha = Mathf.Max(highestScore, alpha);

                    if (beta <= alpha && _useAlphaBetaPruning)
                    {
                        LogWithDepth($"This branch beta ({beta}) is less than or equal to alpha ({alpha}). Pruning branch.", depth);

                        break;
                    }
                }
                _previouslyCheckedBoards.Add(boardString, highestScore);
                return highestScore;
            }
            else
            {
                int lowestScore = int.MaxValue;
                
                HashSet<Move> possibleMoves = _board.AllPossibleMoves(_color.Opponent());
                if (possibleMoves.Count == 0) return 1000;
                
                foreach (Move move in possibleMoves)
                {
                    LogWithDepth($"Checking {move.ToString()}", depth);
                    
                    move.Execute(true);
                    int score = Minimax(moveScore - move.Score,depth - 1, true, alpha, beta);
                    move.Undo(true);
                    
                    LogWithDepth($"{move.ToString()} scored {score}", depth);
                    
                    lowestScore = Mathf.Min(lowestScore, score);
                    beta = Mathf.Min(lowestScore, beta);

                    if (beta <= alpha && _useAlphaBetaPruning)
                    {
                        LogWithDepth($"This branch beta ({beta}) is less than or equal to alpha ({alpha}). Pruning branch.", depth);
                        break;
                    }
                }
                _previouslyCheckedBoards.Add(boardString, lowestScore);
                return lowestScore;
            }
        }

        void LogWithDepth(string s, int depth)
        {
            string d = "";
            for (int i = _depth; i > depth; i--)
            {
                d += "    ";
            }
            Debug.Log(d + s);
        }
    }
}