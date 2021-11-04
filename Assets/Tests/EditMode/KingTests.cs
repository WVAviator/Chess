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
            King king;
            Pawn pawn1;
            Pawn pawn2;
            ChessBoard board;

            public Scenario1()
            {
                king = new King(ChessPieceColor.Black, new Vector2Int(4, 3));
                pawn1 = new Pawn(ChessPieceColor.White, new Vector2Int(4, 4));
                pawn2 = new Pawn(ChessPieceColor.Black, new Vector2Int(5, 3));
                board = new ChessBoard(king, pawn1, pawn2);
                board.PlayerTurn = ChessPieceColor.Black;
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
                Vector2Int check = new Vector2Int(xCheck, yCheck);

                Move move = new Move(king, check);
                bool actual = move.IsLegal();
                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void ReturnsCorrectNumberOfPossibleMoves()
            {
                List<Move> moves = king.GetPossibleMoves();
                Assert.IsTrue(moves.Count == 7);
            }
        }
    
        class Scenario2
        {
            King king;
            Pawn pawn;
            Queen queen;
            ChessBoard board;

            public Scenario2()
            {
                king = new King(ChessPieceColor.White, new Vector2Int(3, 2));
                pawn = new Pawn(ChessPieceColor.White, new Vector2Int(3, 1));
                queen = new Queen(ChessPieceColor.Black, new Vector2Int(5, 2));
                board = new ChessBoard(king, pawn, queen);
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
                Vector2Int check = new Vector2Int(xCheck, yCheck);

                Move move = new Move(king, check);
                bool actual = move.IsLegal();
                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void ReturnsCorrectNumberOfPossibleMoves()
            {
                List<Move> moves = king.GetPossibleMoves();
                Assert.IsTrue(moves.Count == 3);
            }
        }

        class Castling
        {

            PieceFactory _black;
            PieceFactory _white;
            public Castling()
            {
                _black = new PieceFactory(ChessPieceColor.Black);
                _white = new PieceFactory(ChessPieceColor.White);
            }
            
            [Test]
            public void WhiteKingCanCastleRight()
            {
                King king = new King(ChessPieceColor.White, new Vector2Int(4, 0));
                Rook rook = new Rook(ChessPieceColor.White, new Vector2Int(7, 0));
                ChessBoard board = new ChessBoard(king, rook);
                board.PlayerTurn = ChessPieceColor.White;

                Move move = new Move(king, new Vector2Int(6, 0));
                Assert.IsTrue(move.IsLegal());
            }

            [Test]
            public void WhiteKingCanCastleLeft()
            {
                King king = new King(ChessPieceColor.White, new Vector2Int(4, 0));
                Rook rook = new Rook(ChessPieceColor.White, new Vector2Int(0, 0));
                ChessBoard board = new ChessBoard(king, rook);
                board.PlayerTurn = ChessPieceColor.White;

                Move move = new Move(king, new Vector2Int(2, 0));
                Assert.IsTrue(move.IsLegal());
            }

            [Test]
            public void BlackKingCanCastleRight()
            {
                King king = new King(ChessPieceColor.Black, new Vector2Int(4, 7));
                Rook rook = new Rook(ChessPieceColor.Black, new Vector2Int(7, 7));
                ChessBoard board = new ChessBoard(king, rook);
                board.PlayerTurn = ChessPieceColor.Black;
                
                Move move = new Move(king, new Vector2Int(6, 7));
                Assert.IsTrue(move.IsLegal());
            }
            
            [Test]
            public void BlackKingCanCastleLeft()
            {
                King king = new King(ChessPieceColor.Black, new Vector2Int(4, 7));
                Rook rook = new Rook(ChessPieceColor.Black, new Vector2Int(0, 7));
                ChessBoard board = new ChessBoard(king, rook);
                board.PlayerTurn = ChessPieceColor.Black;

                Move move = new Move(king, new Vector2Int(2, 7));
                Assert.IsTrue(move.IsLegal());
            }

            [Test]
            public void CastlingNotAllowedIfPieceBlocking()
            {
                King king = new King(ChessPieceColor.Black, new Vector2Int(4, 7));
                Rook rook = new Rook(ChessPieceColor.Black, new Vector2Int(0, 7));
                Bishop bishop = new Bishop(ChessPieceColor.Black, new Vector2Int(3, 7));
                ChessBoard board = new ChessBoard(king, rook, bishop);
                board.PlayerTurn = ChessPieceColor.Black;

                Move move = new Move(king, new Vector2Int(2, 7));
                Assert.IsFalse(move.IsLegal());
            }

            [Test]
            public void RookAlsoMovesWithLegalCastleLeft()
            {
                King king = new King(ChessPieceColor.White, new Vector2Int(4, 0));
                Rook rook = new Rook(ChessPieceColor.White, new Vector2Int(0, 0));
                ChessBoard board = new ChessBoard(king, rook);
                board.PlayerTurn = ChessPieceColor.White;

                Move move = new Move(king, new Vector2Int(2, 0));
                move.Execute();
                
                Assert.IsTrue(rook.Position == new Vector2Int(3, 0));
            }
            
            [Test]
            public void RookAlsoMovesWithLegalCastleRight()
            {
                King king = new King(ChessPieceColor.White, new Vector2Int(4, 0));
                Rook rook = new Rook(ChessPieceColor.White, new Vector2Int(7, 0));
                ChessBoard board = new ChessBoard(king, rook);
                board.PlayerTurn = ChessPieceColor.White;

                Move move = new Move(king, new Vector2Int(6, 0));
                move.Execute();
                
                Debug.Log(rook.Position);
                Assert.IsTrue(rook.Position == new Vector2Int(5, 0));
            }

            [Test]
            public void WhiteCannotCastleThroughCheck()
            {
                ChessBoard board = new ChessBoard
                {
                    [4, 0] = _white.King,
                    [7, 0] = _white.Rook,
                    [5, 7] = _black.Queen
                };

                Move move = board[4, 0].To(6, 0);
                Assert.IsFalse(move.IsLegal());
            }

            [Test]
            public void BlackCannotCastleThroughCheck()
            {
                King king = _black.King;
                ChessBoard board = new ChessBoard
                {
                    [4, 7] = king,
                    [0, 7] = _black.Rook,
                    [3, 0] = _white.Queen,
                    PlayerTurn = ChessPieceColor.Black
                };

                Move move = king.To(2, 7);
                Assert.IsFalse(move.IsLegal());
            }
        }
    }
}