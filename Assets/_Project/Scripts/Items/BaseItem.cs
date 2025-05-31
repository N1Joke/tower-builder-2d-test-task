using Assets._Project.Scripts.Gameplay;
using Assets._Project.Scripts.GUI;
using Assets._Project.Scripts.Localization;
using Core;
using DG.Tweening;
using Presets;
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
            public ILogMessenger logMessenger;
            public GameSettings gameConfig;
        }

        protected readonly Ctx _ctx;
        protected Vector3 _towerPos;

        public ReactiveEvent<BaseItem> OnDestroy { get; private set; } = new();
        public Vector3 Pos => _ctx.view.transform.position;
        public Vector3 TowerPos => _towerPos;

        public int Id => _ctx.id;

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
                _ctx.logMessenger.ShowLog(LocalizationKeys.TRASH_HOLE_CUBE_REMOVE);

                _ctx.view.transform.DOScale(Vector3.zero, _ctx.gameConfig.cubeDestroyTime).SetLink(_ctx.view.gameObject);
                _ctx.view.transform.DOMove(_ctx.trashHole.HoleCenter, _ctx.gameConfig.cubeDestroyTime).SetLink(_ctx.view.gameObject).OnComplete(Dispose);
            }
            else
            {
                JumpIntoPos(_towerPos);
                _ctx.logMessenger.ShowLog(LocalizationKeys.CUBE_BACK_TO_TOWER);
            }
        }

        public virtual void JumpIntoPos(Vector3 posToJump)
        {
            _ctx.view.transform.DOJump(posToJump, _ctx.gameConfig.cubeJumpPower, 1, _ctx.gameConfig.cubeJumpTime).SetLink(_ctx.view.gameObject);
            _towerPos = posToJump;
        }

        public void MoveTo(Vector3 position)
        {
            _ctx.view.transform.DOMove(position, _ctx.gameConfig.delayBetweenRemoveElementFromTower).SetEase(Ease.Linear).SetLink(_ctx.view.gameObject);
            _towerPos = position;
        }
    }
}