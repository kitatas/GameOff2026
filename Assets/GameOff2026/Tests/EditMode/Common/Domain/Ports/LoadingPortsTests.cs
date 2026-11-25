using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameOff2026.Common.Application;
using GameOff2026.Common.Domain.Ports;
using GameOff2026.Tests.EditMode.TestHelpers;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Common.Domain.Ports
{
    [TestFixture]
    public sealed class LoadingPortsTests
    {
        private LoadingPorts _ports;
        private FakeAsyncSubscriber<LoadingTransitionVO> _subscriber;
        private FakeAsyncPublisher<LoadingTransitionVO> _publisher;

        [SetUp]
        public void SetUp()
        {
            _subscriber = new FakeAsyncSubscriber<LoadingTransitionVO>();
            _publisher = new FakeAsyncPublisher<LoadingTransitionVO>();
            _ports = new LoadingPorts(_subscriber, _publisher);
        }

        [Test]
        public void Constructor_GivenSubscriber_ExposesIt()
        {
            Assert.AreSame(_subscriber, _ports.loadingTransitionSubscriber);
        }

        [Test]
        public async Task PublishLoadingTransitionAsync_Valid_PublishesToPublisher()
        {
            var transition = LoadingTransitionVO.Create(Fade.InOut, 0.5f);
            using var cts = new CancellationTokenSource();

            await _ports.PublishLoadingTransitionAsync(transition, cts.Token).AsTask();

            Assert.AreEqual(1, _publisher.PublishCount);
            Assert.AreSame(transition, _publisher.LastPublished);
            Assert.AreEqual(cts.Token, _publisher.tokens[0]);
        }
    }
}

