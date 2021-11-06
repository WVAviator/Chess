using Chess;
using NUnit.Framework;

namespace Tests.EditMode
{
    public class RandomAITests
    {
        [Test]
        public void BlackAIWaitsForTurn()
        {
            Setup.Board.Standard()
                .WithRandomAI(ChessPieceColor.Black)
                .Move.From(2, 1).To(2, 3).Execute()
                .Get(out var board);

            Assert.IsTrue(board.PlayerTurn == ChessPieceColor.White);
        }
    }
}