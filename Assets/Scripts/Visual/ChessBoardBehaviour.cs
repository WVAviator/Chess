using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public class ChessBoardBehaviour : MonoBehaviour
    {
        public ChessBoard ChessBoard => _chessBoard;
        ChessBoard _chessBoard;

        ChessPieceBehaviour _chessPieceBehaviourPrefab;
        Square _squarePrefab;

        Dictionary<ChessPiece, ChessPieceBehaviour> _chessPieceDictionary;

        public List<Square> Squares = new List<Square>();

        void Awake()
        {
            _chessBoard = new ChessBoard();
            

            _chessBoard.OnPieceAdded += ActivatePiece;
            _chessBoard.OnPieceRemoved += DeactivatePiece;
            
            _chessPieceBehaviourPrefab = Resources.Load<ChessPieceBehaviour>("Prefabs/ChessPiece");
            _squarePrefab = Resources.Load<Square>("Prefabs/Square");
            
            _chessPieceDictionary = new Dictionary<ChessPiece, ChessPieceBehaviour>();
            
            _chessBoard.SetupStandard();

            SetupSquares();
        }

        void SetupSquares()
        {
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    Squares.Add(Instantiate(_squarePrefab, new Vector2(x, y), Quaternion.identity, transform));
                }
            }
        }

        void DeactivatePiece(ChessPiece piece)
        {
            _chessPieceDictionary[piece].gameObject.SetActive(false);
        }

        void ActivatePiece(ChessPiece piece)
        {
            if (_chessPieceDictionary.ContainsKey(piece))
            {
                _chessPieceDictionary[piece].gameObject.SetActive(true);
            }
            else 
            {
                ChessPieceBehaviour chessPieceBehaviour = Instantiate(_chessPieceBehaviourPrefab, transform);
                chessPieceBehaviour.Initialize(piece);
                _chessPieceDictionary[piece] = chessPieceBehaviour;
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) _chessBoard.MostRecentMove().Undo();
        }

    }
}