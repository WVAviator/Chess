using System.Collections.Generic;
using System.Text.RegularExpressions;
using Chess;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ChessBoardTests
{
    class SetupStandard
    {
        [Test]
        public void CorrectNumberOfPiecesInStandardSetup()
        {
            ChessBoard board = new ChessBoard();
            board.SetupStandard();
            
            Assert.IsTrue(board.ChessPieces.Count == 32);
        }

        [Test]
        public void CorrectNumberOfPawns()
        {
            ChessBoard board = new ChessBoard();
            board.SetupStandard();

            int pawnCount = board.ChessPieces.FindAll(p => p is Pawn).Count;
            Assert.IsTrue(pawnCount == 16);
        }
    }

    class AllPossibleMoves
    {
        [Test]
        public void AllPossibleMovesFromDefaultSetupCorrect()
        {
            ChessBoard board = new ChessBoard();
            board.SetupStandard();

            List<Move> allPossible = board.AllPossibleMoves(ChessPieceColor.White);
            Assert.IsTrue(allPossible.Count == 20);
        }
        [Test]
        public void AllPossibleMovesAfterPawn3MovesCorrect()
        {
            ChessBoard board = new ChessBoard();
            board.SetupStandard();

            Vector2Int pawnPosition = new Vector2Int(3, 1);
            Pawn pawn = (Pawn)board.GetPieceAt(pawnPosition);

            Vector2Int newPosition = new Vector2Int(3, 3);
            pawn.MoveTo(newPosition);

            List<Move> allPossible = board.AllPossibleMoves(ChessPieceColor.White);
            Assert.IsTrue(allPossible.Count == 28);
        }
    }

    class MoveHistoryStack
    {
        [Test]
        public void MoveStackReturnsCorrectCount()
        {
            ChessBoard board = new ChessBoard();
            board.SetupStandard();

            List<Move> allPossible = board.AllPossibleMoves(ChessPieceColor.White);
            allPossible[0].Execute();
            allPossible = board.AllPossibleMoves(ChessPieceColor.Black);
            allPossible[0].Execute();

            int actual = board.MoveHistory.Count;
            Assert.IsTrue(actual == 2);
        }

        [Test]
        public void CannotUndoMovesOutOfOrder()
        {
            ChessBoard board = new ChessBoard();
            board.SetupStandard();

            List<Move> allPossible = board.AllPossibleMoves(ChessPieceColor.White);
            Move testMove = allPossible[0];
            testMove.Execute();
            
            allPossible = board.AllPossibleMoves(ChessPieceColor.Black);
            allPossible[0].Execute();
            
            testMove.Undo();
            LogAssert.Expect(LogType.Error, new Regex(@".*"));
            
            int actual = board.MoveHistory.Count;
            Assert.IsTrue(actual == 2);
        }
    }

    class IsInCheck
    {
        [Test]
        public void ReturnsTrueForWhiteWhenWhiteIsInCheck()
        {
            King king = new King(ChessPieceColor.White, new Vector2Int(5, 0));
            Bishop bishop = new Bishop(ChessPieceColor.Black, new Vector2Int(7, 2));
            ChessBoard board = new ChessBoard(king, bishop);

            bool actual = board.IsInCheck(ChessPieceColor.White);
            Assert.IsTrue(actual);
        }
        
        [Test]
        public void ReturnsFalseForWhiteWhenWhiteIsNotInCheck()
        {
            King king = new King(ChessPieceColor.White, new Vector2Int(5, 0));
            Bishop bishop = new Bishop(ChessPieceColor.Black, new Vector2Int(6, 2));
            ChessBoard board = new ChessBoard(king, bishop);

            bool actual = board.IsInCheck(ChessPieceColor.White);
            Assert.IsFalse(actual);
        }
    }
}