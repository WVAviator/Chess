using System.Collections.Generic;
using Chess;
using NUnit.Framework;
using UnityEngine;

public class BishopTests
{
    class Scenario1
    {

        Bishop bishop;
        Pawn pawn1;
        Pawn pawn2;
        Pawn pawn3;
        ChessBoard board;

        public Scenario1()
        {
            bishop = new Bishop(ChessPieceColor.White, new Vector2Int(5, 3));
            pawn1 = new Pawn(ChessPieceColor.Black, new Vector2Int(5, 5));
            pawn2 = new Pawn(ChessPieceColor.White, new Vector2Int(3, 5));
            pawn3 = new Pawn(ChessPieceColor.Black, new Vector2Int(3,1));
            board = new ChessBoard(bishop, pawn1, pawn2, pawn3);
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
            Vector2Int check = new Vector2Int(xCheck, yCheck);

            Move move = new Move(bishop, check);
            bool actual = move.IsLegal();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReturnsCorrectNumberOfPossibleMoves()
        {
            List<Move> moves = bishop.GetPossibleMoves();
            Assert.IsTrue(moves.Count == 7);
        }
        
    }
    class Scenario2
    {
        Bishop bishop;
        Pawn pawn1;
        Pawn pawn2;
        ChessBoard board;

        public Scenario2()
        {
            bishop = new Bishop(ChessPieceColor.White, new Vector2Int(2, 2));
            pawn1 = new Pawn(ChessPieceColor.Black, new Vector2Int(1, 3));
            pawn2 = new Pawn(ChessPieceColor.White, new Vector2Int(5, 5));
            board = new ChessBoard(bishop, pawn1, pawn2);
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
            Vector2Int check = new Vector2Int(xCheck, yCheck);

            Move move = new Move(bishop, check);
            bool actual = move.IsLegal();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReturnsCorrectNumberOfPossibleMoves()
        {
            List<Move> moves = bishop.GetPossibleMoves();
            Assert.IsTrue(moves.Count == 7);
        }
    }
}