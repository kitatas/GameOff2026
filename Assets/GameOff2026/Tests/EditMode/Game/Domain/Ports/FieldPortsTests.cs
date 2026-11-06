using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameOff2026.Game.Application;
using GameOff2026.Game.Domain.Ports;
using GameOff2026.Tests.EditMode.TestHelpers;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Domain.Ports
{
    [TestFixture]
    public sealed class FieldPortsTests
    {
        [Test]
        public async Task PublishRenderFieldSquareAsync_RenderMessage_PublishesSameValueAndToken()
        {
            var publisher = new FakeAsyncPublisher<RenderFieldSquareVO>();
            var ports = new FieldPorts(new FakeAsyncSubscriber<RenderFieldSquareVO>(), publisher);
            var renderFieldSquare = new RenderFieldSquareVO(RenderType.Refresh, new FieldSquareVO[0]);
            using var source = new CancellationTokenSource();

            await ports.PublishRenderFieldSquareAsync(renderFieldSquare, source.Token).AsTask();

            Assert.AreEqual(1, publisher.PublishCount);
            Assert.AreSame(renderFieldSquare, publisher.LastPublished);
            Assert.AreEqual(source.Token, publisher.tokens[0]);
        }

        [Test]
        public void RenderFieldSquareSubscriber_AfterConstruction_ExposesProvidedSubscriber()
        {
            var subscriber = new FakeAsyncSubscriber<RenderFieldSquareVO>();
            var ports = new FieldPorts(subscriber, new FakeAsyncPublisher<RenderFieldSquareVO>());

            Assert.AreSame(subscriber, ports.renderFieldSquareSubscriber);
        }
    }
}
