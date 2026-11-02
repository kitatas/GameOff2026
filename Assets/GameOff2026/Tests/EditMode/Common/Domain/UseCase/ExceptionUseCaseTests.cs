using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameOff2026.Common.Application;
using GameOff2026.Common.Domain.Ports;
using GameOff2026.Common.Domain.UseCase;
using GameOff2026.Tests.EditMode.TestHelpers;
using NUnit.Framework;
using R3;

namespace GameOff2026.Tests.EditMode.Common.Domain.UseCase
{
    [TestFixture]
    public sealed class ExceptionUseCaseTests
    {
        private ExceptionUseCase _useCase;
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
            var ports = new ExceptionPorts(_notifySubscriber, _actionSubscriber, _notifyPublisher, _actionPublisher);
            _useCase = new ExceptionUseCase(ports);
        }

        [TearDown]
        public void TearDown()
        {
            ((IDisposable)_useCase).Dispose();
        }

        [Test]
        public void Subscribers_AfterConstruction_ExposePortsSubscribers()
        {
            Assert.AreSame(_notifySubscriber, _useCase.exceptionNotify);
            Assert.AreSame(_actionSubscriber, _useCase.exceptionAction);
        }

        [Test]
        public void ThrowAsync_BeforeDecision_PublishesFadeInOnlyAndStaysPending()
        {
            var ex = new RetryExceptionVO("failed");

            var task = _useCase.ThrowAsync(ex, CancellationToken.None).AsTask();

            Assert.IsFalse(task.IsCompleted);
            Assert.AreEqual(1, _notifyPublisher.PublishCount);
            Assert.AreSame(ex, _notifyPublisher.LastPublished.exception);
            Assert.AreEqual(Fade.In, _notifyPublisher.LastPublished.transition.fade);
            Assert.AreEqual(ExceptionConfig.FADE_DURATION, _notifyPublisher.LastPublished.transition.duration);
            Assert.AreEqual(0, _actionPublisher.PublishCount);
        }

        [Test]
        public async Task ThrowAsync_AfterDecision_PublishesFadeOutWithoutException()
        {
            var task = _useCase.ThrowAsync(new RetryExceptionVO("failed"), CancellationToken.None).AsTask();

            _useCase.HandleDecision(Unit.Default);
            await task;

            Assert.AreEqual(2, _notifyPublisher.PublishCount);
            var fadeOut = _notifyPublisher.published[1];
            Assert.IsNull(fadeOut.exception);
            Assert.AreEqual(Fade.Out, fadeOut.transition.fade);
            Assert.AreEqual(ExceptionConfig.FADE_DURATION, fadeOut.transition.duration);
        }

        [Test]
        public async Task ThrowAsync_AfterDecision_PublishesActionWithSameException()
        {
            var ex = new RebootExceptionVO("failed");
            var task = _useCase.ThrowAsync(ex, CancellationToken.None).AsTask();

            _useCase.HandleDecision(Unit.Default);
            await task;

            Assert.AreEqual(1, _actionPublisher.PublishCount);
            Assert.AreSame(ex, _actionPublisher.LastPublished.exception);
        }

        [Test]
        public async Task ThrowAsync_Completed_PropagatesTokenToAllPublishes()
        {
            var task = _useCase.ThrowAsync(new QuitExceptionVO("failed"), CancellationToken.None).AsTask();

            _useCase.HandleDecision(Unit.Default);
            await task;

            Assert.IsTrue(_notifyPublisher.tokens.TrueForAll(t => t == CancellationToken.None));
            Assert.AreEqual(CancellationToken.None, _actionPublisher.tokens[0]);
        }

        [Test]
        public async Task ThrowAsync_DecisionBeforeThrow_DoesNotCompleteUntilNextDecision()
        {
            // 呼び出し前の決定は待機中の ThrowAsync に影響しない（Subject は履歴を持たない）
            _useCase.HandleDecision(Unit.Default);

            var task = _useCase.ThrowAsync(new RetryExceptionVO("failed"), CancellationToken.None).AsTask();

            Assert.IsFalse(task.IsCompleted);
            Assert.AreEqual(0, _actionPublisher.PublishCount);

            _useCase.HandleDecision(Unit.Default);
            await task;
        }

        [Test]
        public async Task ThrowRebootAsync_Message_PublishesRebootException()
        {
            var task = _useCase.ThrowRebootAsync("failed", CancellationToken.None).AsTask();
            _useCase.HandleDecision(Unit.Default);
            await task;

            var ex = _actionPublisher.LastPublished.exception;
            Assert.IsInstanceOf<RebootExceptionVO>(ex);
            Assert.AreEqual("failed\n" + ExceptionConfig.REBOOT_MESSAGE, ex.message);
        }

        [Test]
        public async Task ThrowRetryAsync_Message_PublishesRetryException()
        {
            var task = _useCase.ThrowRetryAsync("failed", CancellationToken.None).AsTask();
            _useCase.HandleDecision(Unit.Default);
            await task;

            var ex = _actionPublisher.LastPublished.exception;
            Assert.IsInstanceOf<RetryExceptionVO>(ex);
            Assert.AreEqual("failed\n" + ExceptionConfig.RETRY_MESSAGE, ex.message);
        }

        [Test]
        public async Task ThrowQuitAsync_Message_PublishesQuitException()
        {
            var task = _useCase.ThrowQuitAsync("failed", CancellationToken.None).AsTask();
            _useCase.HandleDecision(Unit.Default);
            await task;

            var ex = _actionPublisher.LastPublished.exception;
            Assert.IsInstanceOf<QuitExceptionVO>(ex);
            Assert.AreEqual("failed\n" + ExceptionConfig.QUIT_MESSAGE, ex.message);
        }

        [Test]
        public async Task FadeOutAsync_Duration_PublishesFadeOutNotifyWithoutException()
        {
            await _useCase.FadeOutAsync(2.0f, CancellationToken.None).AsTask();

            Assert.AreEqual(1, _notifyPublisher.PublishCount);
            Assert.IsNull(_notifyPublisher.LastPublished.exception);
            Assert.AreEqual(Fade.Out, _notifyPublisher.LastPublished.transition.fade);
            Assert.AreEqual(2.0f, _notifyPublisher.LastPublished.transition.duration);
        }

        [Test]
        public void FadeOutAsync_NegativeDuration_Throws()
        {
            Assert.Catch<Exception>(() => _useCase.FadeOutAsync(-1.0f, CancellationToken.None));

            Assert.AreEqual(0, _notifyPublisher.PublishCount);
        }

        [Test]
        public void HandleDecision_NoWaiter_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => _useCase.HandleDecision(Unit.Default));
        }
    }
}
