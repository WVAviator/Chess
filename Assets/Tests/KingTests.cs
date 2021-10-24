using System.Collections.Generic;
using Chess;
using NUnit.Framework;
using UnityEngine;

public class KingTests
{
    class IsLegalMove
    {
        [TestCase(2,2,2,3,true)]
        [TestCase(2, 2, 3, 4, false)]
        [TestCase(0,0,0,0,false)]
        [TestCase(2, 2, 3, 2, true)]
        [TestCase(0, 0, 0, -1, false)]
        public void KingVerifiesMoveLegality(int startX, int startY, int endX, int endY, bool expected)
        {
            Vector2Int start = new Vector2Int(startX, startY);
            Vector2Int end = new Vector2Int(endX, endY);

            King king = new King(ChessPieceColor.Black, start);

            bool actual = king.IsLegalMove(end);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void KingCannotMoveToSpaceOccupiedByAlly()
        {
            Vector2Int kingPosition = new Vector2Int(0, 0);
            Vector2Int pawnPosition = new Vector2Int(0, 1);

            Pawn pawn = new Pawn(ChessPieceColor.Black, pawnPosition);
            King king = new King(ChessPieceColor.Black, kingPosition);

            ChessBoard board = new ChessBoard();
            board.AddPiece(king);
            board.AddPiece(pawn);

            bool actual = king.IsLegalMove(pawnPosition);
            Assert.AreEqual(false, actual);
        }
    }

    class GetPossibleMoves
    {
        [Test]
        public void ReturnsCorrectNumberOfPossibleMoves()
        {
            Vector2Int kingPosition = new Vector2Int(3, 3);

            King king = new King(ChessPieceColor.Black, kingPosition);

            List<Move> moves = king.GetPossibleMoves();
            
            Assert.IsTrue(moves.Count == 8);
        }
    }
}