using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using Chess;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ChessPieceTests
{

    class Color
    {
        [TestCase(ChessPieceColor.White)]
        [TestCase(ChessPieceColor.Black)]
        public void Property_PawnReturnsCorrectColor(ChessPieceColor color)
        {
            Pawn pawn = new Pawn(color);
            ChessPieceColor actual = pawn.Color;
            Assert.AreEqual(actual, color);
        }

        [TestCase(ChessPieceColor.White)]
        [TestCase(ChessPieceColor.Black)]
        public void Property_RookReturnsCorrectColor(ChessPieceColor color)
        {
            Rook rook = new Rook(color);
            ChessPieceColor actual = rook.Color;
            Assert.AreEqual(actual, color);
        }

        [TestCase(ChessPieceColor.White)]
        [TestCase(ChessPieceColor.Black)]
        public void Property_BishopReturnsCorrectColor(ChessPieceColor color)
        {
            Bishop bishop = new Bishop(color);
            ChessPieceColor actual = bishop.Color;
            Assert.AreEqual(actual, color);
        }

        [TestCase(ChessPieceColor.White)]
        [TestCase(ChessPieceColor.Black)]
        public void Property_KnightReturnsCorrectColor(ChessPieceColor color)
        {
            Knight knight = new Knight(color);
            ChessPieceColor actual = knight.Color;
            Assert.AreEqual(actual, color);
        }

        [TestCase(ChessPieceColor.White)]
        [TestCase(ChessPieceColor.Black)]
        public void Property_QueenReturnsCorrectColor(ChessPieceColor color)
        {
            Queen queen = new Queen(color);
            ChessPieceColor actual = queen.Color;
            Assert.AreEqual(actual, color);
        }

        [TestCase(ChessPieceColor.White)]
        [TestCase(ChessPieceColor.Black)]
        public void Property_KingReturnsCorrectColor(ChessPieceColor color)
        {
            King king = new King(color);
            ChessPieceColor actual = king.Color;
            Assert.AreEqual(actual, color);
        }
    }

    class Position
    {
        
        
        [TestCase(0, 0)]
        [TestCase(1, 4)]
        [TestCase(6, 6)]
        public void Property_PawnReturnsCorrectPosition(int x, int y)
        {
            Vector2Int expected = new Vector2Int(x, y);
            Pawn pawn = new Pawn(ChessPieceColor.White, expected);
            Vector2Int actual = pawn.Position;
            Assert.AreEqual(actual, expected);
        }
        
        [TestCase(0, 0)]
        [TestCase(1, 4)]
        [TestCase(6, 6)]
        public void Property_RookReturnsCorrectPosition(int x, int y)
        {
            Vector2Int expected = new Vector2Int(x, y);
            Rook rook = new Rook(ChessPieceColor.White, expected);
            Vector2Int actual = rook.Position;
            Assert.AreEqual(actual, expected);
        }
        
        [TestCase(0, 0)]
        [TestCase(1, 4)]
        [TestCase(6, 6)]
        public void Property_KnightReturnsCorrectPosition(int x, int y)
        {
            Vector2Int expected = new Vector2Int(x, y);
            Knight knight = new Knight(ChessPieceColor.White, expected);
            Vector2Int actual = knight.Position;
            Assert.AreEqual(actual, expected);
        }
        
        [TestCase(0, 0)]
        [TestCase(1, 4)]
        [TestCase(6, 6)]
        public void Property_BishopReturnsCorrectPosition(int x, int y)
        {
            Vector2Int expected = new Vector2Int(x, y);
            Bishop bishop = new Bishop(ChessPieceColor.White, expected);
            Vector2Int actual = bishop.Position;
            Assert.AreEqual(actual, expected);
        }
        
        [TestCase(0, 0)]
        [TestCase(1, 4)]
        [TestCase(6, 6)]
        public void Property_QueenReturnsCorrectPosition(int x, int y)
        {
            Vector2Int expected = new Vector2Int(x, y);
            Queen queen = new Queen(ChessPieceColor.White, expected);
            Vector2Int actual = queen.Position;
            Assert.AreEqual(actual, expected);
        }
        
        [TestCase(0, 0)]
        [TestCase(1, 4)]
        [TestCase(6, 6)]
        public void Property_KingReturnsCorrectPosition(int x, int y)
        {
            Vector2Int expected = new Vector2Int(x, y);
            King king = new King(ChessPieceColor.White, expected);
            Vector2Int actual = king.Position;
            Assert.AreEqual(actual, expected);
        }
        
        [TestCase(-1, 0)]
        [TestCase(-6, 44)]
        [TestCase(6, 8)]
        public void InvalidProperty_KingReturnsDefaultPosition(int x, int y)
        {
            Vector2Int newVector = new Vector2Int(x, y);
            King king = new King(ChessPieceColor.White, newVector);
            Vector2Int actual = king.Position;
            Assert.AreEqual(Vector2Int.zero, actual);
        }
    }

    class Move
    {
        [TestCase(0, 0, 1, 1)]
        [TestCase(1, 2, 4, 4)]
        public void SetPosition_PieceMovedToNewPosition(int x, int y, int newX, int newY)
        {
            Vector2Int start = new Vector2Int(x, y);
            Vector2Int expected = new Vector2Int(newX, newY);

            Pawn pawn = new Pawn(ChessPieceColor.Black, start);
            pawn.MoveTo(expected);
            Assert.AreEqual(pawn.Position, expected);
        }

        [TestCase(8, 9)]
        [TestCase(0, -1)]
        public void SetPosition_InvalidPosition(int x, int y)
        {
            Vector2Int start = Vector2Int.zero;
            Vector2Int illegalPosition = new Vector2Int(x, y);
            
            Pawn pawn = new Pawn(ChessPieceColor.Black, start);
            pawn.MoveTo(illegalPosition);
            LogAssert.Expect(LogType.Error, new Regex(@".*"));
            Assert.AreEqual(pawn.Position, start);
        }
    }
}