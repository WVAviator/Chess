using System.Collections.Generic;
using Chess;
using NUnit.Framework;
using UnityEngine;

namespace Tests.EditMode
{
    public class QueenTests
    {
        class Scenario1
        {
            ChessPiece queen;

            public Scenario1()
            {
                BoardBuilder.BuildBoard
                    .Place.White<Queen>().At(4, 1).AndGet(out queen)
                    .Place.White<Pawn>().At(2, 1)
                    .Place.Black<Pawn>().At(1, 4)
                    .Place.Black<Pawn>().At(4, 6)
                    .Place.White<Rook>().At(4, 0);
            }

            [TestCase(4, 0, false)]
            [TestCase(2, 1, false)]
            [TestCase(8, 1, false)]
            [TestCase(4, 1, false)]
            [TestCase(4, 7, false)]
            [TestCase(0, 5, false)]
            [TestCase(5, 3, false)]
            [TestCase(4, 6, true)]
            [TestCase(1, 4, true)]
            [TestCase(3, 1, true)]
            [TestCase(7, 1, true)]
            [TestCase(7, 4, true)]
            [TestCase(5, 0, true)]
            public void ReturnsCorrectMoveLegality(int xCheck, int yCheck, bool expected)
            {
                Move move = queen.To(xCheck, yCheck);
                
                bool actual = move.IsLegal();
                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void ReturnsCorrectNumberOfPossibleMoves()
            {
                HashSet<Move> moves = queen.GetPossibleMoves();
                Assert.IsTrue(moves.Count == 17);
            }
        }
    
        class Scenario2
        {
            ChessPiece queen;

            public Scenario2()
            {
                BoardBuilder.BuildBoard
                    .BlackGoesFirst
                    .Place.Black<Queen>().At(0, 0).AndGet(out queen)
                    .Place.White<Pawn>().At(0, 5)
                    .Place.White<Rook>().At(7, 7);
            }

            [TestCase(0, 6, false)]
            [TestCase(0, 7, false)]
            [TestCase(-1, -1, false)]
            [TestCase(0, 0, false)]
            [TestCase(1, 2, false)]
            [TestCase(2, 1, false)]
            [TestCase(6, 3, false)]
            [TestCase(0, 5, true)]
            [TestCase(7, 7, true)]
            [TestCase(6, 6, true)]
            [TestCase(7, 0, true)]
            [TestCase(3, 0, true)]
            [TestCase(0, 4, true)]
            public void ReturnsCorrectMoveLegality(int xCheck, int yCheck, bool expected)
            {
                Move move = queen.To(xCheck, yCheck);
                
                bool actual = move.IsLegal();
                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void ReturnsCorrectNumberOfPossibleMoves()
            {
                HashSet<Move> moves = queen.GetPossibleMoves();
                Assert.IsTrue(moves.Count == 19);
            }
        }
    }
}