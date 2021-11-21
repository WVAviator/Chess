using System.Collections.Generic;
using Chess;
using NUnit.Framework;
using UnityEngine;

namespace Tests.EditMode
{
    public class BishopTests
    {
        class Scenario1
        {

            ChessPiece bishop;

            public Scenario1()
            {
                BoardBuilder.BuildBoard
                    .Place.White<Bishop>().At(5, 3).AndGet(out bishop)
                    .Place.Black<Pawn>().At(5, 5)
                    .Place.White<Pawn>().At(3, 5)
                    .Place.Black<Pawn>().At(3, 1);
            }
        
            [TestCase(6, 2, true)]
            [TestCase(7, 1, true)]
            [TestCase(4, 2, true)]
            [TestCase(3, 1, true)]
            [TestCase(2, 0, false)]
            [TestCase(4, 4, true)]
            [TestCase(3, 5, false)]
            [TestCase(2, 6, false)]
            [TestCase(6, 4, true)]
            [TestCase(7, 5, true)]
            [TestCase(5, 5, false)]
            [TestCase(5, 4, false)]
            [TestCase(5, 2, false)]
            [TestCase(5, 3, false)]
            [TestCase(6, 3, false)]
            [TestCase(8, 0, false)]
            public void ReturnsCorrectMoveLegality(int xCheck, int yCheck, bool expected)
            {
                Move move = bishop.To(xCheck, yCheck);
                
                bool actual = move.IsLegal();
                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void ReturnsCorrectNumberOfPossibleMoves()
            {
                HashSet<Move> moves = bishop.GetPossibleMoves();
                Assert.IsTrue(moves.Count == 7);
            }
        
        }
        class Scenario2
        {
            ChessPiece bishop;

            public Scenario2()
            {
                BoardBuilder.BuildBoard
                    .Place.White<Bishop>().At(2, 2).AndGet(out bishop)
                    .Place.Black<Pawn>().At(1, 3)
                    .Place.White<Pawn>().At(5, 5);
            }
        
            [TestCase(2, 2, false)]
            [TestCase(1, 1, true)]
            [TestCase(0, 0, true)]
            [TestCase(3, 1, true)]
            [TestCase(4, 0, true)]
            [TestCase(3, 3, true)]
            [TestCase(4, 4, true)]
            [TestCase(5, 5, false)]
            [TestCase(6, 6, false)]
            [TestCase(7, 7, false)]
            [TestCase(-1, -1, false)]
            [TestCase(1, 3, true)]
            [TestCase(0, 4, false)]
            [TestCase(2, 3, false)]
            [TestCase(2, 1, false)]
            [TestCase(3, 2, false)]
            public void ReturnsCorrectMoveLegality(int xCheck, int yCheck, bool expected)
            {
                Move move = bishop.To(xCheck, yCheck);
                
                bool actual = move.IsLegal();
                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void ReturnsCorrectNumberOfPossibleMoves()
            {
                HashSet<Move> moves = bishop.GetPossibleMoves();
                Assert.IsTrue(moves.Count == 7);
            }
        }
    }
}