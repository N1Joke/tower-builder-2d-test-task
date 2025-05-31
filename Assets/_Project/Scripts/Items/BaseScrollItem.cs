using Assets._Project.Scripts.Gameplay;
using Assets._Project.Scripts.GUI;
using Assets._Project.Scripts.Localization;
using Assets._Project.Scripts.Presets;
using Core;
using DG.Tweening;
using Presets;
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
            public IScrollChecker scrollChecker;
            public ILogMessenger logMessenger;
            public GameSettings gameConfig;
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
                _ctx.logMessenger.ShowLog(LocalizationKeys.FAIL_DRAG_CUBE_IN_SCROLL);
                DestroyWithAnimation(rectTransform);
                return;
            }
            if (_ctx.towerBuilder.PlaceItem(rectTransform, _ctx.id))
            {
                GameObject.Destroy(rectTransform.gameObject);
                _ctx.logMessenger.ShowLog(LocalizationKeys.SUCCESS_CUBE_PLACE);
            }
            else
                DestroyWithAnimation(rectTransform);            
        }

        private void DestroyWithAnimation(RectTransform rectTransform)
        {
            rectTransform.DORotate(new Vector3(0f, 0f, 180f), _ctx.gameConfig.cubeDestroyTime).SetEase(Ease.Linear).SetLink(rectTransform.gameObject);
            rectTransform.DOScale(Vector3.zero, _ctx.gameConfig.cubeDestroyTime).SetEase(Ease.Linear).SetLink(rectTransform.gameObject).OnComplete(() => GameObject.Destroy(rectTransform.gameObject));
        }
    }
}