using UnityEngine;

namespace Chess
{
    public class MouseDown
    {
        public Vector2 ClickedScreenPosition { get; }
        readonly Collider2D _collider;

        public MouseDown(Vector2 currentMousePosition)
        {
            ClickedScreenPosition = Input.mousePosition;
            _collider = GetCollider(currentMousePosition, 1);
        }

        public void DragTo(Vector2 currentMousePosition)
        {
            if (_collider == null) return;
            if (_collider.TryGetComponent(out Square draggingSquare)) draggingSquare.Drag(currentMousePosition);
        }

        public void Release(Vector2 currentMousePosition)
        {
            if (_collider == null) return;
            Collider2D releasedCollider = GetCollider(currentMousePosition, 1);
            if (_collider.TryGetComponent(out Square draggedSquare)) draggedSquare.Release(releasedCollider);
        }

        public void Click(Vector2 currentMousePosition)
        {
            if (_collider == null) return;
            if (_collider.TryGetComponent(out Square clickedSquare)) clickedSquare.Click();
        }

        Collider2D GetCollider(Vector3 position, int layer) => Physics2D.OverlapPoint(position, layer);

    }
}