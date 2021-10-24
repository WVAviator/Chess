using System.Text.RegularExpressions;
using Chess;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MoveTests
{
    class NewPosition
    {
        [TestCase(0, 0, 1, 1)]
        [TestCase(0, 1, 1, 1)]
        [TestCase(4, 4, 5, 5)]
        public void InitializedMoveReturnsNewPosition(int fromX, int fromY, int toX, int toY)
        {
            Vector2Int moveFrom = new Vector2Int(fromX, fromY);
            Vector2Int moveTo = new Vector2Int(toX, toY);
            
            Pawn pawn = new Pawn(ChessPieceColor.Black, moveFrom);
            Move move = new Move(pawn, moveTo);
            
            Assert.AreEqual(moveTo, move.NewPosition);
        }
        [TestCase(3, 3, 3, 2, -1)]
        [TestCase(3, 3, 2, 4, 1)]
        [TestCase(0, 0, 7, 7, 7)]
        [TestCase(7, 7, 0, 0, -7)]
        [TestCase(5, 5, 4, 5, 0)]
        public void InitializedMoveReturnsCorrectYDifference(int fromX, int fromY, int toX, int toY, int expected)
        {
            Vector2Int moveFrom = new Vector2Int(fromX, fromY);
            Vector2Int moveTo = new Vector2Int(toX, toY);
            
            Pawn pawn = new Pawn(ChessPieceColor.Black, moveFrom);
            Move move = new Move(pawn, moveTo);
            
            Assert.AreEqual(expected, move.YDifference);
        }
        
        [TestCase(3, 3, 3, 2, 0)]
        [TestCase(3, 3, 2, 4, -1)]
        [TestCase(0, 0, 7, 7, 7)]
        [TestCase(7, 7, 0, 0, -7)]
        [TestCase(5, 5, 6, 5, 1)]
        public void InitializedMoveReturnsCorrectXDifference(int fromX, int fromY, int toX, int toY, int expected)
        {
            Vector2Int moveFrom = new Vector2Int(fromX, fromY);
            Vector2Int moveTo = new Vector2Int(toX, toY);
            
            Pawn pawn = new Pawn(ChessPieceColor.Black, moveFrom);
            Move move = new Move(pawn, moveTo);
            
            Assert.AreEqual(expected, move.XDifference);
        }

        [TestCase(0, 0, -1, -1)]
        [TestCase(6, 7, 6, 8)]
        [TestCase(1, 1, -1, 6)]
        [TestCase(1, 1, 1, -1)]
        public void InvalidMoveReturnsNoPositionChange(int fromX, int fromY, int toX, int toY)
        {
            Vector2Int moveFrom = new Vector2Int(fromX, fromY);
            Vector2Int moveTo = new Vector2Int(toX, toY);
            
            Pawn pawn = new Pawn(ChessPieceColor.Black, moveFrom);
            Move move = new Move(pawn, moveTo);
            
            Assert.AreEqual(moveFrom, move.NewPosition);
        }
    }

    class Execute
    {
        [Test]
        public void LegalMoveExecutesSuccessfully()
        {
            Vector2Int start = new Vector2Int(6, 6);
            Vector2Int end = new Vector2Int(6, 4);
            
            Pawn pawn = new Pawn(ChessPieceColor.Black, start);
            ChessBoard board = new ChessBoard(pawn);
            Move move = new Move(pawn, end);
            
            move.Execute();
            Assert.AreEqual(end, pawn.Position);
        }

        [Test]
        public void IllegalMoveFailsToExecute()
        {
            Vector2Int start = new Vector2Int(6, 6);
            Vector2Int end = new Vector2Int(6, 3);
            
            Pawn pawn = new Pawn(ChessPieceColor.Black, start);
            ChessBoard board = new ChessBoard(pawn);
            Move move = new Move(pawn, end);
            
            move.Execute();
            LogAssert.Expect(LogType.Error, new Regex(@".*"));
            Assert.AreEqual(start, pawn.Position);
        }

        

        [Test]
        public void TakingOpponentPieceRemovesPieceFromBoard()
        {
            ChessBoard board = new ChessBoard();

            Vector2Int whitePosition = new Vector2Int(3, 3);
            Vector2Int blackPosition = new Vector2Int(4, 4);

            Pawn whitePawn = new Pawn(ChessPieceColor.White, whitePosition);
            Pawn blackPawn = new Pawn(ChessPieceColor.Black, blackPosition);
            
            board.AddPiece(whitePawn);
            board.AddPiece(blackPawn);

            Move move = new Move(whitePawn, blackPawn.Position);

            move.Execute();

            bool actual = board.ChessPieces.Contains(blackPawn);
            Assert.AreEqual(false, actual);
        }

        
    }

    class Undo
    {
        [Test]
        public void SuccessfulUndoAfterMoveExecuted()
        {
            Vector2Int start = new Vector2Int(6, 6);
            Vector2Int end = new Vector2Int(6, 4);
            
            Pawn pawn = new Pawn(ChessPieceColor.Black, start);
            ChessBoard board = new ChessBoard(pawn);
            Move move = new Move(pawn, end);
            
            move.Execute();
            move.Undo();
            
            Assert.AreEqual(start, pawn.Position);
        }
        [Test]
        public void UndoBeforeExecuteDoesNothing()
        {
            Vector2Int start = new Vector2Int(6, 6);
            Vector2Int end = new Vector2Int(6, 4);
            
            Pawn pawn = new Pawn(ChessPieceColor.Black, start);
            ChessBoard board = new ChessBoard(pawn);
            Move move = new Move(pawn, end);
            
            move.Undo();
            
            Assert.AreEqual(start, pawn.Position);
        }
        [Test]
        public void UndoRestoresOpponentPieceToBoard()
        {
            Vector2Int whitePosition = new Vector2Int(3, 3);
            Vector2Int blackPosition = new Vector2Int(4, 4);

            Pawn whitePawn = new Pawn(ChessPieceColor.White, whitePosition);
            Pawn blackPawn = new Pawn(ChessPieceColor.Black, blackPosition);
            
            ChessBoard board = new ChessBoard(whitePawn, blackPawn);

            Move move = new Move(whitePawn, blackPawn.Position);

            move.Execute();
            move.Undo();

            bool actual = board.ChessPieces.Contains(blackPawn);
            Assert.AreEqual(true, actual);
        }
    }
}