using System.Collections.Generic;
using Chess;
using NUnit.Framework;
using UnityEngine;

namespace Tests.EditMode
{
    public class RookTests
    {
        class Scenario1
        {
            Rook rook;
            Pawn pawn1;
            Pawn pawn2;
            ChessBoard board;

            public Scenario1()
            {
                rook = new Rook(ChessPieceColor.White, new Vector2Int(1, 2));
                pawn1 = new Pawn(ChessPieceColor.White, new Vector2Int(1, 6));
                pawn2 = new Pawn(ChessPieceColor.Black, new Vector2Int(3, 2));

                board = new ChessBoard(pawn1, pawn2, rook);
            }

            [TestCase(1, 6, false)]
            [TestCase(1, 7, false)]
            [TestCase(2, 3, false)]
            [TestCase(-1, 2, false)]
            [TestCase(1, 2, false)]
            [TestCase(4, 2, false)]
            [TestCase(7, 2, false)]
            [TestCase(3, 2, true)]
            [TestCase(0, 2, true)]
            [TestCase(1, 5, true)]
            [TestCase(1, 0, true)]
            [TestCase(1, 1, true)]
            [TestCase(2, 2, true)]
            public void ReturnsCorrectMoveLegality(int xCheck, int yCheck, bool expected)
            {
                Vector2Int check = new Vector2Int(xCheck, yCheck);

                Move move = new Move(rook, check);
                bool actual = move.IsLegal();
                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void ReturnsCorrectNumberOfPossibleMoves()
            {
                List<Move> moves = rook.GetPossibleMoves();
                Assert.IsTrue(moves.Count == 8);
            }
        }
    
        class Scenario2
        {
            Rook rook;
            Pawn pawn1;
            Pawn pawn2;
            Pawn pawn3;
            ChessBoard board;

            public Scenario2()
            {
                rook = new Rook(ChessPieceColor.Black, new Vector2Int(5, 6));
                pawn1 = new Pawn(ChessPieceColor.White, new Vector2Int(2, 6));
                pawn2 = new Pawn(ChessPieceColor.White, new Vector2Int(3, 4));
                pawn3 = new Pawn(ChessPieceColor.Black, new Vector2Int(5, 1));

                board = new ChessBoard(pawn1, pawn2, pawn3, rook);
                board.PlayerTurn = ChessPieceColor.Black;
            }

            [TestCase(1, 6, false)]
            [TestCase(0, 6, false)]
            [TestCase(3, 4, false)]
            [TestCase(8, 6, false)]
            [TestCase(5, 6, false)]
            [TestCase(5, 0, false)]
            [TestCase(5, 1, false)]
            [TestCase(2, 6, true)]
            [TestCase(5, 2, true)]
            [TestCase(7, 6, true)]
            [TestCase(5, 7, true)]
            [TestCase(6, 6, true)]
            [TestCase(5, 4, true)]
            public void ReturnsCorrectMoveLegality(int xCheck, int yCheck, bool expected)
            {
                Vector2Int check = new Vector2Int(xCheck, yCheck);

                Move move = new Move(rook, check);
                bool actual = move.IsLegal();
                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void ReturnsCorrectNumberOfPossibleMoves()
            {
                List<Move> moves = rook.GetPossibleMoves();
                Assert.IsTrue(moves.Count == 10);
            }
        }
    }
}