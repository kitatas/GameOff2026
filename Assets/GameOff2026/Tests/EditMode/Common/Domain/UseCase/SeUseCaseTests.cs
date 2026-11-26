using System;
using GameOff2026.Common.Application;
using GameOff2026.Common.Domain.Repository;
using GameOff2026.Common.Domain.UseCase;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Common.Domain.UseCase
{
    [TestFixture]
    public sealed class SeUseCaseTests
    {
        private FakeSoundRepository _soundRepository;
        private SeUseCase _useCase;

        [SetUp]
        public void SetUp()
        {
            _soundRepository = new FakeSoundRepository();
            _useCase = new SeUseCase(new SaveRepository(), _soundRepository);
        }

        [TearDown]
        public void TearDown()
        {
            ((IDisposable)_useCase).Dispose();
        }

        [Test]
        public void PlayOneShots_MultipleLoop_ResolvesAudioForEachShot()
        {
            _useCase.PlayOneShots(SeType.None, 3, 0.1f);
            Assert.AreEqual(3, _soundRepository.SeFindCount);
        }

        [Test]
        public void SetMasterVolume_OutsideRange_ClampsVolume()
        {
            _useCase.SetMasterVolume(2.0f);
            Assert.AreEqual(1.0f, _useCase.masterVolumeValue);
            _useCase.SetMasterVolume(-1.0f);
            Assert.AreEqual(0.0f, _useCase.masterVolumeValue);
        }

        private sealed class FakeSoundRepository : ISoundRepository
        {
            public int SeFindCount { get; private set; }

            public BgmAudioVO Find(BgmType type)
            {
                return new BgmAudioVO(type, "BGM_TEST");
            }

            public SeAudioVO Find(SeType type)
            {
                SeFindCount++;
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
    }
}