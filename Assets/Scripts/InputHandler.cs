using UnityEngine;

namespace Chess
{
    public class InputHandler : MonoBehaviour
    {
        [SerializeField] float minimumDragDistance = 10;
        bool _inputAllowed = true;

        Camera _mainCamera;
        bool _isDragging;
        Vector2 _currentMouseWorldPosition;

        MouseDown _mouseDown;
        
        void Awake() => _mainCamera = Camera.main;
        void Update()
        {
            if (!_inputAllowed) return;
            
            SetCurrentMouseWorldPosition();

            if (Input.GetMouseButtonDown(0)) _mouseDown = new MouseDown(_currentMouseWorldPosition);
            
            if (Input.GetMouseButtonUp(0))
            {
                if (!_isDragging) ProcessClick();
                else ProcessRelease();
                _isDragging = false;
            }

            if (Input.GetMouseButton(0)) _isDragging = MovedEnoughToBeConsideredDragging();
            if (_isDragging) ProcessDrag();
        }

        bool MovedEnoughToBeConsideredDragging() =>
            ((Vector2)Input.mousePosition - _mouseDown.ClickedScreenPosition).sqrMagnitude >
            (minimumDragDistance * minimumDragDistance);

        void SetCurrentMouseWorldPosition() => 
            _currentMouseWorldPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        

        void ProcessClick() => _mouseDown.Click(_currentMouseWorldPosition);
        void ProcessDrag() => _mouseDown.DragTo(_currentMouseWorldPosition);
        void ProcessRelease() => _mouseDown.Release(_currentMouseWorldPosition);
    }
}