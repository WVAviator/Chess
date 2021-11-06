using System.Collections;
using System.Collections.Generic;
using Chess;
using UnityEngine;

public class PawnPromotionPopup : MonoBehaviour
{

    [SerializeField] RectTransform _blackPanel;
    [SerializeField] RectTransform _whitePanel;

    Move _move;
    ChessPieceColor _color;

    Camera mainCamera;
    
    void Awake() => mainCamera = Camera.main;

    public void Show(Move move)
    {
        _move = move;
        _color = move.ChessPiece.Color;
        
        if (_color == ChessPieceColor.Black)
        {
            _blackPanel.gameObject.SetActive(true);
            _blackPanel.position = GetPanelPosition(_move.ChessPiece.Position);
        }
        else
        {
            _whitePanel.gameObject.SetActive(true);
            _whitePanel.position = GetPanelPosition(_move.ChessPiece.Position);
        }
    }

    Vector2 GetPanelPosition(Vector2Int chessPiecePosition)
    {
        Vector2 panelPosition = mainCamera.WorldToScreenPoint((Vector2)chessPiecePosition);
        //panelPosition.y += Screen.height * 0.5f;
        return panelPosition;
    }

    public void SelectBishop()
    {
        _blackPanel.gameObject.SetActive(false);
        _whitePanel.gameObject.SetActive(false);
        _move.PromotionPiece = new Bishop(_color);
    }
    
    public void SelectKnight()
    {
        _blackPanel.gameObject.SetActive(false);
        _whitePanel.gameObject.SetActive(false);
        _move.PromotionPiece = new Knight(_color);
    }
    
    public void SelectQueen()
    {
        _blackPanel.gameObject.SetActive(false);
        _whitePanel.gameObject.SetActive(false);
        _move.PromotionPiece = new Queen(_color);
    }
    
    public void SelectRook()
    {
        _blackPanel.gameObject.SetActive(false);
        _whitePanel.gameObject.SetActive(false);
        _move.PromotionPiece = new Rook(_color);
    }

}
