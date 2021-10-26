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
    [UnityTest]
    public IEnumerator AwakeSetsUpNewDefaultBoard()
    {
        GameObject chessBoardGameObject = new GameObject();
        chessBoardGameObject.AddComponent<ChessBoardBehaviour>();
        yield return null;

        ChessPieceBehaviour[] pieces = Object.FindObjectsOfType<ChessPieceBehaviour>();
        Assert.IsTrue(pieces.Length == 32);
    }
}