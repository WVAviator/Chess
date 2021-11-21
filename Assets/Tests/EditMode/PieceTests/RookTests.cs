using System.Collections.Generic;
using Chess;
using NUnit.Framework;
using UnityEngine;

namespace Tests.EditMode
{
    public class RookTests
    {
        class Scenario1
        {
            ChessPiece rook;

            public Scenario1()
            {
                BoardBuilder.BuildBoard
                    .Place.White<Rook>().At(1, 2).AndGet(out rook)
                    .Place.White<Pawn>().At(1, 6)
                    .Place.Black<Pawn>().At(3, 2);
            }

            [TestCase(1, 6, false)]
            [TestCase(1, 7, false)]
            [TestCase(2, 3, false)]
            [TestCase(-1, 2, false)]
            [TestCase(1, 2, false)]
            [TestCase(4, 2, false)]
            [TestCase(7, 2, false)]
            [TestCase(3, 2, true)]
            [TestCase(0, 2, true)]
            [TestCase(1, 5, true)]
            [TestCase(1, 0, true)]
            [TestCase(1, 1, true)]
            [TestCase(2, 2, true)]
            public void ReturnsCorrectMoveLegality(int xCheck, int yCheck, bool expected)
            {
                Move move = rook.To(xCheck, yCheck);
                
                bool actual = move.IsLegal();
                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void ReturnsCorrectNumberOfPossibleMoves()
            {
                HashSet<Move> moves = rook.GetPossibleMoves();
                Assert.IsTrue(moves.Count == 8);
            }
        }
    
        class Scenario2
        {
            ChessPiece rook;

            public Scenario2()
            {
                BoardBuilder.BuildBoard
                    .BlackGoesFirst
                    .Place.Black<Rook>().At(5, 6).AndGet(out rook)
                    .Place.White<Pawn>().At(2, 6)
                    .Place.White<Pawn>().At(3, 4)
                    .Place.Black<Pawn>().At(5, 1);
            }

            [TestCase(1, 6, false)]
            [TestCase(0, 6, false)]
            [TestCase(3, 4, false)]
            [TestCase(8, 6, false)]
            [TestCase(5, 6, false)]
            [TestCase(5, 0, false)]
            [TestCase(5, 1, false)]
            [TestCase(2, 6, true)]
            [TestCase(5, 2, true)]
            [TestCase(7, 6, true)]
            [TestCase(5, 7, true)]
            [TestCase(6, 6, true)]
            [TestCase(5, 4, true)]
            public void ReturnsCorrectMoveLegality(int xCheck, int yCheck, bool expected)
            {
                Move move = rook.To(xCheck, yCheck);
                
                bool actual = move.IsLegal();
                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void ReturnsCorrectNumberOfPossibleMoves()
            {
                HashSet<Move> moves = rook.GetPossibleMoves();
                Assert.IsTrue(moves.Count == 10);
            }
        }
    }
}