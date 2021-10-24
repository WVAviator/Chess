using System.Collections.Generic;
using Chess;
using NUnit.Framework;
using UnityEngine;

public class BishopTests
{
    class IsLegalMove
    {
        [TestCase(3, 3, 4, 4, true)]
        [TestCase(3, 3, 2, 4, true)]
        [TestCase(3, 3, 0, 0, true)]
        [TestCase(0, 7, 7, 0, true)]
        [TestCase(3, 3, 3, 5, false)]
        [TestCase(3, 3,3,3,false)]
        [TestCase(3, 3, 0, 3, false)]
        [TestCase(3, 3, 8, 8, false)]
        public void BishopVerifiesMoveLegality(int startX, int startY, int endX, int endY, bool expected)
        {
            Vector2Int start = new Vector2Int(startX, startY);
            Vector2Int end = new Vector2Int(endX, endY);

            Bishop bishop = new Bishop(ChessPieceColor.Black, start);

            bool actual = bishop.IsLegalMove(end);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void BishopFlagsIllegalMoveWhenBlocked()
        {
            Vector2Int bishopStartPosition = new Vector2Int(0, 0);
            Vector2Int bishopEndPosition = new Vector2Int(3, 3);
            Vector2Int pawnPosition = new Vector2Int(1, 1);

            Bishop bishop = new Bishop(ChessPieceColor.White, bishopStartPosition);
            Pawn pawn = new Pawn(ChessPieceColor.White, pawnPosition);

            ChessBoard board = new ChessBoard();
            board.AddPiece(bishop);
            board.AddPiece(pawn);

            bool actual = bishop.IsLegalMove(bishopEndPosition);
            Assert.AreEqual(false, actual);
        }
        
        [Test]
        public void BishopFlagsIllegalMoveWhenBlocked2()
        {
            Vector2Int bishopStartPosition = new Vector2Int(3, 3);
            Vector2Int bishopEndPosition = new Vector2Int(0, 6);
            Vector2Int pawnPosition = new Vector2Int(1, 5);

            Bishop bishop = new Bishop(ChessPieceColor.White, bishopStartPosition);
            Pawn pawn = new Pawn(ChessPieceColor.White, pawnPosition);

            ChessBoard board = new ChessBoard();
            board.AddPiece(bishop);
            board.AddPiece(pawn);

            bool actual = bishop.IsLegalMove(bishopEndPosition);
            Assert.AreEqual(false, actual);
        }
        
        [Test]
        public void MoveToTakeOpponentPieceIsLegal()
        {
            Vector2Int bishopStartPosition = new Vector2Int(0, 0);
            Vector2Int pawnPosition = new Vector2Int(1, 1);

            Bishop bishop = new Bishop(ChessPieceColor.White, bishopStartPosition);
            Pawn pawn = new Pawn(ChessPieceColor.Black, pawnPosition);

            ChessBoard board = new ChessBoard();
            board.AddPiece(bishop);
            board.AddPiece(pawn);

            bool actual = bishop.IsLegalMove(pawnPosition);
            Assert.AreEqual(true, actual);
        }
        
        [Test]
        public void MoveToTakeAllyPieceIsIllegal()
        {
            Vector2Int bishopStartPosition = new Vector2Int(0, 0);
            Vector2Int pawnPosition = new Vector2Int(1, 1);

            Bishop bishop = new Bishop(ChessPieceColor.White, bishopStartPosition);
            Pawn pawn = new Pawn(ChessPieceColor.White, pawnPosition);

            ChessBoard board = new ChessBoard();
            board.AddPiece(bishop);
            board.AddPiece(pawn);

            bool actual = bishop.IsLegalMove(pawnPosition);
            Assert.AreEqual(false, actual);
        }
    }
    class GetPossibleMoves
    {
        [Test]
        public void ReturnsCorrectNumberOfPossibleMoves()
        {
            Vector2Int position = new Vector2Int(1, 1);

            Bishop piece = new Bishop(ChessPieceColor.Black, position);
            ChessBoard board = new ChessBoard(piece);

            List<Move> moves = piece.GetPossibleMoves();
            
            Assert.IsTrue(moves.Count == 9);
        }
    }
}