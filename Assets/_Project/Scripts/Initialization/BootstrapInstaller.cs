using Assets._Project.Scripts.GUI;
using Assets._Project.Scripts.Presets;
using Assets._Project.Scripts.UserData;
using GUI;
using Presets;
using System;
using System.Collections.Generic;
using UnityEngine;
using UserSave;
using Zenject;

namespace MonoInstallers
{
    public class BootstrapInstaller : MonoInstaller
    {
        [Header("Prefabs")]
        [SerializeField] private GUIView _guiViewPrefab;
        [Header("Presets")]
        [SerializeField] private GameSettings _gameSettings;
        [SerializeField] private ItemConfig _itemConfig;

        private List<IDisposable> _disposables = new List<IDisposable>();
        private GUIView _guiInstance;

        public override void InstallBindings()
        {
            DontDestroyOnLoad(this);
        }

        private void Awake()
        {
            Container.Bind<ItemConfig>().FromScriptableObject(_itemConfig).AsSingle();
            Container.Bind<GameSettings>().FromScriptableObject(_gameSettings).AsSingle();

            _guiInstance = Instantiate(_guiViewPrefab, transform);
            Container.Bind<GUIView>().FromInstance(_guiInstance).AsSingle();

            var logs = new LogMessenger(new LogMessenger.Ctx
            {
                tmp = _guiInstance.messageContainer
            });
            Container.Bind<ILogMessenger>().FromInstance(logs).AsSingle();
            _disposables.Add(logs);

            var userData = new UserData();
            Container.Bind<ITowerSaver>().FromInstance(userData).AsSingle();
            _disposables.Add(userData);

            base.Start();
        }

        private void OnDestroy()
        {
            GameObject.Destroy(_guiInstance.gameObject);

            for (int i = _disposables.Count - 1; i >= 0; i--)
                _disposables[i]?.Dispose();
            _disposables.Clear();
        }
    }
}