using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Common.Application;
using GameOff2026.Common.Domain.Repository;

namespace GameOff2026.Common.Domain.UseCase
{
    public sealed class SeUseCase : BaseSoundUseCase<SeType, SeSoundVO>
    {
        public SeUseCase(SaveRepository saveRepository, ISoundRepository soundRepository) : base(saveRepository,
            soundRepository)
        {
        }

        protected override async UniTask<(VolumeVO thisVolume, VolumeVO masterVolume)> LoadVolumeAsync(
            CancellationToken token)
        {
            var saveData = await _saveRepository.LoadAsync(token);
            return (new VolumeVO(0.0f, false), new VolumeVO(0.0f, false));
            // TODO: return (saveData.seVolume, saveData.masterVolume);
        }

        public override void SaveVolume()
        {
            var vo = new VolumeVO(thisVolumeValue, isThisMute.CurrentValue);
            // TODO: _saveRepository.SaveSeVolume(vo);
        }

        public void PlayOneShots(SeType type, int loop, float delayRate)
        {
            for (int i = 0; i < loop; i++)
            {
                Play(type, delayRate * i);
            }
        }

        protected override SeSoundVO CreateSound(AudioVO<SeType> audio, float delay)
        {
            return new SeSoundVO(audio as SeAudioVO, delay, isMute.CurrentValue);
        }
    }
}