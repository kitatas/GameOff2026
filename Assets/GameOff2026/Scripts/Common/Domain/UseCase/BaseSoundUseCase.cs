using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Common.Application;
using GameOff2026.Common.Domain.Repository;
using R3;
using UnityEngine;

namespace GameOff2026.Common.Domain.UseCase
{
    public abstract class BaseSoundUseCase<TType, TSoundVO> : IDisposable
        where TType : Enum where TSoundVO : SoundVO<TType>
    {
        protected readonly SaveRepository _saveRepository;
        private readonly ISoundRepository _soundRepository;
        private readonly Subject<TSoundVO> _play;
        private readonly ReactiveProperty<float> _thisVolume;
        private readonly ReactiveProperty<float> _masterVolume;
        private readonly ReadOnlyReactiveProperty<float> _volume;
        private readonly ReactiveProperty<bool> _isThisMute;
        private readonly ReactiveProperty<bool> _isMasterMute;
        private readonly ReadOnlyReactiveProperty<bool> _isMute;

        public BaseSoundUseCase(SaveRepository saveRepository, ISoundRepository soundRepository)
        {
            _saveRepository = saveRepository;
            _soundRepository = soundRepository;
            _play = new Subject<TSoundVO>();
            _thisVolume = new ReactiveProperty<float>(0.0f);
            _masterVolume = new ReactiveProperty<float>(0.0f);
            _volume = _thisVolume
                .CombineLatest(_masterVolume, (x, y) => x * y)
                .ToReadOnlyReactiveProperty();
            _isThisMute = new ReactiveProperty<bool>(false);
            _isMasterMute = new ReactiveProperty<bool>(false);
            _isMute = _isThisMute
                .CombineLatest(_isMasterMute, (x, y) => x || y)
                .ToReadOnlyReactiveProperty();
        }

        public virtual Subject<TSoundVO> play => _play;
        public virtual ReadOnlyReactiveProperty<float> volume => _volume;
        public float thisVolumeValue => _thisVolume.CurrentValue;
        public float masterVolumeValue => _masterVolume.CurrentValue;
        public virtual ReadOnlyReactiveProperty<bool> isThisMute => _isThisMute;
        public virtual ReadOnlyReactiveProperty<bool> isMasterMute => _isMasterMute;
        public virtual ReadOnlyReactiveProperty<bool> isMute => _isMute;

        protected abstract UniTask<(VolumeVO thisVolume, VolumeVO masterVolume)> LoadVolumeAsync(
            CancellationToken token);

        public virtual async UniTask LoadAsync(CancellationToken token)
        {
            var data = await LoadVolumeAsync(token);
            SetVolume(data.thisVolume.value);
            SetMasterVolume(data.masterVolume.value);
            _isThisMute.Value = data.thisVolume.isMute;
            _isMasterMute.Value = data.masterVolume.isMute;
        }

        public virtual void Play(TType type, float delay = 0.0f)
        {
            var audio = _soundRepository.Find(type);
            var sound = CreateSound(audio, delay);
            _play?.OnNext(sound);
        }

        public virtual void SetVolume(float value)
        {
            _thisVolume.Value = Mathf.Clamp01(value);
        }

        public virtual void SetMasterVolume(float value)
        {
            _masterVolume.Value = Mathf.Clamp01(value);
        }

        public virtual void SwitchMute()
        {
            _isThisMute.Value = !_isThisMute.CurrentValue;
            SaveVolume();
        }

        public virtual void SwitchMasterMute()
        {
            _isMasterMute.Value = !_isMasterMute.CurrentValue;
        }

        public abstract void SaveVolume();

        public void SaveMasterVolume()
        {
            var vo = new VolumeVO(_masterVolume.CurrentValue, _isMasterMute.CurrentValue);
            _saveRepository.SaveMasterVolume(vo);
        }

        protected abstract TSoundVO CreateSound(AudioVO<TType> audio, float delay);

        void IDisposable.Dispose()
        {
            _play?.Dispose();
            _thisVolume?.Dispose();
            _masterVolume?.Dispose();
            _volume?.Dispose();
            _isThisMute?.Dispose();
            _isMasterMute?.Dispose();
            _isMute?.Dispose();
        }
    }
}