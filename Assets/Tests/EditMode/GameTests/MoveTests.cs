using System.Text.RegularExpressions;
using Chess;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.EditMode
{
    public class MoveTests
    {
        class Execute
        {
            [Test]
            public void LegalMoveExecutesSuccessfully()
            {
                Setup.Board
                    .BlackGoesFirst
                    .Place.Black<Pawn>().At(6, 6).AndGet(out var pawn)
                    .Move.From(6, 6).To(6, 4).Execute();
                
                
                Vector2Int expected = new Vector2Int(6, 4);
                Assert.AreEqual(expected, pawn.Position);
            }

            [Test]
            public void IllegalMoveFailsToExecute()
            {
                Setup.Board
                    .Place.Black<Pawn>().At(6, 6).AndGet(out var pawn)
                    .Move.From(6, 6).To(6, 3).Execute();
                
                LogAssert.Expect(LogType.Error, new Regex(@".*"));

                Vector2Int expected = new Vector2Int(6, 6);
                Assert.AreEqual(expected, pawn.Position);
            }

        

            [Test]
            public void TakingOpponentPieceRemovesPieceFromBoard()
            {
                Setup.Board
                    .Place.White<Pawn>().At(3, 3)
                    .Place.Black<Pawn>().At(4, 4).AndGet(out var pawn)
                    .Move.From(3, 3).To(4, 4).Execute()
                    .Get(out var board);
                
                Assert.IsFalse(board.Contains(pawn));
            }
        }

        class Undo
        {
            [Test]
            public void SuccessfulUndoAfterMoveExecuted()
            {
                Setup.Board
                    .Place.Black<Pawn>().At(6, 6).AndGet(out var pawn)
                    .BlackGoesFirst
                    .Move.From(6, 6).To(6, 4).Execute()
                    .ThenUndo();

                Vector2Int expected = new Vector2Int(6, 6);
                Assert.IsTrue(pawn.Position == expected);
            }
            
            [Test]
            public void UndoBeforeExecuteDoesNothing()
            {
                Setup.Board
                    .Place.Black<Pawn>().At(6, 6).AndGet(out var pawn)
                    .BlackGoesFirst
                    .Move.From(6, 6).To(6, 4)
                    .ThenUndo();

                Vector2Int expected = new Vector2Int(6, 6);
                Assert.IsTrue(pawn.Position == expected);
            }
            
            [Test]
            public void UndoRestoresOpponentPieceToBoard()
            {
                Setup.Board
                    .Place.White<Pawn>().At(3, 3)
                    .Place.Black<Pawn>().At(4, 4).AndGet(out var pawn)
                    .Move.From(3, 3).To(4, 4).Execute()
                    .ThenUndo()
                    .Get(out var board);
                
                Assert.IsTrue(board.Contains(pawn));
            }
        }

        class Check
        {
            [Test]
            public void KingCannotSacrificeSelf()
            {
                Setup.Board
                    .Place.Black<King>().At(4, 7).AndGet(out var king)
                    .Place.White<Queen>().At(0, 6)
                    .Place.White<Bishop>().At(2, 4)
                    .Move.From(0, 6).To(4, 6).Execute();
                
                Assert.IsTrue(king.GetPossibleMoves().Count == 0);
            }
        }
    }
}