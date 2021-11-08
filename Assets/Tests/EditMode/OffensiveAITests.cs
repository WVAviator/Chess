using Chess;
using NUnit.Framework;

namespace Tests.EditMode
{
    public class OffensiveAITests
    {
        [Test]
        public void AIPicksBestMove()
        {
            Setup.Board
                .WithOffensiveAI(ChessPieceColor.Black)
                .Place.Black<Queen>().At(4, 4)
                .Place.White<Pawn>().At(6, 6)
                .Place.White<Bishop>().At(2, 6)
                .Place.White<Queen>().At(2, 1).AndGet(out var queen)
                .Move.From(2, 1).To(2, 2).Execute()
                .Get(out var board);
            
            Assert.IsFalse(board.Contains(queen));
        }
    }
}