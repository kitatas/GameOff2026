using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Common.Application;
using GameOff2026.Common.Data.DataStore;
using UnityEngine;

namespace GameOff2026.Common.Domain.Repository
{
    public sealed class SaveRepository
    {
        private static SaveDTO Load()
        {
            var data = ES3.Load(SaveConfig.ES3_KEY, defaultValue: "");
            return string.IsNullOrEmpty(data)
                ? Create()
                : JsonUtility.FromJson<SaveDTO>(data);
        }

        public async UniTask<SaveVO> LoadAsync(CancellationToken token)
        {
#if UNITY_WEBGL
            // WebGL はスレッドプールが使えないため、RunOnThreadPool と同じ挙動をメインスレッド上で再現する
            await UniTask.Yield(PlayerLoopTiming.Update, token);
            token.ThrowIfCancellationRequested();

            SaveDTO dto;
            try
            {
                dto = Load();
            }
            finally
            {
                await UniTask.Yield();
            }

            return dto.ToVO();
#else
            var dto = await UniTask.RunOnThreadPool(Load, cancellationToken: token);
            return dto.ToVO();
#endif
        }

        private static SaveDTO Create()
        {
            var data = new SaveDTO();
            Save(data);
            return data;
        }

        private static void Save(SaveDTO data)
        {
            ES3.Save(SaveConfig.ES3_KEY, JsonUtility.ToJson(data));
        }
    }
}