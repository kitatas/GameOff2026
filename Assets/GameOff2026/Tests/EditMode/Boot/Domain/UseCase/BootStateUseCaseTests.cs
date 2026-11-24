using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameOff2026.Boot.Application;
using GameOff2026.Boot.Data.Entity;
using GameOff2026.Boot.Domain.Ports;
using GameOff2026.Boot.Domain.UseCase;
using GameOff2026.Common.Data.Entity;
using GameOff2026.Tests.EditMode.TestHelpers;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Boot.Domain.UseCase
{
    [TestFixture]
    public sealed class BootStateUseCaseTests
    {
        private BootStateEntity _entity;
        private FakeAsyncPublisher<BootState> _publisher;
        private BootStateUseCase _useCase;

        [SetUp]
        public void SetUp()
        {
            _entity = new BootStateEntity();
            _publisher = new FakeAsyncPublisher<BootState>();
            var ports = new BootStatePorts(new FakeAsyncSubscriber<BootState>(), _publisher);
            _useCase = new BootStateUseCase(_entity, new RetryCountEntity(), ports);
        }

        [TearDown]
        public void TearDown()
        {
            _useCase.Dispose();
        }

        [Test]
        public async Task InitAsync_InitializesAndPublishesInitState()
        {
            await _useCase.InitAsync(CancellationToken.None).AsTask();

            Assert.AreEqual(BootState.Init, _entity.value);
            Assert.AreEqual(1, _publisher.PublishCount);
            Assert.AreEqual(BootState.Init, _publisher.LastPublished);
        }

        [Test]
        public async Task PublishAsync_None_DoesNotPublishOrChangeState()
        {
            await _useCase.PublishAsync(BootState.None, CancellationToken.None).AsTask();

            Assert.AreEqual(0, _publisher.PublishCount);
            Assert.AreEqual(default(BootState), _entity.value);
        }

        [Test]
        public async Task PublishAsync_ValidState_UpdatesEntityAndPublishes()
        {
            await _useCase.PublishAsync(BootState.Splash, CancellationToken.None).AsTask();

            Assert.AreEqual(BootState.Splash, _entity.value);
            Assert.AreEqual(1, _publisher.PublishCount);
            Assert.AreEqual(BootState.Splash, _publisher.LastPublished);
        }
    }
}

