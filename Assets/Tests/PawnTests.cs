using System.Collections.Generic;
using System.Linq;
using Chess;
using NUnit.Framework;
using UnityEngine;

public class PawnTests
{
    class IsLegalMove
    {
        [TestCase(0, 1, 0, 2)]
        [TestCase(0, 1, 0, 3)]
        [TestCase(2, 2, 2, 3)]
        public void WhitePawnVerifiesLegalMove(int startX, int startY, int endX, int endY)
        {
            Vector2Int start = new Vector2Int(startX, startY);
            Vector2Int end = new Vector2Int(endX, endY);

            Pawn pawn = new Pawn(ChessPieceColor.White, start);
            bool actual = pawn.IsLegalMove(end);
            Assert.AreEqual(true, actual);
        }

        [TestCase(0, 6, 0, 5)]
        [TestCase(0, 6, 0, 4)]
        [TestCase(4, 4, 4, 3)]
        public void BlackPawnVerifiesLegalMove(int startX, int startY, int endX, int endY)
        {
            Vector2Int start = new Vector2Int(startX, startY);
            Vector2Int end = new Vector2Int(endX, endY);

            Pawn pawn = new Pawn(ChessPieceColor.Black, start);
            bool actual = pawn.IsLegalMove(end);
            Assert.AreEqual(true, actual);
        }
        
        [TestCase(0, 2, 1, 2)]
        [TestCase(4, 4, 4, 3)]
        [TestCase(0, 1, 1, 1)]
        [TestCase(0, 2, 0, 4)]
        [TestCase(0, 0, 0, 0)]
        public void WhitePawnFlagsIllegalMove(int startX, int startY, int endX, int endY)
        {
            Vector2Int start = new Vector2Int(startX, startY);
            Vector2Int end = new Vector2Int(endX, endY);

            Pawn pawn = new Pawn(ChessPieceColor.White, start);
            bool actual = pawn.IsLegalMove(end);
            Assert.AreEqual(false, actual);
        }
        
        [TestCase(6, 6, 5, 6)]
        [TestCase(5, 5, 6, 5)]
        [TestCase(4, 4, 4, 5)]
        [TestCase(4, 4, 4, 2)]
        [TestCase(4,4,4,4)]
        public void BlackPawnFlagsIllegalMove(int startX, int startY, int endX, int endY)
        {
            Vector2Int start = new Vector2Int(startX, startY);
            Vector2Int end = new Vector2Int(endX, endY);

            Pawn pawn = new Pawn(ChessPieceColor.Black, start);
            bool actual = pawn.IsLegalMove(end);
            Assert.AreEqual(false, actual);
        }

        [TestCase(2, 2, 3, 3, true)]
        [TestCase(3, 3, 2, 4, true)]
        [TestCase(3, 3, 3, 4, false)]
        [TestCase(3, 3,3, 5, false)]
        [TestCase(3, 3, 3, 2, false)]
        [TestCase(3, 3, 2, 3, false)]
        public void WhitePawnCanTakeBlackPawn(int whiteX, int whiteY, int blackX, int blackY, bool expected)
        {
            Vector2Int whitePosition = new Vector2Int(whiteX, whiteY);
            Pawn whitePawn = new Pawn(ChessPieceColor.White, whitePosition);

            Vector2Int blackPosition = new Vector2Int(blackX, blackY);
            Pawn blackPawn = new Pawn(ChessPieceColor.Black, blackPosition);

            ChessBoard board = new ChessBoard();
            board.AddPiece(whitePawn);
            board.AddPiece(blackPawn);
            
            bool actual = whitePawn.IsLegalMove(blackPawn.Position);
            Assert.AreEqual(expected, actual);
        }
        
