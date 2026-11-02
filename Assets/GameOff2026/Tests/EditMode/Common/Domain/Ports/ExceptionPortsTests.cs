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
    public sealed class ExceptionPortsTests
    {
        private ExceptionPorts _ports;
        private FakeAsyncSubscriber<ExceptionNotifyVO> _notifySubscriber;
        private FakeAsyncSubscriber<ExceptionActionVO> _actionSubscriber;
        private FakeAsyncPublisher<ExceptionNotifyVO> _notifyPublisher;
        private FakeAsyncPublisher<ExceptionActionVO> _actionPublisher;

        [SetUp]
        public void SetUp()
        {
            _notifySubscriber = new FakeAsyncSubscriber<ExceptionNotifyVO>();
            _actionSubscriber = new FakeAsyncSubscriber<ExceptionActionVO>();
            _notifyPublisher = new FakeAsyncPublisher<ExceptionNotifyVO>();
            _actionPublisher = new FakeAsyncPublisher<ExceptionActionVO>();
            _ports = new ExceptionPorts(_notifySubscriber, _actionSubscriber, _notifyPublisher, _actionPublisher);
        }

        [Test]
        public void Constructor_GivenSubscribers_ExposesThemInOrder()
        {
            Assert.AreSame(_notifySubscriber, _ports.exceptionNotifySubscriber);
            Assert.AreSame(_actionSubscriber, _ports.exceptionActionSubscriber);
        }

        [Test]
        public async Task PublishExceptionNotifyAsync_Valid_PublishesToNotifyPublisherOnly()
        {
            var notify = ExceptionNotifyVO.Create(null, Fade.In, 0.5f);
            using var cts = new CancellationTokenSource();

            await _ports.PublishExceptionNotifyAsync(notify, cts.Token).AsTask();

            Assert.AreEqual(1, _notifyPublisher.PublishCount);
            Assert.AreSame(notify, _notifyPublisher.LastPublished);
            Assert.AreEqual(cts.Token, _notifyPublisher.tokens[0]);
            Assert.AreEqual(0, _actionPublisher.PublishCount);
        }

        [Test]
        public async Task PublishExceptionActionAsync_Valid_PublishesToActionPublisherOnly()
        {
            var action = new ExceptionActionVO(new QuitExceptionVO("failed"));
            using var cts = new CancellationTokenSource();

            await _ports.PublishExceptionActionAsync(action, cts.Token).AsTask();

            Assert.AreEqual(1, _actionPublisher.PublishCount);
            Assert.AreSame(action, _actionPublisher.LastPublished);
            Assert.AreEqual(cts.Token, _actionPublisher.tokens[0]);
            Assert.AreEqual(0, _notifyPublisher.PublishCount);
        }
    }
}
