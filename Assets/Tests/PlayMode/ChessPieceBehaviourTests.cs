using System.Collections;
using Chess;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode
{
    public class ChessPieceBehaviourTests
    {
        GameObject chessBoardGameObject;
        ChessPieceBehaviour chessPieceBehaviour;

        public ChessPieceBehaviourTests()
        {
            GameObject chessBoardGameObject = new GameObject();
            chessBoardGameObject.AddComponent<ChessBoardBehaviour>();
            chessPieceBehaviour = Object.FindObjectOfType<ChessPieceBehaviour>();
        }

        public static Vector2Int RoundedPosition(Transform t)
        {
            Vector3 position = t.position;
            Vector2Int roundedPosition = new Vector2Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
            return roundedPosition;
        }

        [UnityTest]
        public IEnumerator NewObjectHasUnderlyingData()
        {
            Assert.IsNotNull(chessPieceBehaviour.ChessPiece);
            yield return null;
        }

        [UnityTest]
        public IEnumerator NewObjectSetsFirstPosition()
        {
            Vector2Int chessPiecePosition = chessPieceBehaviour.ChessPiece.Position;
            Vector2Int roundedPosition = RoundedPosition(chessPieceBehaviour.transform);

            Assert.IsTrue(roundedPosition == chessPiecePosition);
        
            yield return null;
        }

        [UnityTest]
        public IEnumerator NewObjectSetsSprite()
        {
            SpriteRenderer spriteRenderer = chessPieceBehaviour.GetComponentInChildren<SpriteRenderer>();

            Assert.NotNull(spriteRenderer);
            Assert.NotNull(spriteRenderer.sprite);

            yield return null;
        }
    
        [UnityTest]
        public IEnumerator PieceMovesAfterChangeInUnderlyingData()
        {
            Vector2Int expected = new Vector2Int(4, 4);
            chessPieceBehaviour.ChessPiece.MoveTo(expected);
            yield return new WaitForSeconds(3);
        
            Vector2Int roundedPosition = RoundedPosition(chessPieceBehaviour.transform);

            Assert.IsTrue(roundedPosition == expected);

            yield return null;
        }

        [UnityTest]
        public IEnumerator AllPiecesHaveASprite()
        {
            foreach (ChessPieceBehaviour cpb in Object.FindObjectsOfType<ChessPieceBehaviour>())
            {
                SpriteRenderer spriteRenderer = cpb.GetComponentInChildren<SpriteRenderer>();
                Assert.NotNull(spriteRenderer);
                Assert.NotNull(spriteRenderer.sprite);
            }

            yield return null;
        }

        [UnityTest]
        public IEnumerator CanFindPieceByPosition()
        {
            ChessPieceBehaviour piece = ChessPieceBehaviour.FindBy(new Vector2Int(0, 0));
            Assert.IsTrue(piece.ChessPiece is Rook);

            yield return null;
        }
    }
}