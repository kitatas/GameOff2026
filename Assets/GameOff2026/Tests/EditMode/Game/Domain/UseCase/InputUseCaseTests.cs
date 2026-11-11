using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameOff2026.Game.Application;
using GameOff2026.Game.Domain.UseCase;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Domain.UseCase
{
    [TestFixture]
    public sealed class InputUseCaseTests
    {
        private InputUseCase _useCase;

        [SetUp]
        public void SetUp()
        {
            _useCase = new InputUseCase();
        }

        [TearDown]
        public void TearDown()
        {
            ((IDisposable)_useCase).Dispose();
        }

        [Test]
        public async Task InputAsync_InputHandledAfterSubscription_ReturnsInput()
        {
            var input = new InputVO(InputType.Move, Direction.Right);
            var inputTask = _useCase.InputAsync(CancellationToken.None).AsTask();

            _useCase.HandleInput(input);
            var actual = await inputTask;

            Assert.AreSame(input, actual);
        }

        [Test]
        public async Task InputAsync_CancellationRequested_ThrowsOperationCanceledException()
        {
            using var source = new CancellationTokenSource();
            var inputTask = _useCase.InputAsync(source.Token).AsTask();

            source.Cancel();

            try
            {
                await inputTask;
                Assert.Fail("OperationCanceledException が発生しませんでした。");
            }
            catch (OperationCanceledException)
            {
            }
        }
    }
}