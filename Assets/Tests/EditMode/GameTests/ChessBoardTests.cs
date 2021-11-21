using System.Collections.Generic;
using System.Linq;
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
                board.Setup().StandardSetup();
            
                Assert.IsTrue(board.ChessPieces.Count == 32);
            }

            [Test]
            public void CorrectNumberOfPawns()
            {
                ChessBoard board = new ChessBoard();
                board.Setup().StandardSetup();

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
                board.Setup().StandardSetup();

                HashSet<Move> allPossible = board.AllPossibleMoves(ChessPieceColor.White);
                Assert.IsTrue(allPossible.Count == 20);
            }
            [Test]
            public void AllPossibleMovesAfterPawn3MovesCorrect()
            {
                ChessBoard board = new ChessBoard();
                board.Setup().StandardSetup();

                Vector2Int pawnPosition = new Vector2Int(3, 1);
                Pawn pawn = (Pawn)board.GetPieceAt(pawnPosition);

                Vector2Int newPosition = new Vector2Int(3, 3);
                pawn.MoveTo(newPosition);

                HashSet<Move> allPossible = board.AllPossibleMoves(ChessPieceColor.White);
                Assert.IsTrue(allPossible.Count == 28);
            }
        }

        class MoveHistoryStack
        {
            [Test]
            public void MoveStackReturnsCorrectCount()
            {
                ChessBoard board = new ChessBoard();
                board.Setup().StandardSetup();

                HashSet<Move> allPossible = board.AllPossibleMoves(ChessPieceColor.White);
                allPossible.ElementAt(0).Execute();
                allPossible = board.AllPossibleMoves(ChessPieceColor.Black);
                allPossible.ElementAt(0).Execute();

                int actual = board.MoveHistory.Count;
                Assert.IsTrue(actual == 2);
            }
        }

        class IsInCheck
        {
            [Test]
            public void ReturnsTrueForWhiteWhenWhiteIsInCheck()
            {
                BoardBuilder.BuildBoard
                    .Place.White<King>().At(5, 0)
                    .Place.Black<Bishop>().At(7, 2)
                    .Get(out var board);
                
                bool actual = board.IsInCheck(ChessPieceColor.White);
                Assert.IsTrue(actual);
            }
        
            [Test]
            public void ReturnsFalseForWhiteWhenWhiteIsNotInCheck()
            {
                BoardBuilder.BuildBoard
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
                BoardBuilder.BuildBoard
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
                BoardBuilder.BuildBoard
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
                BoardBuilder.BuildBoard
                    .Place.White<Pawn>().At(1, 1)
                    .Get(out var board);
                
                int actual = board.EvaluateScore(ChessPieceColor.White);
                Assert.IsTrue(actual == 1);
            }
            
            [Test]
            public void RookEvaluatesToFive()
            {
                BoardBuilder.BuildBoard
                    .Place.White<Rook>().At(1, 1)
                    .Get(out var board);
                
                int actual = board.EvaluateScore(ChessPieceColor.White);
                Assert.IsTrue(actual == 5);
            }
            
            [Test]
            public void BishopEvaluatesToThree()
            {
                BoardBuilder.BuildBoard
                    .Place.White<Bishop>().At(1, 1)
                    .Get(out var board);
                
                int actual = board.EvaluateScore(ChessPieceColor.White);
                Assert.IsTrue(actual == 3);
            }
            
            [Test]
            public void KnightEvaluatesToThree()
            {
                BoardBuilder.BuildBoard
                    .Place.White<Knight>().At(1, 1)
                    .Get(out var board);
                
                int actual = board.EvaluateScore(ChessPieceColor.White);
                Assert.IsTrue(actual == 3);
            }
            
            [Test]
            public void QueenEvaluatesToNine()
            {
                BoardBuilder.BuildBoard
                    .Place.White<Queen>().At(1, 1)
                    .Get(out var board);
                
                int actual = board.EvaluateScore(ChessPieceColor.White);
                Assert.IsTrue(actual == 9);
            }
            
            [Test]
            public void KingEvaluatesToOneHundred()
            {
                BoardBuilder.BuildBoard
                    .Place.White<King>().At(1, 1)
                    .Get(out var board);
                
                int actual = board.EvaluateScore(ChessPieceColor.White);
                Assert.IsTrue(actual == 100);
            }
            
            [Test]
            public void StandardSetupEvaluatesToCorrectValue()
            {
                BoardBuilder.BuildBoard.StandardSetup()
                    .Get(out var board);

                int white = board.EvaluateScore(ChessPieceColor.White);
                int black = board.EvaluateScore(ChessPieceColor.Black);
                Assert.IsTrue(white == black && white == 139);
            }
        }

        class SetupFromTextString
        {
            [Test]
            public void KnightsAppearInCorrectPositions()
            {
                string testString =
                    "--------" +
                    "--n-----" +
                    "--------" +
                    "--------" +
                    "--------" +
                    "------N-" +
                    "--------" +
                    "--------";
                
                BoardBuilder.BuildBoard.WithString(testString)
                    .Get(out var board);
                
                Assert.IsTrue(board[6, 2] is Knight && board[6, 2].Color == ChessPieceColor.White);
                Assert.IsTrue(board[2, 6] is Knight && board[2, 6].Color == ChessPieceColor.Black);
                Assert.IsTrue(board.ChessPieces.Count == 2);
            }
            
            [Test]
            public void BishopsAppearInCorrectPositions()
            {
                string testString =
                    "-------B" +
                    "--------" +
                    "--------" +
                    "--------" +
                    "--------" +
                    "--------" +
                    "--------" +
                    "b-------";
                
                BoardBuilder.BuildBoard.WithString(testString)
                    .Get(out var board);
                
                Assert.IsTrue(board[0, 0] is Bishop && board[0, 0].Color == ChessPieceColor.Black);
                Assert.IsTrue(board[7, 7] is Bishop && board[7, 7].Color == ChessPieceColor.White);
                Assert.IsTrue(board.ChessPieces.Count == 2);
            }
            
            [Test]
            public void RooksAppearInCorrectPositions()
            {
                string testString =
                    "--------" +
                    "--r-----" +
                    "--------" +
                    "--------" +
                    "--------" +
                    "--R-----" +
                    "--------" +
                    "--------";
                
                BoardBuilder.BuildBoard.WithString(testString)
                    .Get(out var board);
                
                Assert.IsTrue(board[2, 6] is Rook && board[2, 6].Color == ChessPieceColor.Black);
                Assert.IsTrue(board[2, 2] is Rook && board[2, 2].Color == ChessPieceColor.White);
                Assert.IsTrue(board.ChessPieces.Count == 2);
            }
            
            [Test]
            public void QueensAppearInCorrectPositions()
            {
                string testString =
                    "--------" +
                    "--------" +
                    "---Q----" +
                    "--------" +
                    "---q----" +
                    "--------" +
                    "--------" +
                    "--------";
                
                BoardBuilder.BuildBoard.WithString(testString)
                    .Get(out var board);
                
                Assert.IsTrue(board[3, 3] is Queen && board[3, 3].Color == ChessPieceColor.Black);
                Assert.IsTrue(board[3, 5] is Queen && board[3, 5].Color == ChessPieceColor.White);
                Assert.IsTrue(board.ChessPieces.Count == 2);
            }
            
            [Test]
            public void KingsAppearInCorrectPositions()
            {
                string testString =
                    "--------" +
                    "--------" +
                    "--------" +
                    "-------K" +
                    "--------" +
                    "--------" +
                    "--------" +
                    "k-------";
                
                BoardBuilder.BuildBoard.WithString(testString)
                    .Get(out var board);
                
                Assert.IsTrue(board[7, 4] is King && board[7, 4].Color == ChessPieceColor.White);
                Assert.IsTrue(board[0, 0] is King && board[0, 0].Color == ChessPieceColor.Black);
                Assert.IsTrue(board.ChessPieces.Count == 2);
            }
            
            [Test]
            public void PawnsAppearInCorrectPositions()
            {
                string testString =
                    "------P-" +
                    "--------" +
                    "--------" +
                    "--------" +
                    "--------" +
                    "--------" +
                    "p-------" +
                    "--------";
                
                BoardBuilder.BuildBoard.WithString(testString)
                    .Get(out var board);
                
                Assert.IsTrue(board[0, 1] is Pawn && board[0, 1].Color == ChessPieceColor.Black);
                Assert.IsTrue(board[6, 7] is Pawn && board[6, 7].Color == ChessPieceColor.White);
                Assert.IsTrue(board.ChessPieces.Count == 2);
            }

            [Test]
            public void BoardStringInIsSameAsBoardStringOut()
            {
                string input =
                    "-K----P-" +
                    "--------" +
                    "---q----" +
                    "-----n--" +
                    "-k------" +
                    "--------" +
                    "p----Q--" +
                    "--B-----";
                BoardBuilder.BuildBoard.WithString(input)
                    .Get(out var board);
                
                string output = board.ConvertToBoardString();
                Assert.AreEqual(input, output);
            }
            
        }
    }
}