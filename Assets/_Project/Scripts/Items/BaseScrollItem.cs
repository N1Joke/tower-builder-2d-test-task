using Assets._Project.Scripts.Gameplay;
using Assets._Project.Scripts.GUI;
using Assets._Project.Scripts.Presets;
using Core;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Project.Scripts.Items
{
    public class BaseScrollItem : BaseDisposable
    {
        public struct Ctx
        {
            public ScrollRect scrollRect;
            public RectTransform content;
            public Canvas canvas;
            public ItemConfig config;
            public int id;
            public ITowerBuilder towerBuilder;
            internal IScrollChecker scrollChecker;
        }

        protected readonly Ctx _ctx;
        protected ScrollItemView _view;

        public BaseScrollItem(Ctx ctx)
        {
            _ctx = ctx;

            Init();
        }

        protected virtual void Init()
        {
            _view = GameObject.Instantiate(_ctx.config.scrollItemViewPrefab, _ctx.content);
            _view.GetComponent<Image>().sprite = _ctx.config.itemSprites[_ctx.id];
            _view.dragHandler.Init(_ctx.canvas, _ctx.scrollRect.gameObject);
            AddDispose(_view.dragHandler.CallBack.SubscribeWithSkip(OnDragEnd));
            AddObject(_view.gameObject);
        }

        private void OnDragEnd(RectTransform rectTransform)
        {
            if (_ctx.scrollChecker.IsInScrollPlate(rectTransform))
            {
                DestroyWithAnimation(rectTransform);
                return;
            }
            if (_ctx.towerBuilder.PlaceItem(rectTransform, _ctx.id))
                GameObject.Destroy(rectTransform.gameObject);
            else
                DestroyWithAnimation(rectTransform);
        }

        private void DestroyWithAnimation(RectTransform rectTransform)
        {
            rectTransform.DORotate(new Vector3(0f, 0f, 180f), 0.25f).SetEase(Ease.Linear).SetLink(rectTransform.gameObject);
            rectTransform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.Linear).SetLink(rectTransform.gameObject).OnComplete(() => GameObject.Destroy(rectTransform.gameObject));
        }
    }
}