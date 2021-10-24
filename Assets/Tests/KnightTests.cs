using System.Collections.Generic;
using Chess;
using NUnit.Framework;
using UnityEngine;

public class KnightTests
{
    class IsLegalMove
    {
        [TestCase(4, 4, 3, 2, true)]
        [TestCase(4, 4, 3, 6, true)]
        [TestCase(4, 4, 2, 5, true)]
        [TestCase(4, 4, 6, 5, true)]
        [TestCase(4, 4, 6, 6, false)]
        [TestCase(4, 4, 5, 5, false)]
        public void KnightVerifiesMoveLegality(int startX, int startY, int endX, int endY, bool expected)
        {
            Vector2Int start = new Vector2Int(startX, startY);
            Vector2Int end = new Vector2Int(endX, endY);

            Knight knight = new Knight(ChessPieceColor.Black, start);

            bool actual = knight.IsLegalMove(end);
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void MoveToTakeOpponentPieceIsLegal()
        {
            Vector2Int knightStartPosition = new Vector2Int(0, 0);
            Vector2Int pawnPosition = new Vector2Int(2, 1);

            Knight knight = new Knight(ChessPieceColor.White, knightStartPosition);
            Pawn pawn = new Pawn(ChessPieceColor.Black, pawnPosition);

            ChessBoard board = new ChessBoard();
            board.AddPiece(knight);
            board.AddPiece(pawn);

            bool actual = knight.IsLegalMove(pawnPosition);
            Assert.AreEqual(true, actual);
        }
        
        [Test]
        public void MoveToTakeAllyPieceIsIllegal()
        {
            Vector2Int knightStartPosition = new Vector2Int(0, 0);
            Vector2Int pawnPosition = new Vector2Int(2, 1);

            Knight knight = new Knight(ChessPieceColor.White, knightStartPosition);
            Pawn pawn = new Pawn(ChessPieceColor.White, pawnPosition);

            ChessBoard board = new ChessBoard();
            board.AddPiece(knight);
            board.AddPiece(pawn);

            bool actual = knight.IsLegalMove(pawnPosition);
            Assert.AreEqual(false, actual);
        }
    }
    class GetPossibleMoves
    {
        [Test]
        public void ReturnsCorrectNumberOfPossibleMoves()
        {
            Vector2Int knightPosition = new Vector2Int(1, 1);

            Knight knight = new Knight(ChessPieceColor.Black, knightPosition);

            List<Move> moves = knight.GetPossibleMoves();
            
            Assert.IsTrue(moves.Count == 4);
        }
    }
}

public class QueenTests
{
    class IsLegalMove
    {
        [TestCase(3, 3, 4, 4, true)]
        [TestCase(3, 3, 2, 4, true)]
        [TestCase(3, 3, 0, 0, true)]
        [TestCase(0, 7, 7, 0, true)]
        [TestCase(3, 3, 3, 5, true)]
        [TestCase(3, 3,3,3,false)]
        [TestCase(3, 3, 0, 3, true)]
        [TestCase(3, 3, 8, 8, false)]
        [TestCase(3, 3, 4, 5, false)]
        public void QueenVerifiesMoveLegality(int startX, int startY, int endX, int endY, bool expected)
        {
            Vector2Int start = new Vector2Int(startX, startY);
            Vector2Int end = new Vector2Int(endX, endY);

            Queen queen = new Queen(ChessPieceColor.Black, start);

            bool actual = queen.IsLegalMove(end);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void QueenVerifiesIllegalMoveWhenBlocked()
        {
            Vector2Int queenStart = new Vector2Int(0, 0);
            Vector2Int queenEnd = new Vector2Int(6, 6);
            Vector2Int pawnPosition = new Vector2Int(3, 3);

            Queen queen = new Queen(ChessPieceColor.Black, queenStart);
            Pawn pawn = new Pawn(ChessPieceColor.Black, pawnPosition);

            ChessBoard board = new ChessBoard();
            board.AddPiece(queen);
            board.AddPiece(pawn);
            
            bool actual = queen.IsLegalMove(queenEnd);
            Assert.AreEqual(false, actual);
        }
        
        [Test]
        public void QueenVerifiesIllegalMoveWhenBlocked2()
        {
            Vector2Int queenStart = new Vector2Int(0, 0);
            Vector2Int queenEnd = new Vector2Int(0, 6);
            Vector2Int pawnPosition = new Vector2Int(0, 3);

            Queen queen = new Queen(ChessPieceColor.Black, queenStart);
            Pawn pawn = new Pawn(ChessPieceColor.Black, pawnPosition);

            ChessBoard board = new ChessBoard();
            board.AddPiece(queen);
            board.AddPiece(pawn);
            
            bool actual = queen.IsLegalMove(queenEnd);
            Assert.AreEqual(false, actual);
        }
        
        [Test]
        public void MoveToTakeOpponentPieceIsLegal()
        {
            Vector2Int queenStartPosition = new Vector2Int(6, 5);
            Vector2Int pawnPosition = new Vector2Int(2, 1);

            Queen queen = new Queen(ChessPieceColor.White, queenStartPosition);
            Pawn pawn = new Pawn(ChessPieceColor.Black, pawnPosition);

            ChessBoard board = new ChessBoard();
            board.AddPiece(queen);
            board.AddPiece(pawn);

            bool actual = queen.IsLegalMove(pawnPosition);
            Assert.AreEqual(true, actual);
        }
        
        [Test]
        public void MoveToTakeAllyPieceIsIllegal()
        {
            Vector2Int queenStartPosition = new Vector2Int(6, 5);
            Vector2Int pawnPosition = new Vector2Int(2, 1);

            Queen queen = new Queen(ChessPieceColor.White, queenStartPosition);
            Pawn pawn = new Pawn(ChessPieceColor.White, pawnPosition);

            ChessBoard board = new ChessBoard();
            board.AddPiece(queen);
            board.AddPiece(pawn);

            bool actual = queen.IsLegalMove(pawnPosition);
            Assert.AreEqual(false, actual);
        }
        
    }
    class GetPossibleMoves
    {
        [Test]
        public void ReturnsCorrectNumberOfPossibleMoves()
        {
            Vector2Int position = new Vector2Int(1, 1);

            Queen piece = new Queen(ChessPieceColor.Black, position);

            List<Move> moves = piece.GetPossibleMoves();
            
            Assert.IsTrue(moves.Count == 23);
        }
    }
}