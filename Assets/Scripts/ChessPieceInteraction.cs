using UnityEngine;

namespace Chess
{
    public class ChessPieceInteraction : MonoBehaviour
    {
        ChessPieceBehaviour _chessPieceBehaviour;
        Vector3 _targetPosition;
        bool _isDragging;

        [SerializeField] float _moveSpeed = 10;

        void Awake() => _chessPieceBehaviour = GetComponent<ChessPieceBehaviour>();
        void Start()
        {
            _chessPieceBehaviour.ChessPiece.OnPieceMoved += UpdateTargetPosition;
            UpdateTargetPosition(_chessPieceBehaviour.ChessPiece.Position);
            transform.position = _targetPosition;
        }
        
        void OnDisable() => _chessPieceBehaviour.ChessPiece.OnPieceMoved -= UpdateTargetPosition;

        void Update()
        {
            if (transform.position == _targetPosition || _isDragging) return;
            AnimateMovement();
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
            _isDragging = true;
            DragPieceTo(newPosition);
        }

        public void Release(Vector2Int requestedPosition)
        {
            if (!_chessPieceBehaviour.ChessPiece.IsMyTurn()) return;
            _isDragging = false;

            Move move = new Move(_chessPieceBehaviour.ChessPiece, requestedPosition);
            move.Execute();
        }

        public void Click()
        {
            if (!_chessPieceBehaviour.ChessPiece.IsMyTurn()) return;
        }
        
        
    }
}