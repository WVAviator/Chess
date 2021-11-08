using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Chess
{
    public class ChessPieceInteraction : MonoBehaviour
    {
        ChessPieceBehaviour _chessPieceBehaviour;
        Vector3 _targetPosition;
        bool _isDragging;
        
        ChessBoardBehaviour _chessBoardBehaviour;

        [SerializeField] float _moveSpeed = 10;

        HashSet<Move> _legalMoves;

        void Awake()
        {
            _chessPieceBehaviour = GetComponent<ChessPieceBehaviour>();
            _chessBoardBehaviour = FindObjectOfType<ChessBoardBehaviour>();
        }

        void Start()
        {
            _chessPieceBehaviour.ChessPiece.OnPieceMoved += UpdateTargetPosition;
            UpdateTargetPosition(_chessPieceBehaviour.ChessPiece.Position);
            transform.position = _targetPosition;
        }
        
        void OnEnable()
        {
            if (_chessPieceBehaviour.ChessPiece == null) return;
            _chessPieceBehaviour.ChessPiece.OnPieceMoved += UpdateTargetPosition;
        }

        void OnDisable() => _chessPieceBehaviour.ChessPiece.OnPieceMoved -= UpdateTargetPosition;

        void Update()
        {
            if (transform.position == _targetPosition || _isDragging) return;
            AnimateMovement();
        }

        void ShowAvailableMoves()
        {
            _legalMoves = _chessPieceBehaviour.ChessPiece.GetPossibleMoves();
            foreach (Square s in _chessBoardBehaviour.Squares) s.ManageHighlighting(_legalMoves);
        }

        void HideAvailableMoves()
        {
            _legalMoves = new HashSet<Move>();
            foreach (Square s in _chessBoardBehaviour.Squares) s.ManageHighlighting(_legalMoves);
        }

        void AnimateMovement()
        {
            transform.position =
                Vector3.MoveTowards(transform.position, _targetPosition, _moveSpeed * Time.deltaTime);

            if (IsCloseEnough()) transform.position = _targetPosition;
        }
        
        bool IsCloseEnough() => (transform.position - _targetPosition).sqrMagnitude < 0.001f;

        void UpdateTargetPosition(Vector2Int position)
        {
            _targetPosition = new Vector3(position.x, position.y, -0.05f);
        }
        
        

        void DragPieceTo(Vector3 position)
        {
            if (!_chessPieceBehaviour.ChessPiece.IsMyTurn()) return;
            Vector3 dragPosition = new Vector3(position.x, position.y, -0.05f);
            transform.position = dragPosition;
        }

        public void Drag(Vector3 newPosition)
        {
            if (!_chessPieceBehaviour.ChessPiece.IsMyTurn()) return;

            if (!_isDragging) ShowAvailableMoves();
            _isDragging = true;
            DragPieceTo(newPosition);
        }

        public void Release(Vector2Int requestedPosition)
        {
            _isDragging = false;
            HideAvailableMoves();

            if (!_chessPieceBehaviour.ChessPiece.IsMyTurn()) return;
            

            Move move = new Move(_chessPieceBehaviour.ChessPiece, requestedPosition);
            move.Execute();
            
            if (move.IsPromotion)
            {
                FindObjectOfType<PawnPromotionPopup>().Show(move);
            }
        }

        public void Click()
        {
            if (!_chessPieceBehaviour.ChessPiece.IsMyTurn()) return;
        }
        
        
    }
}