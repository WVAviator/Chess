using System.Collections.Generic;
using Chess;
using NUnit.Framework;
using UnityEngine;

namespace Tests.EditMode
{
    public class KingTests
    {

        class Scenario1
        {
            ChessPiece king;

            public Scenario1()
            {
                BoardBuilder.BuildBoard
                    .BlackGoesFirst
                    .Place.Black<King>().At(4, 3).AndGet(out king)
                    .Place.White<Pawn>().At(4, 4)
                    .Place.Black<Pawn>().At(5, 3);
            }
        
            [TestCase(3,3,true)]
            [TestCase(3,4,true)]
            [TestCase(4,4,true)]
            [TestCase(5,4,true)]
            [TestCase(5,3,false)]
            [TestCase(5,2,true)]
            [TestCase(4,2,true)]
            [TestCase(3,2,true)]
            [TestCase(4,1,false)]
            [TestCase(6,3,false)]
            [TestCase(4,5,false)]
            [TestCase(4,3,false)]
            [TestCase(2,3,false)]
            public void ReturnsCorrectMoveLegality(int xCheck, int yCheck, bool expected)
            {
                Move move = king.To(xCheck, yCheck);
                
                bool actual = move.IsLegal();
                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void ReturnsCorrectNumberOfPossibleMoves()
            {
                HashSet<Move> moves = king.GetPossibleMoves();
                Assert.IsTrue(moves.Count == 7);
            }
        }
    
        class Scenario2
        {
            ChessPiece king;

            public Scenario2()
            {
                BoardBuilder.BuildBoard
                    .Place.White<King>().At(3, 2).AndGet(out king)
                    .Place.White<Pawn>().At(3, 1)
                    .Place.Black<Queen>().At(5, 2);
            }
        
            [TestCase(2,1,true)]
            [TestCase(2,2,false)]
            [TestCase(2,3,true)]
            [TestCase(3,3,true)]
            [TestCase(4,3,false)]
            [TestCase(4,2,false)]
            [TestCase(4,1,false)]
            [TestCase(3,1,false)]
            [TestCase(3,0,false)]
            [TestCase(5,2,false)]
            [TestCase(3,2,false)]
            public void ReturnsCorrectMoveLegality(int xCheck, int yCheck, bool expected)
            {
                Move move = king.To(xCheck, yCheck);
                
                bool actual = move.IsLegal();
                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void ReturnsCorrectNumberOfPossibleMoves()
            {
                HashSet<Move> moves = king.GetPossibleMoves();
                Assert.IsTrue(moves.Count == 3);
            }
        }

        class Castling
        {
            [Test]
            public void WhiteKingCanCastleRight()
            {
                BoardBuilder.BuildBoard
                    .Place.White<King>().At(4, 0)
                    .Place.White<Rook>().At(7, 0)
                    .Move.From(4, 0).To(6, 0).AndGet(out Move move);
                Assert.IsTrue(move.IsLegal());
            }

            [Test]
            public void WhiteKingCanCastleLeft()
            {
                BoardBuilder.BuildBoard
                    .Place.White<King>().At(4, 0)
                    .Place.White<Rook>().At(0, 0)
                    .Move.From(4, 0).To(2, 0).AndGet(out Move move);
                Assert.IsTrue(move.IsLegal());
            }

            [Test]
            public void BlackKingCanCastleRight()
            {
                BoardBuilder.BuildBoard
                    .Place.Black<King>().At(4, 7)
                    .Place.Black<Rook>().At(7, 7)
                    .BlackGoesFirst
                    .Move.From(4, 7).To(6, 7).AndGet(out Move move);
                Assert.IsTrue(move.IsLegal());
            }
            
            [Test]
            public void BlackKingCanCastleLeft()
            {
                BoardBuilder.BuildBoard
                    .Place.Black<King>().At(4, 7)
                    .Place.Black<Rook>().At(0, 7)
                    .BlackGoesFirst
                    .Move.From(4, 7).To(2, 7).AndGet(out Move move);
                Assert.IsTrue(move.IsLegal());
            }

            [Test]
            public void CastlingNotAllowedIfPieceBlocking()
            {
                BoardBuilder.BuildBoard
                    .Place.Black<King>().At(4, 7)
                    .Place.Black<Rook>().At(0, 7)
                    .Place.Black<Bishop>().At(3, 7)
                    .BlackGoesFirst
                    .Move.From(4, 7).To(2, 7).AndGet(out Move move);
                
                Assert.IsFalse(move.IsLegal());
            }

            [Test]
            public void RookAlsoMovesWithLegalCastleLeft()
            {
                BoardBuilder.BuildBoard
                    .Place.White<King>().At(4, 0)
                    .Place.White<Rook>().At(0, 0).AndGet(out ChessPiece rook)
                    .Move.From(4, 0).To(2, 0).Execute()
                    .Get(out ChessBoard board);
                Debug.Log(rook.Position);
                Assert.IsTrue(board[3, 0] == rook);
            }
            
            [Test]
            public void RookAlsoMovesWithLegalCastleRight()
            {
                BoardBuilder.BuildBoard
                    .Place.White<King>().At(4, 0)
                    .Place.White<Rook>().At(7, 0).AndGet(out ChessPiece rook)
                    .Move.From(4, 0).To(6, 0).Execute()
                    .Get(out ChessBoard board);
                
                Assert.IsTrue(board[5, 0] == rook);
            }

            [Test]
            public void WhiteCannotCastleThroughCheck()
            {
                BoardBuilder.BuildBoard
                    .Place.White<King>().At(4, 0)
                    .Place.White<Rook>().At(7, 0)
                    .Place.Black<Queen>().At(5, 7)
                    .Move.From(4, 0).To(6, 0).AndGet(out Move move);
                
                Assert.IsFalse(move.IsLegal());
            }
            
            [Test]
            public void BlackCannotCastleThroughCheck()
            {
                BoardBuilder.BuildBoard
                    .Place.Black<King>().At(4, 7)
                    .Place.Black<Rook>().At(0, 7)
                    .Place.White<Queen>().At(3, 0)
                    .Move.From(4, 7).To(2, 7).AndGet(out Move move);
                
                Assert.IsFalse(move.IsLegal());
            }

            [Test]
            public void BlackCastleCanBeUndone()
            {
                BoardBuilder.BuildBoard
                    .Place.Black<King>().At(4, 7).AndGet(out var king)
                    .Place.Black<Rook>().At(0, 7).AndGet(out var rook)
                    .BlackGoesFirst
                    .Move.From(4, 7).To(2, 7).Execute()
                    .ThenUndo()
                    .Get(out ChessBoard board);

                Assert.IsTrue(board[4, 7] == king);
                Assert.IsTrue(board[0, 7] == rook);
            }
        }
    }
}