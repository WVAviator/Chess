using System.Collections.Generic;
using Chess;
using NUnit.Framework;
using UnityEngine;

namespace Tests.EditMode
{
    public class KnightTests
    {

        class Scenario1
        {
            ChessPiece knight;

            public Scenario1()
            {
                 Setup.Board
                    .Place.White<Knight>().At(1, 5).AndGet(out knight)
                    .Place.White<Pawn>().At(3, 6)
                    .Place.Black<Pawn>().At(3, 4);
            }

            [TestCase(0, 7, true)]
            [TestCase(0, 3, true)]
            [TestCase(2, 3, true)]
            [TestCase(3, 4, true)]
            [TestCase(2, 7, true)]
            [TestCase(-1, 4, false)]
            [TestCase(0, 6, false)]
            [TestCase(1, 6, false)]
            [TestCase(3, 6, false)]
            [TestCase(3, 5, false)]
            [TestCase(3, 3, false)]
            public void ReturnsCorrectMoveLegality(int xCheck, int yCheck, bool expected)
            {
                Move move = knight.To(xCheck, yCheck);
                
                bool actual = move.IsLegal();
                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void ReturnsCorrectNumberOfPossibleMoves()
            {
                HashSet<Move> moves = knight.GetPossibleMoves();
                Assert.IsTrue(moves.Count == 5);
            }
        }

        class Scenario2
        {
            ChessPiece knight;

            public Scenario2()
            {
                Setup.Board
                    .BlackGoesFirst
                    .Place.Black<Knight>().At(3, 2).AndGet(out knight)
                    .Place.White<Pawn>().At(4, 2)
                    .Place.White<Pawn>().At(2, 4)
                    .Place.Black<Pawn>().At(4, 3)
                    .Place.Black<Pawn>().At(4, 4);
            }

            [TestCase(2, 4, true)]
            [TestCase(1, 3, true)]
            [TestCase(1, 1, true)]
            [TestCase(2, 0, true)]
            [TestCase(4, 0, true)]
            [TestCase(5, 1, true)]
            [TestCase(5, 3, true)]
            [TestCase(4, 4, false)]
            [TestCase(4, 3, false)]
            [TestCase(4, 2, false)]
            [TestCase(3, 2, false)]
            [TestCase(3, 1, false)]
            [TestCase(1, 4, false)]
            public void ReturnsCorrectMoveLegality(int xCheck, int yCheck, bool expected)
            {
                Move move = knight.To(xCheck, yCheck);
                
                bool actual = move.IsLegal();
                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void ReturnsCorrectNumberOfPossibleMoves()
            {
                HashSet<Move> moves = knight.GetPossibleMoves();
                Assert.IsTrue(moves.Count == 7);
            }
        }
    }
}