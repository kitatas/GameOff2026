using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameOff2026.Common.Application;
using GameOff2026.Common.Data.Entity;
using GameOff2026.Common.Domain.Ports;
using GameOff2026.Common.Domain.UseCase;
using GameOff2026.Tests.EditMode.TestHelpers;
using MessagePipe;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Common.Domain.UseCase
{
    [TestFixture]
    public sealed class BaseModalUseCaseTests
    {
        private enum TestModalType
        {
            None,
            Menu,
        }

        private TestModalEntity _entity;
        private FakeAsyncSubscriber<BaseModalTransitionVO<TestModalType>> _subscriber;
        private FakeAsyncPublisher<BaseModalTransitionVO<TestModalType>> _publisher;
        private TestModalUseCase _useCase;

        [SetUp]
        public void SetUp()
        {
            _entity = new TestModalEntity();
            _subscriber = new FakeAsyncSubscriber<BaseModalTransitionVO<TestModalType>>();
            _publisher = new FakeAsyncPublisher<BaseModalTransitionVO<TestModalType>>();
            var ports = new TestModalPorts(_subscriber, _publisher);
            _useCase = new TestModalUseCase(_entity, ports);
        }

        [Test]
        public void Subscriber_AfterConstruction_ExposesPortsSubscriber()
        {
            Assert.AreSame(_subscriber, _useCase.subscriber);
        }

        [Test]
        public async Task FadeAsync_ValidModal_UpdatesEntityAndPublishesTransition()
        {
            var modal = new TestModalVO(TestModalType.Menu, Fade.InOut);
            using var source = new CancellationTokenSource();

            await _useCase.FadeAsync(modal, source.Token).AsTask();

            var actual = _entity.value as TestModalTransitionVO;
            Assert.IsNotNull(actual);
            Assert.AreEqual(TestModalType.Menu, actual.type);
            Assert.AreEqual(Fade.InOut, actual.transition.fade);
            Assert.AreEqual(0.25f, actual.transition.duration);
            Assert.AreEqual(1, _publisher.PublishCount);
            Assert.AreSame(actual, _publisher.LastPublished);
            Assert.AreEqual(source.Token, _publisher.tokens[0]);
        }

        private sealed class TestModalEntity : BaseEntity<BaseModalTransitionVO<TestModalType>>
        {
        }

        private sealed class TestModalVO : BaseModalVO<TestModalType>
        {
            public TestModalVO(TestModalType type, Fade fade) : base(type, fade)
            {
            }
        }

        private sealed class TestModalTransitionVO : BaseModalTransitionVO<TestModalType>
        {
            public TestModalTransitionVO(TestModalType type, TransitionVO transition) : base(type, transition)
            {
            }
        }

        private sealed class TestModalPorts : BasePubSubPorts<BaseModalTransitionVO<TestModalType>>
        {
            public TestModalPorts(
                IAsyncSubscriber<BaseModalTransitionVO<TestModalType>> subscriber,
                IAsyncPublisher<BaseModalTransitionVO<TestModalType>> publisher) : base(subscriber, publisher)
            {
            }
        }

        private sealed class TestModalUseCase : BaseModalUseCase<TestModalType>
        {
            public TestModalUseCase(TestModalEntity entity, TestModalPorts ports) : base(entity, ports)
            {
            }

            protected override BaseModalTransitionVO<TestModalType> GetModalTransition(BaseModalVO<TestModalType> modal)
            {
                var testModal = modal as TestModalVO;
                var transition = new TransitionVO(testModal.fade, 0.25f);
                return new TestModalTransitionVO(testModal.type, transition);
            }
        }
    }
}