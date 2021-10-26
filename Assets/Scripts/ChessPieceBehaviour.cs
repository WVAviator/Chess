using System;
using UnityEngine;

namespace Chess
{
    public class ChessPieceBehaviour : MonoBehaviour
    {
        public ChessPiece ChessPiece => _chessPiece;
        ChessPiece _chessPiece;

        SpriteRenderer _spriteRenderer;

        void Awake()
        {
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        public void Initialize(ChessPiece piece)
        {
            _chessPiece = piece;
            _chessPiece.OnPieceMoved += SetPosition;
            SetPosition(piece.Position);
            SetSprite();
        }

        void SetSprite()
        {
            _spriteRenderer.sprite = GetSpriteFromFile();
        }

        Sprite GetSpriteFromFile()
        {
            string path = GetSpriteFilePath();
            return Resources.Load<Sprite>(path);
        }

        string GetSpriteFilePath()
        {
            string basePath = "Sprites/Pieces";
            string colorPath = _chessPiece.Color == ChessPieceColor.Black ? "Black" : "White";
            string piecePath = _chessPiece.PieceName;
            
            return String.Join("/", basePath, colorPath, piecePath);
        }

        void SetPosition(Vector2Int position)
        {
            transform.position = new Vector3(position.x, position.y, -0.05f);
        }

        void OnDisable() => _chessPiece.OnPieceMoved -= SetPosition;
    }
}
