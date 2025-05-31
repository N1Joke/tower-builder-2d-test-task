using Assets._Project.Scripts.Localization;
using Core;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Tools.Extensions;
using UnityEngine.Localization;

namespace Assets._Project.Scripts.GUI
{
    public class LogMessenger : BaseDisposable, ILogMessenger
    {
        private const string TABREF = "GameTexts";

        public struct Ctx
        {
            public TextMeshProUGUI tmp;
        }

        private readonly Ctx _ctx;
        private IDisposable _removeTextTimer;

        private readonly Dictionary<string, string> _localizedCache = new();

        public LogMessenger(Ctx ctx)
        {
            _ctx = ctx;

            PreloadLocalizationKeys();
        }

        public void PreloadLocalizationKeys()
        {
            foreach (var key in typeof(LocalizationKeys).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy))
            {
                var keyValue = key.GetValue(null) as string;
                GetLocalizedTextAsync(keyValue, (text) => { });
            }
        }

        public void ShowLog(string key)
        {
            _removeTextTimer?.Dispose();
            _ctx.tmp.text = "";

            if (_localizedCache.TryGetValue(key, out var cachedText))            
                ApplyText(cachedText);            
            else            
                GetLocalizedTextAsync(key, ApplyText);            
        }

        private void ApplyText(string text)
        {
            _ctx.tmp.text = text;
            _removeTextTimer = ReactiveExtensions.DelayedCall(5f, ClearText);
        }

        private void ClearText()
        {
            _ctx.tmp.text = "";
        }

        private async void GetLocalizedTextAsync(string key, Action<string> callback)
        {
            var localizedString = new LocalizedString(TABREF, key);
            var handle = localizedString.GetLocalizedStringAsync();
            await handle.ToUniTask();

            var result = handle.Result;
            callback?.Invoke(result);

            if (!_localizedCache.ContainsKey(key))            
                _localizedCache[key] = result;            
        }

        protected override void OnDispose()
        {
            _removeTextTimer?.Dispose();
            base.OnDispose();
        }
    }
}