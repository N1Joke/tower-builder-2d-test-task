using Assets._Project.Scripts.Gameplay;
using Core;
using DG.Tweening;
using System;
using Tools.Extensions;
using UnityEngine;

namespace Assets._Project.Scripts.Items
{
    public class BaseItem : BaseDisposable
    {
        public struct Ctx
        {
            public int id;
            public BaseItemView view;
            public ITrashHole trashHole;
        }

        protected readonly Ctx _ctx;
        protected Vector3 _towerPos;

        public ReactiveEvent<BaseItem> OnDestroy { get; private set; } = new();
        public Vector3 Pos => _ctx.view.transform.position;
        public Vector3 TowerPos => _towerPos;

        public BaseItem(Ctx ctx)
        {
            _ctx = ctx;

            Init();
        }

        protected virtual void Init()
        {
            _towerPos = Pos;
            AddObject(_ctx.view.gameObject);
            AddDispose(_ctx.view.dragHandler.OnEndDrag.SubscribeWithSkip(OnEndDrag));
        }

        protected virtual void OnEndDrag()
        {
            if (_ctx.trashHole.Utilize(_ctx.view.spriteRenderer))
            {
                OnDestroy?.Notify(this);

                _ctx.view.transform.DOScale(Vector3.zero, 0.2f).SetLink(_ctx.view.gameObject);
                _ctx.view.transform.DOMove(_ctx.trashHole.HoleCenter, 0.2f).SetLink(_ctx.view.gameObject).OnComplete(Dispose);               
            }
            else
                JumpIntoPos(_towerPos);
        }        

        public virtual void JumpIntoPos(Vector3 posToJump)
        {
            _ctx.view.transform.DOJump(posToJump, 3f, 1, 0.35f).SetLink(_ctx.view.gameObject);
            _towerPos = posToJump;
        }

        public void MoveTo(Vector3 position)
        {
            _ctx.view.transform.DOMove(position, 0.1f).SetEase(Ease.Linear).SetLink(_ctx.view.gameObject);
            _towerPos = position;
        }
    }
}