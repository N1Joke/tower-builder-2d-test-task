using Tools.Extensions;
using UnityEngine;

namespace Assets._Project.Scripts.Tools
{
    [RequireComponent(typeof(Collider2D))]
    public class SceneDragHandler : MonoBehaviour
    {
        private Camera _camera;
        private bool _isDragging = false;
        private Vector3 _offset;
        private bool _canDrag = true;

        public ReactiveEvent OnEndDrag { get; private set; } = new();
        
        private  void Awake()
        {
            _camera = Camera.main;            
        }

        public void ToggleDrag(bool enable) => _canDrag = enable;

        private void Update()
        {
            if (!_canDrag)
                return;

            if (!Application.isMobilePlatform)
            {
                if (Input.GetMouseButtonDown(0))
                    TryStartDrag(Input.mousePosition);

                if (Input.GetMouseButton(0) && _isDragging)
                    Drag(Input.mousePosition);

                if (Input.GetMouseButtonUp(0) && _isDragging)
                    EndDrag();
            }
            else
            {
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);
                    Vector2 touchPos = touch.position;

                    if (touch.phase == TouchPhase.Began)
                        TryStartDrag(touchPos);
                    else if ((touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary) && _isDragging)
                        Drag(touchPos);
                    else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                        EndDrag();
                }
            }
        }

        private void TryStartDrag(Vector2 screenPos)
        {
            Vector3 worldPos = _camera.ScreenToWorldPoint(screenPos);
            Vector2 point = new Vector2(worldPos.x, worldPos.y);

            Collider2D hit = Physics2D.OverlapPoint(point);
            if (hit != null && hit.gameObject == gameObject)
            {
                _isDragging = true;
                _offset = transform.position - worldPos;
                _offset.z = 0f;
            }
        }

        private void Drag(Vector2 screenPos)
        {
            Vector3 worldPos = _camera.ScreenToWorldPoint(screenPos);
            worldPos.z = 0f;
            transform.position = worldPos + _offset;
        }

        private void EndDrag()
        {
            _isDragging = false;
            OnEndDrag?.Notify();
        }
    }
}