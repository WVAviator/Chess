using System.Collections.Generic;
using Chess;
using NUnit.Framework;
using UnityEngine;

public class KnightTests
{

    class Scenario1
    {
        Knight knight;
        Pawn pawn1;
        Pawn pawn2;
        ChessBoard board;

        public Scenario1()
        {
            knight = new Knight(ChessPieceColor.White, new Vector2Int(1, 5));
            pawn1 = new Pawn(ChessPieceColor.White, new Vector2Int(3, 6));
            pawn2 = new Pawn(ChessPieceColor.Black, new Vector2Int(3, 4));
            board = new ChessBoard(knight, pawn1, pawn2);
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
            Vector2Int check = new Vector2Int(xCheck, yCheck);

            Move move = new Move(knight, check);
            bool actual = move.IsLegal();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReturnsCorrectNumberOfPossibleMoves()
        {
            List<Move> moves = knight.GetPossibleMoves();
            Assert.IsTrue(moves.Count == 5);
        }
    }

    class Scenario2
    {
        Knight knight;
        Pawn pawn1;
        Pawn pawn2;
        Pawn pawn3;
        Pawn pawn4;
        ChessBoard board;

        public Scenario2()
        {
            knight = new Knight(ChessPieceColor.Black, new Vector2Int(3, 2));
            pawn1 = new Pawn(ChessPieceColor.White, new Vector2Int(4, 2));
            pawn2 = new Pawn(ChessPieceColor.White, new Vector2Int(2, 4));
            pawn3 = new Pawn(ChessPieceColor.Black, new Vector2Int(4, 3));
            pawn4 = new Pawn(ChessPieceColor.Black, new Vector2Int(4, 4));

            board = new ChessBoard(knight, pawn1, pawn2, pawn3, pawn4);
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
            Vector2Int check = new Vector2Int(xCheck, yCheck);

            Move move = new Move(knight, check);
            bool actual = move.IsLegal();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReturnsCorrectNumberOfPossibleMoves()
        {
            List<Move> moves = knight.GetPossibleMoves();
            Assert.IsTrue(moves.Count == 7);
        }
    }
}