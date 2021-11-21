using System.Diagnostics;
using System.Linq;
using Chess;
using NUnit.Framework;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Tests.EditMode
{
    public class MinimaxTests
    {
        [Test]
        public void WhiteDoesNotSacrificeQueenToTakeKnight()
        {
            BoardBuilder.BuildBoard
                .WithMinimaxAI(ChessPieceColor.White, 1)
                .Place.White<Queen>().At(1, 1).AndGet(out var queen)
                .Place.Black<Knight>().At(4, 4)
                .Place.Black<Rook>().At(4, 7)
                .BlackGoesFirst
                .Move.From(4, 7).To(4, 6).Execute()
                .Get(out var board);
            
            Assert.IsFalse(board[4, 4] == queen);
        }

        [Test]
        public void AIPicksBestMove()
        {
            BoardBuilder.BuildBoard
                .WithMinimaxAI(ChessPieceColor.Black, 0)
                .Place.Black<Queen>().At(4, 4).AndGet(out var blackQueen)
                .Place.White<Pawn>().At(6, 6)
                .Place.White<Bishop>().At(2, 6)
                .Place.White<Queen>().At(2, 1).AndGet(out var queen)
                .Move.From(2, 1).To(2, 2).Execute()
                .Get(out var board);

            Debug.Log($"Black queen chose: {blackQueen.Position}");
            Assert.IsFalse(board.Contains(queen));
        }

        [Test]
        public void AISkipsOpportunityToTakeOpponentKnightToAvoidCheckmate()
        {
            BoardBuilder.BuildBoard
                .WithMinimaxAI(ChessPieceColor.Black, 2)
                .Place.Black<King>().At(7, 5)
                .Place.White<King>().At(0, 4)
                .Place.White<Rook>().At(6, 0)
                .Place.White<Queen>().At(0, 1)
                .Place.Black<Bishop>().At(5, 2).AndGet(out var bishop)
                .Place.White<Knight>().At(4, 3)
                .BlackGoesFirst.StartGame()
                .Get(out var board);

            Assert.IsFalse(board[4, 3] == bishop);
        }

        [Test]
        public void DepthOfOneTakesNoLongerThanFiveSecondsWithStandardSetup()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            
            BoardBuilder.BuildBoard.StandardSetup()
                .WithMinimaxAI(ChessPieceColor.White, 1)
                .StartGame();
            
            stopwatch.Stop();
            Assert.Less(stopwatch.ElapsedMilliseconds, 5000);
        }

        class PuzzleTests
        {
            [Test]
            public void Test01()
            {
                string textBoard = Resources.Load<TextAsset>("Puzzles/01").text;

                BoardBuilder.BuildBoard.WithString(textBoard)
                    .WithMinimaxAI(ChessPieceColor.White, 3)
                    .StartGame()
                    .Get(out var board);
                
                Queen queen = board.ChessPieces.Find(p => p is Queen && p.Color == ChessPieceColor.White) as Queen;

                Debug.Log($"Queen is at {queen.Position}");
                
                Assert.IsTrue(board[6,7] == queen);
            }
        }
    }
}