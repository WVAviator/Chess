using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Chess
{
    public class Square : MonoBehaviour
    {
        public Vector2Int Position { get; private set; }

        void Awake() => Position = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        
        public void Click()
        {
            ChessPieceInteraction piece = GetPiece(Position);
            if (piece != null) piece.Click();
        }

        public void Drag(Vector3 toPosition)
        {
            ChessPieceInteraction piece = GetPiece(Position);
            if (piece != null) piece.Drag(toPosition);
        }

        public void Release(Collider2D releasedOn)
        {
            ChessPieceInteraction piece = GetPiece(Position);
            if (piece == null) return;

            if (releasedOn == null)
            {
                piece.Release(Position);
                return;
            }
            
            if (!releasedOn.TryGetComponent(out Square releasedSquare)) piece.Release(Position);

            piece.Release(releasedSquare.Position);
        }

        static ChessPieceInteraction GetPiece(Vector2Int position)
        {
            ChessPieceBehaviour pieceInSquare = ChessPieceBehaviour.FindBy(position);
            if (pieceInSquare == null) return null;
            ChessPieceInteraction pieceInteraction = pieceInSquare.GetComponent<ChessPieceInteraction>();
            return pieceInteraction;
        }
        
    }
}
