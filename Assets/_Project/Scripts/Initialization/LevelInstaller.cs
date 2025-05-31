using System.Collections.Generic;
using System;
using UnityEngine;
using Zenject;
using Assets._Project.Scripts.Gameplay;
using Assets._Project.Scripts.GUI;
using Assets._Project.Scripts.Presets;
using GUI;

namespace MonoInstallers
{
    public class LevelInstaller : MonoInstaller
    {
        [field: SerializeField] private SpriteRenderer _towerSpritePart;
        [field: SerializeField] private Camera _camera;

        private List<IDisposable> _disposables = new List<IDisposable>();
        [Inject]
        private ItemConfig _itemConfig;
        [Inject]
        private GUIView _guiView;

        public override void InstallBindings()
        {
           
        }

        private void Awake()
        {
            var towerBuilder = new TowerBuilder(new TowerBuilder.Ctx
            {
                camera = _camera,
                spriteRenderer = _towerSpritePart
            });
            _disposables.Add(towerBuilder);

            _disposables.Add(new ScrollController(new ScrollController.Ctx
            {
                config = _itemConfig,
                content = _guiView.scrollContent,
                scrollRect = _guiView.scrollRect,
                canvas = _guiView.canvas,
                towerBuilder = towerBuilder
            }));
        }

        private void OnDestroy()
        {
            for (int i = _disposables.Count - 1; i >= 0; i--)
                _disposables[i]?.Dispose();
            _disposables.Clear();
        }
    }
}