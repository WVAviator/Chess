using System.Collections;
using Chess;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace Tests.PlayMode
{
    public class ChessBoardBehaviourTests
    {
        public ChessBoardBehaviourTests()
        {
            GameObject chessBoardGameObject = new GameObject();
            chessBoardGameObject.AddComponent<ChessBoardBehaviour>();
        }
    
        [UnityTest]
        public IEnumerator CorrectNumberOfPiecesGeneratedByNewBoard()
        {
            ChessPieceBehaviour[] pieces = Object.FindObjectsOfType<ChessPieceBehaviour>();
            Assert.IsTrue(pieces.Length == 32);

            yield return null;
        }

        [UnityTest]
        public IEnumerator CorrectNumberOfSquareGeneratedByNewBoard()
        {
            Square[] squares = Object.FindObjectsOfType<Square>();
            Assert.IsTrue(squares.Length == 64);

            yield return null;
        }

    }
}