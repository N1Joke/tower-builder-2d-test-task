using GUI;
using Presets;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace MonoInstallers
{
    public class BootstrapInstaller : MonoInstaller
    {
        [Header("Prefabs")]
        [SerializeField] private GUIView _guiViewPrefab;
        [Header("Presets")]
        [SerializeField] private GameSettings _gameSettings;

        private List<IDisposable> _disposables = new List<IDisposable>();

        public override void InstallBindings()
        {
            DontDestroyOnLoad(this);

            var guiInstance = Instantiate(_guiViewPrefab, transform);

           
        }

        private void OnDestroy()
        {
            for (int i = _disposables.Count - 1; i >= 0; i--)
                _disposables[i]?.Dispose();
            _disposables.Clear();
        }
    }
}