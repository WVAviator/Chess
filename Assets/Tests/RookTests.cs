using System.Collections.Generic;
using Chess;
using NUnit.Framework;
using UnityEngine;

public class RookTests
{
    class IsLegalMove
    {
        [TestCase(0,0,0,6, true)]
        [TestCase(0,0,6,0, true)]
        [TestCase(5,5,2,5, true)]
        [TestCase(4,4,0,4, true)]
        [TestCase(4, 4, 3, 3, false)]
        [TestCase(3, 2, 4, 4, false)]
        [TestCase(3, 3, 3, 3, false)]
        [TestCase(3, 3, 3, 8, false)]
        public void RookVerifiesMoveLegality(int startX, int startY, int endX, int endY, bool expected)
        {
            Vector2Int start = new Vector2Int(startX, startY);
            Vector2Int end = new Vector2Int(endX, endY);

            Rook rook = new Rook(ChessPieceColor.Black, start);

            bool actual = rook.IsLegalMove(end);
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void RookFlagsIllegalMoveWhenBlockedByAlly()
        {
            Vector2Int rookStartPosition = new Vector2Int(0, 0);
            Vector2Int rookEndPosition = new Vector2Int(0, 2);
            Vector2Int pawnPosition = new Vector2Int(0, 1);

            Rook rook = new Rook(ChessPieceColor.White, rookStartPosition);
            Pawn pawn = new Pawn(ChessPieceColor.White, pawnPosition);

            ChessBoard board = new ChessBoard();
            board.AddPiece(rook);
            board.AddPiece(pawn);

            bool actual = rook.IsLegalMove(rookEndPosition);
            Assert.AreEqual(false, actual);
        }

        [Test]
        public void MoveToTakeOpponentPieceIsLegal()
        {
            Vector2Int rookStartPosition = new Vector2Int(0, 0);
            Vector2Int pawnPosition = new Vector2Int(0, 1);

            Rook rook = new Rook(ChessPieceColor.White, rookStartPosition);
            Pawn pawn = new Pawn(ChessPieceColor.Black, pawnPosition);

            ChessBoard board = new ChessBoard();
            board.AddPiece(rook);
            board.AddPiece(pawn);

            bool actual = rook.IsLegalMove(pawnPosition);
            Assert.AreEqual(true, actual);
        }
        
        [Test]
        public void MoveToTakeAllyPieceIsIllegal()
        {
            Vector2Int rookStartPosition = new Vector2Int(0, 0);
            Vector2Int pawnPosition = new Vector2Int(0, 1);

            Rook rook = new Rook(ChessPieceColor.White, rookStartPosition);
            Pawn pawn = new Pawn(ChessPieceColor.White, pawnPosition);

            ChessBoard board = new ChessBoard();
            board.AddPiece(rook);
            board.AddPiece(pawn);

            bool actual = rook.IsLegalMove(pawnPosition);
            Assert.AreEqual(false, actual);
        }
    }
    class GetPossibleMoves
    {
        [Test]
        public void ReturnsCorrectNumberOfPossibleMoves()
        {
            Vector2Int position = new Vector2Int(1, 1);

            Rook piece = new Rook(ChessPieceColor.Black, position);

            List<Move> moves = piece.GetPossibleMoves();
            
            Assert.IsTrue(moves.Count == 14);
        }
    }
}