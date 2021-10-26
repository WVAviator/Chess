using System;
using System.Collections;
using System.Collections.Generic;
using Chess;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

public class ChessBoardBehaviourTests
{
    GameObject chessBoardGameObject;

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
}