        [TestCase(4, 4, 5, 5, true)]
        [TestCase(4, 4, 3, 5, true)]
        [TestCase(4, 4, 4, 5, false)]
        [TestCase(4, 4, 4, 3, false)]
        [TestCase(4, 4, 5, 4, false)]
        public void BlackPawnCanTakeWhitePawn(int whiteX, int whiteY, int blackX, int blackY, bool expected)
        {
            Vector2Int whitePosition = new Vector2Int(whiteX, whiteY);
            Pawn whitePawn = new Pawn(ChessPieceColor.White, whitePosition);

            Vector2Int blackPosition = new Vector2Int(blackX, blackY);
            Pawn blackPawn = new Pawn(ChessPieceColor.Black, blackPosition);
            
            ChessBoard board = new ChessBoard();
            board.AddPiece(whitePawn);
            board.AddPiece(blackPawn);

            bool actual = blackPawn.IsLegalMove(whitePawn.Position);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void WhitePawnCannotJumpOverBlackPawn()
        {
            Vector2Int whitePosition = new Vector2Int(3, 1);
            Pawn whitePawn = new Pawn(ChessPieceColor.White, whitePosition);

            Vector2Int blackPosition = new Vector2Int(3, 2);
            Pawn blackPawn = new Pawn(ChessPieceColor.Black, blackPosition);
            
            ChessBoard board = new ChessBoard();
            board.AddPiece(whitePawn);
            board.AddPiece(blackPawn);

            bool actual = whitePawn.IsLegalMove(new Vector2Int(3, 3));
            Assert.AreEqual(false, actual);
        }
        [Test]
        public void BlackPawnCannotJumpOverWhitePawn()
        {
            Vector2Int whitePosition = new Vector2Int(3, 5);
            Pawn whitePawn = new Pawn(ChessPieceColor.White, whitePosition);

            Vector2Int blackPosition = new Vector2Int(3, 6);
            Pawn blackPawn = new Pawn(ChessPieceColor.Black, blackPosition);
            
            ChessBoard board = new ChessBoard();
            board.AddPiece(whitePawn);
            board.AddPiece(blackPawn);

            bool actual = blackPawn.IsLegalMove(new Vector2Int(3, 4));
            Assert.AreEqual(false, actual);
        }
    }

    class GetPossibleMoves
    {
        [Test]
        public void PawnGivesListOfMovesAtStart()
        {
            Vector2Int pawnPosition = new Vector2Int(2, 1);
            Pawn pawn = new Pawn(ChessPieceColor.White, pawnPosition);
            
            ChessBoard board = new ChessBoard(pawn);

            List<Move> possibleMoves = pawn.GetPossibleMoves();

            Vector2Int expected01 = new Vector2Int(2, 2);
            Vector2Int expected02 = new Vector2Int(2, 3);
            
            Assert.IsTrue(possibleMoves.Any(m => m.NewPosition == expected01));
            Assert.IsTrue(possibleMoves.Any(m => m.NewPosition == expected02));
            Assert.IsTrue(possibleMoves.Count == 2);
        }

        [Test]
        public void PawnGivesEmptyListOfPossibleMovesWhenBlocked()
        {
            Vector2Int pawnPosition = new Vector2Int(2, 1);
            Vector2Int bishopPosition = new Vector2Int(2, 2);
            
            Pawn pawn = new Pawn(ChessPieceColor.White, pawnPosition);
            Bishop bishop = new Bishop(ChessPieceColor.Black, bishopPosition);

            ChessBoard board = new ChessBoard();
            board.AddPiece(pawn);
            board.AddPiece(bishop);

            List<Move> possibleMoves = pawn.GetPossibleMoves();

            Assert.IsTrue(possibleMoves.Count == 0);
        }

        [Test]
        public void PawnListsEnemiesInPossibleMovesWhenTheyAreDiagonal()
        {
            Vector2Int pawnPosition = new Vector2Int(2, 1);
            Vector2Int bishopPosition = new Vector2Int(3, 2);
            Vector2Int knightPosition = new Vector2Int(1, 2);
            
            Pawn pawn = new Pawn(ChessPieceColor.White, pawnPosition);
            Bishop bishop = new Bishop(ChessPieceColor.Black, bishopPosition);
            Knight knight = new Knight(ChessPieceColor.White, knightPosition);

            ChessBoard board = new ChessBoard();
            board.AddPiece(pawn);
            board.AddPiece(bishop);
            board.AddPiece(knight);

            List<Move> possibleMoves = pawn.GetPossibleMoves();

            Assert.IsTrue(possibleMoves.Any(m => m.NewPosition == bishopPosition));
            Assert.IsTrue(possibleMoves.Count == 3);
        }
    }
}