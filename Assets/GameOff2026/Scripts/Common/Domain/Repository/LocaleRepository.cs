using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Common.Application;
using UnityEngine.Localization.Settings;

namespace GameOff2026.Common.Domain.Repository
{
    public sealed class LocaleRepository
    {
        private readonly Dictionary<string, string> _localeMap;

        public LocaleRepository()
        {
            _localeMap = new Dictionary<string, string>();
        }

        public async UniTask LoadAsync(CancellationToken token)
        {
            var stringTable = await LocalizationSettings.StringDatabase
                .GetTableAsync(LocaleConfig.TABLE_NAME).Task
                .AsUniTask()
                .AttachExternalCancellation(token);

            foreach (var entry in stringTable.Values)
            {
                _localeMap[entry.Key] = entry.Value;
            }
        }

        public string Get(string key)
        {
            return _localeMap.TryGetValue(key, out var value)
                ? value
                : throw new QuitExceptionVO(ExceptionConfig.NOT_FOUND_LOCALE);
        }
    }
}