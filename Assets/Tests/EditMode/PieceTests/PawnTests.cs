using System.Collections.Generic;
using Chess;
using NUnit.Framework;
using UnityEngine;

namespace Tests.EditMode
{
    public class PawnTests
    {
        class Scenario1
        {
            ChessPiece _pawn;

            public Scenario1()
            {
                BoardBuilder.BuildBoard
                    .Place.White<Pawn>().At(3, 1).AndGet(out _pawn)
                    .Place.Black<Pawn>().At(4, 2);
            }

            [TestCase(2, 2, false)]
            [TestCase(3, 4, false)]
            [TestCase(3, 0, false)]
            [TestCase(3, 1, false)]
            [TestCase(4, 1, false)]
            [TestCase(2, 1, false)]
            [TestCase(4, 2, true)]
            [TestCase(3, 2, true)]
            [TestCase(3, 3, true)]
            public void ReturnsCorrectMoveLegality(int xCheck, int yCheck, bool expected)
            {
                Move move = _pawn.To(xCheck, yCheck);
                
                bool actual = move.IsLegal();
                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void ReturnsCorrectNumberOfPossibleMoves()
            {
                HashSet<Move> moves = _pawn.GetPossibleMoves();
                Assert.IsTrue(moves.Count == 3);
            }
        }
    
        class Scenario2
        {
            ChessPiece _pawn;

            public Scenario2()
            {
                BoardBuilder.BuildBoard
                    .BlackGoesFirst
                    .Place.Black<Pawn>().At(3, 6).AndGet(out _pawn)
                    .Place.White<Pawn>().At(3, 5)
                    .Place.White<Pawn>().At(2, 5);
            }

            [TestCase(3, 4, false)]
            [TestCase(3, 5, false)]
            [TestCase(4, 5, false)]
            [TestCase(4, 6, false)]
            [TestCase(2, 6, false)]
            [TestCase(2, 7, false)]
            [TestCase(2, 5, true)]
            public void ReturnsCorrectMoveLegality(int xCheck, int yCheck, bool expected)
            {
                Move move = _pawn.To(xCheck, yCheck);
                
                bool actual = move.IsLegal();
                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void ReturnsCorrectNumberOfPossibleMoves()
            {
                HashSet<Move> moves = _pawn.GetPossibleMoves();
                Assert.IsTrue(moves.Count == 1);
            }
        }
        class Scenario3
        {
            ChessPiece _pawn;
            
            public Scenario3()
            {
                BoardBuilder.BuildBoard
                    .BlackGoesFirst
                    .Place.Black<Pawn>().At(3, 5).AndGet(out _pawn)
                    .Place.Black<Pawn>().At(2, 4)
                    .Place.White<Pawn>().At(4, 4);
            }

            [TestCase(3, 3, false)]
            [TestCase(2, 4, false)]
            [TestCase(2, 5, false)]
            [TestCase(3, 6, false)]
            [TestCase(4, 4, true)]
            [TestCase(3, 4, true)]
            public void ReturnsCorrectMoveLegality(int xCheck, int yCheck, bool expected)
            {
                Move move = _pawn.To(xCheck, yCheck);
                
                bool actual = move.IsLegal();
                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void ReturnsCorrectNumberOfPossibleMoves()
            {
                HashSet<Move> moves = _pawn.GetPossibleMoves();
                Assert.IsTrue(moves.Count == 2);
            }
        }
        class Scenario4
        {
            ChessPiece pawn;

            public Scenario4()
            {
                BoardBuilder.BuildBoard
                    .Place.White<Pawn>().At(6, 1).AndGet(out pawn)
                    .Place.Black<Knight>().At(5, 2)
                    .Place.White<King>().At(3, 1);
            }

            [TestCase(5, 2, true)]
            [TestCase(6, 3, false)]
            [TestCase(6, 2, false)]
            [TestCase(7, 1, false)]
            public void ReturnsCorrectMoveLegality(int xCheck, int yCheck, bool expected)
            {
                Move move = pawn.To(xCheck, yCheck);
                
                bool actual = move.IsLegal();
                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void ReturnsCorrectNumberOfPossibleMoves()
            {
                HashSet<Move> moves = pawn.GetPossibleMoves();
                Assert.IsTrue(moves.Count == 1);
            }
        }

        class EnPassant
        {
            [Test]
            public void WhiteCanTakeBlackEnPassant()
            {
                BoardBuilder.BuildBoard
                    .Place.White<Pawn>().At(4, 4)
                    .Place.Black<Pawn>().At(3, 6)
                    .BlackGoesFirst
                    .Move.From(3, 6).To(3, 4).Execute()
                    .Move.From(4, 4).To(3, 5).AndGet(out Move move);
                
                Assert.IsTrue(move.IsLegal());
            }

            [Test]
            public void BlackTakesWhiteEnPassant()
            {
                BoardBuilder.BuildBoard
                    .Place.White<Pawn>().At(4, 1).AndGet(out var pawn)
                    .Place.Black<Pawn>().At(3, 3)
                    .Move.From(4, 1).To(4, 3).Execute()
                    .Move.From(3, 3).To(4, 2).Execute()
                    .Get(out var board);
                
                Assert.IsFalse(board.ChessPieces.Contains(pawn));

            }
        }

        class Promotion
        {
            [Test]
            public void WhitePawnNoLongerExistsAtLastSquare()
            {
                BoardBuilder.BuildBoard
                    .Place.White<Pawn>().At(0, 6).AndGet(out var pawn)
                    .Move.From(0, 6).To(0, 7).Execute()
                    .Get(out var board);

                Assert.IsFalse(board.Contains(pawn));
            }

            [Test]
            public void BlackPawnPromotionCanBeUndone()
            {
                BoardBuilder.BuildBoard
                    .BlackGoesFirst
                    .Place.Black<Pawn>().At(0, 1).AndGet(out var pawn)
                    .Move.From(0, 1).To(0, 0).Execute()
                    .ThenUndo()
                    .Get(out var board);
                
                Assert.IsTrue(board[0, 1] == pawn);
            }

            [Test]
            public void WhitePawnPromotesToRequestedQueen()
            {
                BoardBuilder.BuildBoard
                    .Place.White<Pawn>().At(0, 6).AndGet(out var pawn)
                    .Move.From(0, 6).To(0, 7).AndGet(out var move)
                    .Get(out var board);

                move.PromotionPiece = new Queen(pawn.Color);
                move.Execute();
                
                Assert.IsTrue(board[0, 7] is Queen);
            }

            [Test]
            public void BlackPawnPromotionUndoAlsoRemovesNewQueen()
            {
                BoardBuilder.BuildBoard
                    .BlackGoesFirst
                    .Place.Black<Pawn>().At(0, 1).AndGet(out var pawn)
                    .Move.From(0, 1).To(0, 0).Execute()
                    .ThenUndo()
                    .Get(out var board);
                
                Assert.IsTrue(board[0, 1] == pawn);
                Assert.IsTrue(board[0, 0] == null);
            }
        }
    }
}