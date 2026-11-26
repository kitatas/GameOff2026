using System;
using GameOff2026.Common.Application;
using GameOff2026.Common.Domain.Repository;
using GameOff2026.Common.Domain.UseCase;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Common.Domain.UseCase
{
    [TestFixture]
    public sealed class BgmUseCaseTests
    {
        private FakeSoundRepository _soundRepository;
        private BgmUseCase _useCase;

        [SetUp]
        public void SetUp()
        {
            _soundRepository = new FakeSoundRepository();
            _useCase = new BgmUseCase(new SaveRepository(), _soundRepository);
        }

        [TearDown]
        public void TearDown()
        {
            ((IDisposable)_useCase).Dispose();
        }

        [Test]
        public void Play_ValidType_RequestsAudioFromRepository()
        {
            _useCase.Play(BgmType.None, 0.25f);
            Assert.AreEqual(BgmType.None, _soundRepository.LastBgmType);
        }

        [Test]
        public void SetVolume_OutsideRange_ClampsVolume()
        {
            _useCase.SetVolume(2.0f);
            Assert.AreEqual(1.0f, _useCase.thisVolumeValue);
            _useCase.SetVolume(-1.0f);
            Assert.AreEqual(0.0f, _useCase.thisVolumeValue);
        }

        private sealed class FakeSoundRepository : ISoundRepository
        {
            public BgmType LastBgmType { get; private set; }

            public BgmAudioVO Find(BgmType type)
            {
                LastBgmType = type;
                return new BgmAudioVO(type, "BGM_TEST");
            }

            public SeAudioVO Find(SeType type)
            {
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