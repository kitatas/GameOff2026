using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameOff2026.Common.Application;
using GameOff2026.Common.Data.Entity;
using GameOff2026.Common.Domain.Ports;
using GameOff2026.Common.Domain.UseCase;
using GameOff2026.Tests.EditMode.TestHelpers;
using NUnit.Framework;
using R3;

namespace GameOff2026.Tests.EditMode.Common.Domain.UseCase
{
    [TestFixture]
    public sealed class BaseStateUseCaseTests
    {
        private TestEntity _entity;
        private TestStateUseCase _useCase;

        [SetUp]
        public void SetUp()
        {
            _entity = new TestEntity();
            var ports = new TestPorts(
                new FakeAsyncSubscriber<TestState>(),
                new FakeAsyncPublisher<TestState>());
            _useCase = new TestStateUseCase(_entity, new RetryCountEntity(), ports);
        }

        [TearDown]
        public void TearDown()
        {
            _useCase.Dispose();
        }

        [Test]
        public void ForceChange_DifferentState_EmitsAndUpdatesForceChangeState()
        {
            var received = new List<TestState>();
            using var subscription = _useCase.forceChange.Subscribe(received.Add);

            _useCase.ForceChange(TestState.Playing);

            CollectionAssert.AreEqual(new[] { TestState.None, TestState.Playing }, received);
            Assert.AreEqual(TestState.Playing, _useCase.forceChangeState);
        }

        [Test]
        public void ForceChange_CurrentState_DoesNotEmit()
        {
            _entity.Set(TestState.Playing);
            var received = new List<TestState>();
            using var subscription = _useCase.forceChange.Subscribe(received.Add);

            _useCase.ForceChange(TestState.Playing);

            CollectionAssert.AreEqual(new[] { TestState.None }, received);
        }

        [Test]
        public void IsMaxRetry_RepeatedSameState_ReturnsTrueOnlyAfterLimit()
        {
            _entity.Set(TestState.Playing);

            for (var i = 0; i < ExceptionConfig.MAX_RETRY_COUNT; i++)
            {
                Assert.IsFalse(_useCase.IsMaxRetry(TestState.Playing));
            }

            Assert.IsTrue(_useCase.IsMaxRetry(TestState.Playing));
        }

        [Test]
        public void IsMaxRetry_DifferentState_ResetsRetryCount()
        {
            _entity.Set(TestState.Playing);
            _useCase.IsMaxRetry(TestState.Playing);

            Assert.IsFalse(_useCase.IsMaxRetry(TestState.Idle));
            Assert.IsFalse(_useCase.IsMaxRetry(TestState.Playing));
        }

        private enum TestState
        {
            None,
            Idle,
            Playing,
        }

        private sealed class TestEntity : BaseEntity<TestState>
        {
        }

        private sealed class TestPorts : BaseStatePorts<TestState>
        {
            public TestPorts(FakeAsyncSubscriber<TestState> subscriber, FakeAsyncPublisher<TestState> publisher)
                : base(subscriber, publisher)
            {
            }
        }

        private sealed class TestStateUseCase : BaseStateUseCase<TestState>
        {
            public TestStateUseCase(TestEntity entity, RetryCountEntity retryCountEntity, TestPorts ports)
                : base(entity, retryCountEntity, ports)
            {
            }

            public override UniTask InitAsync(System.Threading.CancellationToken token)
            {
                return UniTask.CompletedTask;
            }
        }
    }
}
