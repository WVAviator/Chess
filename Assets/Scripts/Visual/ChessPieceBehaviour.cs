using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chess
{
    public class ChessPieceBehaviour : MonoBehaviour
    {
        public ChessPiece ChessPiece => _chessPiece;
        ChessPiece _chessPiece;

        SpriteRenderer _spriteRenderer;

        static List<ChessPieceBehaviour> AllPieces = new List<ChessPieceBehaviour>();

        void Awake() => _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        void OnEnable() => AllPieces.Add(this);

        void OnDisable()
        {
            AllPieces.Remove(this);
        }

        public static ChessPieceBehaviour FindBy(Vector2Int position) =>
            AllPieces.FirstOrDefault(p => p.ChessPiece.Position == position);
        
        public void Initialize(ChessPiece piece)
        {
            _chessPiece = piece;
            SetSprite();
        }

        void SetSprite() => _spriteRenderer.sprite = GetSpriteFromFile();

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
    }
}
