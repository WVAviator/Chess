using System;
using UnityEngine;

namespace Chess
{
    public class ChessBoardBehaviour : MonoBehaviour
    {
        public ChessBoard ChessBoard => _chessBoard;
        ChessBoard _chessBoard;

        ChessPieceBehaviour _chessPieceBehaviourPrefab;

        void Awake()
        {
            _chessBoard = new ChessBoard();
            _chessBoard.SetupStandard();
            _chessPieceBehaviourPrefab = Resources.Load<ChessPieceBehaviour>("Prefabs/ChessPiece");

            foreach (ChessPiece piece in _chessBoard.ChessPieces)
            {
                ChessPieceBehaviour chessPieceBehaviour = Instantiate(_chessPieceBehaviourPrefab, transform);
                chessPieceBehaviour.Initialize(piece);
            }
        }
    }
}