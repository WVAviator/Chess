using System.Collections.Generic;
using System.Text.RegularExpressions;
using Chess;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.EditMode
{
    public class ChessBoardTests
    {
        class SetupStandard
        {
            [Test]
            public void CorrectNumberOfPiecesInStandardSetup()
            {
                ChessBoard board = new ChessBoard();
                board.Setup().Standard();
            
                Assert.IsTrue(board.ChessPieces.Count == 32);
            }

            [Test]
            public void CorrectNumberOfPawns()
            {
                ChessBoard board = new ChessBoard();
                board.Setup().Standard();

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
                board.Setup().Standard();

                List<Move> allPossible = board.AllPossibleMoves(ChessPieceColor.White);
                Assert.IsTrue(allPossible.Count == 20);
            }
            [Test]
            public void AllPossibleMovesAfterPawn3MovesCorrect()
            {
                ChessBoard board = new ChessBoard();
                board.Setup().Standard();

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
                board.Setup().Standard();

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
                board.Setup().Standard();

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
                Setup.Board
                    .Place.White<King>().At(5, 0)
                    .Place.Black<Bishop>().At(7, 2)
                    .Get(out var board);
                
                bool actual = board.IsInCheck(ChessPieceColor.White);
                Assert.IsTrue(actual);
            }
        
            [Test]
            public void ReturnsFalseForWhiteWhenWhiteIsNotInCheck()
            {
                Setup.Board
                    .Place.White<King>().At(5, 0)
                    .Place.Black<Bishop>().At(6, 2)
                    .Get(out var board);

                bool actual = board.IsInCheck(ChessPieceColor.White);
                Assert.IsFalse(actual);
            }
        }

        class PlayerTurn
        {
            [Test]
            public void WhiteCannotMoveWhenItsBlacksTurn()
            {
                Setup.Board
                    .BlackGoesFirst
                    .Place.White<Pawn>().At(1, 1).AndGet(out var pawn);

                bool actual = pawn.GetPossibleMoves().Count == 0;
                Assert.IsTrue(actual);
            }
            
        }

        class IsCheckmate
        {
            [Test]
            public void CheckmateMoveRegistersAsCheckmate()
            {
                Setup.Board
                    .Place.White<Rook>().At(1, 2)
                    .Place.White<Rook>().At(0, 1)
                    .Place.Black<King>().At(5, 0)
                    .Move.From(1, 2).To(1, 0).Execute()
                    .Get(out var board);
                
                Assert.IsTrue(board.IsCheckmate(ChessPieceColor.Black));
            }
        }

        class EvaluateScore
        {
            [Test]
            public void PawnEvaluatesToOne()
            {
                Setup.Board
                    .Place.White<Pawn>().At(1, 1)
                    .Get(out var board);
                
                int actual = board.EvaluateScore(ChessPieceColor.White);
                Assert.IsTrue(actual == 1);
            }
            
            [Test]
            public void RookEvaluatesToFive()
            {
                Setup.Board
                    .Place.White<Rook>().At(1, 1)
                    .Get(out var board);
                
                int actual = board.EvaluateScore(ChessPieceColor.White);
                Assert.IsTrue(actual == 5);
            }
            
            [Test]
            public void BishopEvaluatesToThree()
            {
                Setup.Board
                    .Place.White<Bishop>().At(1, 1)
                    .Get(out var board);
                
                int actual = board.EvaluateScore(ChessPieceColor.White);
                Assert.IsTrue(actual == 3);
            }
            
            [Test]
            public void KnightEvaluatesToThree()
            {
                Setup.Board
                    .Place.White<Knight>().At(1, 1)
                    .Get(out var board);
                
                int actual = board.EvaluateScore(ChessPieceColor.White);
                Assert.IsTrue(actual == 3);
            }
            
            [Test]
            public void QueenEvaluatesToNine()
            {
                Setup.Board
                    .Place.White<Queen>().At(1, 1)
                    .Get(out var board);
                
                int actual = board.EvaluateScore(ChessPieceColor.White);
                Assert.IsTrue(actual == 9);
            }
            
            [Test]
            public void KingEvaluatesToFifty()
            {
                Setup.Board
                    .Place.White<King>().At(1, 1)
                    .Get(out var board);
                
                int actual = board.EvaluateScore(ChessPieceColor.White);
                Assert.IsTrue(actual == 100);
            }
            
            [Test]
            public void StandardSetupEvaluatesToCorrectValue()
            {
                Setup.Board.Standard()
                    .Get(out var board);

                int white = board.EvaluateScore(ChessPieceColor.White);
                int black = board.EvaluateScore(ChessPieceColor.Black);
                Assert.IsTrue(white == black && white == 139);
            }
        }
    }
}