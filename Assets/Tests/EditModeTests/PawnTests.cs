using System.Collections.Generic;
using System.Linq;
using Chess;
using NUnit.Framework;
using UnityEngine;

public class PawnTests
{
    class Scenario1
    {
        Pawn pawn;
        Pawn pawn2;
        ChessBoard board;

        public Scenario1()
        {
            pawn = new Pawn(ChessPieceColor.White, new Vector2Int(3, 1));
            pawn2 = new Pawn(ChessPieceColor.Black, new Vector2Int(4, 2));

            board = new ChessBoard(pawn, pawn2);
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
            Vector2Int check = new Vector2Int(xCheck, yCheck);

            Move move = new Move(pawn, check);
            bool actual = move.IsLegal();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReturnsCorrectNumberOfPossibleMoves()
        {
            List<Move> moves = pawn.GetPossibleMoves();
            Assert.IsTrue(moves.Count == 3);
        }
    }
    
    class Scenario2
    {
        Pawn pawn;
        Pawn pawn1;
        Pawn pawn2;
        ChessBoard board;

        public Scenario2()
        {
            pawn = new Pawn(ChessPieceColor.Black, new Vector2Int(3, 6));
            pawn1 = new Pawn(ChessPieceColor.White, new Vector2Int(3, 5));
            pawn2 = new Pawn(ChessPieceColor.White, new Vector2Int(2, 5));

            board = new ChessBoard(pawn, pawn1, pawn2);
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
            Vector2Int check = new Vector2Int(xCheck, yCheck);

            Move move = new Move(pawn, check);
            bool actual = move.IsLegal();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReturnsCorrectNumberOfPossibleMoves()
        {
            List<Move> moves = pawn.GetPossibleMoves();
            Assert.IsTrue(moves.Count == 1);
        }
    }
    class Scenario3
    {
        Pawn pawn;
        Pawn pawn1;
        Pawn pawn2;
        ChessBoard board;

        public Scenario3()
        {
            pawn = new Pawn(ChessPieceColor.Black, new Vector2Int(3, 5));
            pawn1 = new Pawn(ChessPieceColor.Black, new Vector2Int(2, 4));
            pawn2 = new Pawn(ChessPieceColor.White, new Vector2Int(4, 4));

            board = new ChessBoard(pawn, pawn1, pawn2);
        }

        [TestCase(3, 3, false)]
        [TestCase(2, 4, false)]
        [TestCase(2, 5, false)]
        [TestCase(3, 6, false)]
        [TestCase(4, 4, true)]
        [TestCase(3, 4, true)]
        public void ReturnsCorrectMoveLegality(int xCheck, int yCheck, bool expected)
        {
            Vector2Int check = new Vector2Int(xCheck, yCheck);

            Move move = new Move(pawn, check);
            bool actual = move.IsLegal();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReturnsCorrectNumberOfPossibleMoves()
        {
            List<Move> moves = pawn.GetPossibleMoves();
            Assert.IsTrue(moves.Count == 2);
        }
    }
}