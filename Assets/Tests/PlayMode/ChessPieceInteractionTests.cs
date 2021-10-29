using System;
using System.Collections;
using System.Text.RegularExpressions;
using Chess;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace Tests.PlayMode
{
    public class ChessPieceInteractionTests
    {
        GameObject _chessBoardGameObject;
        ChessBoardBehaviour _chessBoardBehaviour;
        ChessPieceInteraction _chessPieceInteraction;
        ChessPieceBehaviour _chessPieceBehaviour;
        public ChessPieceInteractionTests()
        {
            
            Setup();
        }

        void Setup()
        {
            if (_chessBoardGameObject != null) Object.Destroy(_chessBoardGameObject);
            _chessBoardGameObject = new GameObject();
            _chessBoardBehaviour = _chessBoardGameObject.AddComponent<ChessBoardBehaviour>();
            _chessPieceBehaviour = ChessPieceBehaviour.FindBy(new Vector2Int(1, 1));
            _chessPieceInteraction = _chessPieceBehaviour.GetComponent<ChessPieceInteraction>();
        }

        [UnityTest]
        public IEnumerator DragToInvalidPositionResetsPieceToOriginalPosition()
        {
            yield return new WaitForSeconds(1);

            Vector3 startPosition = _chessPieceInteraction.transform.position;
            Vector3 newPosition = new Vector3(-1, -1, 0);

            _chessPieceInteraction.Drag(newPosition);
            yield return null;

            _chessPieceInteraction.Release(new Vector2Int(-1, -1));
            LogAssert.Expect(LogType.Error, new Regex(@".*"));
            yield return null;
            
            

            Assert.IsFalse(_chessPieceInteraction.transform.position == newPosition);
            Assert.IsFalse(_chessPieceInteraction.transform.position == startPosition);
            yield return new WaitForSeconds(2);

            Assert.IsTrue(_chessPieceInteraction.transform.position == startPosition);
        }

        [UnityTest]
        public IEnumerator DragToValidSquareInitiatesMove()
        {
            Setup();
            
            Vector3 newPosition = new Vector3(1.05f, 2.04f, 0);
            Vector2Int newPositionInt = new Vector2Int(1, 2);
            
            _chessPieceInteraction.Drag(newPosition);
            yield return null;
            _chessPieceInteraction.Release(newPositionInt);
            yield return null;
            Assert.IsTrue(_chessPieceBehaviour.ChessPiece.Position == newPositionInt);
        }

        [UnityTest]
        public IEnumerator DragToOccupiedSquareTakesPiece()
        {
            Setup();
            
            ChessPieceBehaviour opponentBehaviour = ChessPieceBehaviour.FindBy(new Vector2Int(2, 6));
            ChessPieceInteraction opponentInteraction = opponentBehaviour.GetComponent<ChessPieceInteraction>();
            
            _chessPieceInteraction.Drag(new Vector3(1.05f, 3.1f, 0));
            yield return null;
            _chessPieceInteraction.Release(new Vector2Int(1, 3));
            yield return null;
            
            opponentInteraction.Drag(new Vector3(2.05f, 4.1f, 0));
            yield return null;
            opponentInteraction.Release(new Vector2Int(2, 4));
            yield return null;
            
            _chessPieceInteraction.Drag(new Vector3(2.05f, 4.1f, 0));
            yield return null;
            _chessPieceInteraction.Release(new Vector2Int(2, 4));
            yield return null;
            
            Assert.IsFalse(opponentInteraction.gameObject.activeSelf);
        }

        [UnityTest]
        public IEnumerator UndoRestoresRemovedPiece()
        {
            Setup();
            
            ChessPieceBehaviour opponentBehaviour = ChessPieceBehaviour.FindBy(new Vector2Int(2, 6));
            ChessPieceInteraction opponentInteraction = opponentBehaviour.GetComponent<ChessPieceInteraction>();
            
            _chessPieceInteraction.Drag(new Vector3(1.05f, 3.1f, 0));
            yield return null;
            _chessPieceInteraction.Release(new Vector2Int(1, 3));
            yield return null;
            
            opponentInteraction.Drag(new Vector3(2.05f, 4.1f, 0));
            yield return null;
            opponentInteraction.Release(new Vector2Int(2, 4));
            yield return null;
            
            _chessPieceInteraction.Drag(new Vector3(2.05f, 4.1f, 0));
            yield return null;
            _chessPieceInteraction.Release(new Vector2Int(2, 4));
            yield return null;

            _chessPieceBehaviour.ChessPiece.Board.MostRecentMove().Undo();
            yield return null;
            
            Assert.IsTrue(opponentInteraction.gameObject.activeSelf);
        }

    }
}