using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameOff2026.Common.Application;
using GameOff2026.Common.Domain.Repository;
using GameOff2026.Common.Domain.UseCase;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Common.Domain.UseCase
{
    [TestFixture]
    public sealed class BaseSoundUseCaseTests
    {
        private FakeSoundRepository _soundRepository;
        private TestSoundUseCase _useCase;

        [SetUp]
        public void SetUp()
        {
            _soundRepository = new FakeSoundRepository();
            _useCase = new TestSoundUseCase(new SaveRepository(), _soundRepository);
        }

        [TearDown]
        public void TearDown()
        {
            ((IDisposable)_useCase).Dispose();
        }

        [Test]
        public void Play_ValidType_PublishesCreatedSound()
        {
            _useCase.Play(SeType.None, 0.25f);

            Assert.AreEqual(1, _soundRepository.FindCount);
        }

        [Test]
        public void SetVolume_OutsideRange_ClampsAndPublishesVolume()
        {
            _useCase.SetVolume(2.0f);
            Assert.AreEqual(1.0f, _useCase.thisVolumeValue);

            _useCase.SetVolume(-1.0f);
            Assert.AreEqual(0.0f, _useCase.thisVolumeValue);
        }

        [Test]
        public void SetMasterVolume_OutsideRange_ClampsMasterVolume()
        {
            _useCase.SetMasterVolume(2.0f);
            Assert.AreEqual(1.0f, _useCase.masterVolumeValue);

            _useCase.SetMasterVolume(-1.0f);
            Assert.AreEqual(0.0f, _useCase.masterVolumeValue);
        }

        [Test]
        public void SwitchMute_WhenOff_TogglesMuteAndSavesVolume()
        {
            _useCase.SwitchMute();

            Assert.IsTrue(_useCase.isThisMute.CurrentValue);
            Assert.AreEqual(1, _useCase.SaveVolumeCount);
        }

        [Test]
        public async Task LoadAsync_ValidVolumes_AppliesThisAndMasterVolumes()
        {
            await _useCase.LoadAsync(CancellationToken.None).AsTask();

            Assert.AreEqual(0.4f, _useCase.thisVolumeValue);
            Assert.AreEqual(0.8f, _useCase.masterVolumeValue);
            Assert.IsTrue(_useCase.isThisMute.CurrentValue);
            Assert.IsFalse(_useCase.isMasterMute.CurrentValue);
        }

        private sealed class FakeSoundRepository : ISoundRepository
        {
            public int FindCount { get; private set; }

            public BgmAudioVO Find(BgmType type)
            {
                return new BgmAudioVO(type, "BGM_TEST");
            }

            public SeAudioVO Find(SeType type)
            {
                FindCount++;
                return new SeAudioVO(type, "SE_TEST");
            }

            public AudioVO<T> Find<T>(T type) where T : Enum
            {
                return type switch
                {
                    BgmType bgm => Find(bgm) as AudioVO<T>,
                    SeType se => Find(se) as AudioVO<T>,
                    _ => null
                };
            }
        }

        private sealed class TestSoundUseCase : BaseSoundUseCase<SeType, SeSoundVO>
        {
            public int SaveVolumeCount { get; private set; }

            public TestSoundUseCase(SaveRepository saveRepository, ISoundRepository soundRepository)
                : base(saveRepository, soundRepository)
            {
            }

            protected override async UniTask<(VolumeVO thisVolume, VolumeVO masterVolume)> LoadVolumeAsync(
                CancellationToken token)
            {
                await UniTask.CompletedTask;
                return (new VolumeVO(0.4f, true), new VolumeVO(0.8f, false));
            }

            public override void SaveVolume()
            {
                SaveVolumeCount++;
            }

            protected override SeSoundVO CreateSound(AudioVO<SeType> audio, float delay)
            {
                return new SeSoundVO(audio as SeAudioVO, delay, isMute.CurrentValue);
            }
        }
    }
}
