using UnityEngine.EventSystems;
using UnityEngine;
using Tools.Extensions;
using Assets._Project.Scripts.Gameplay;
using System.ComponentModel;

namespace Assets._Project.Scripts.Tools
{
    public class DragHandler : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        private Vector2 _pointerStartPos;
        private Vector2 _pointerPos;
        private RectTransform _rectTransform;
        private Canvas _canvas;
        private RectTransform _tempItemRect;
        private GameObject _scrollRectObj;
        private float _offsetYTouchToSpawnCard = 20;
        private bool _startedDrag;
        private bool _inited;

        public ReactiveEvent<RectTransform> CallBack { get; private set; } = new();

        public void Init(Canvas canvas, GameObject scrollRectObj)
        {
            _inited = true;
            _scrollRectObj = scrollRectObj;
            _canvas = canvas;
        }

        protected void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!_inited)
                return;

            _pointerStartPos = eventData.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_inited)
                return;

            _pointerPos = eventData.position;

            if (_tempItemRect != null)
                _tempItemRect.anchoredPosition += eventData.delta / _canvas.scaleFactor;
            else if (_pointerPos.y - _pointerStartPos.y > _offsetYTouchToSpawnCard)
                ProcessActionOnCardMoveDelta();
            else
            {
                if (!_startedDrag)
                {
                    ExecuteEvents.Execute(_scrollRectObj, eventData, ExecuteEvents.beginDragHandler);
                    _startedDrag = true;
                }
                else
                    ExecuteEvents.Execute(_scrollRectObj, eventData, ExecuteEvents.dragHandler);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!_inited)
                return;

            ExecuteEvents.Execute(_scrollRectObj, eventData, ExecuteEvents.endDragHandler);

            _startedDrag = false;

            if (_tempItemRect != null)
                CallBack?.Notify(_tempItemRect);
            ResetDragHandler();
        }

        private void ResetDragHandler()
        {
            _tempItemRect = null;
            

        }

        private void ProcessActionOnCardMoveDelta()
        {
            var instance = Instantiate(gameObject, _canvas.transform);
            _tempItemRect = instance.GetComponent<RectTransform>();
            var copyDragHandler = instance.GetComponent<DragHandler>();
            copyDragHandler.enabled = false;
            _tempItemRect.position = _rectTransform.position;
            _tempItemRect.sizeDelta = _rectTransform.sizeDelta;
        }
    }
}
