using System.Collections.Generic;
using Chess;
using NUnit.Framework;
using UnityEngine;

public class QueenTests
{
    class Scenario1
    {
        Queen queen;
        Pawn pawn1;
        Pawn pawn2;
        Pawn pawn3;
        Rook rook;
        ChessBoard board;

        public Scenario1()
        {
            queen = new Queen(ChessPieceColor.White, new Vector2Int(4, 1));
            pawn1 = new Pawn(ChessPieceColor.White, new Vector2Int(2, 1));
            pawn2 = new Pawn(ChessPieceColor.Black, new Vector2Int(1, 4));
            pawn3 = new Pawn(ChessPieceColor.Black, new Vector2Int(4, 6));
            rook = new Rook(ChessPieceColor.White, new Vector2Int(4, 0));

            board = new ChessBoard(queen, pawn1, pawn2, pawn3, rook);
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
            Vector2Int check = new Vector2Int(xCheck, yCheck);

            Move move = new Move(queen, check);
            bool actual = move.IsLegal();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReturnsCorrectNumberOfPossibleMoves()
        {
            List<Move> moves = queen.GetPossibleMoves();
            Assert.IsTrue(moves.Count == 17);
        }
    }
    
    class Scenario2
    {
        Queen queen;
        Pawn pawn;
        Rook rook;
        ChessBoard board;

        public Scenario2()
        {
            queen = new Queen(ChessPieceColor.Black, new Vector2Int(0, 0));
            pawn = new Pawn(ChessPieceColor.White, new Vector2Int(0, 5));
            rook = new Rook(ChessPieceColor.White, new Vector2Int(7, 7));

            board = new ChessBoard(queen, pawn, rook);
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
            Vector2Int check = new Vector2Int(xCheck, yCheck);

            Move move = new Move(queen, check);
            bool actual = move.IsLegal();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReturnsCorrectNumberOfPossibleMoves()
        {
            List<Move> moves = queen.GetPossibleMoves();
            Assert.IsTrue(moves.Count == 19);
        }
    }
}