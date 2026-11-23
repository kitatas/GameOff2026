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
    public sealed class ShopPortsTests
    {
        [Test]
        public async Task PublishShopsAsync_ShopsAndToken_PublishesSameValues()
        {
            var publisher = new FakeAsyncPublisher<ShopVO[]>();
            var ports = new ShopPorts(new FakeAsyncSubscriber<ShopVO[]>(), publisher);
            var shops = new[]
            {
                new ShopVO(new ShopItemVO(1, "Item", "Description", 50, false, 10),
                    new PurchasedItemVO(1, 0), true),
            };
            using var source = new CancellationTokenSource();

            await ports.PublishShopsAsync(shops, source.Token).AsTask();

            Assert.AreEqual(1, publisher.PublishCount);
            Assert.AreSame(shops, publisher.LastPublished);
            Assert.AreEqual(source.Token, publisher.tokens[0]);
        }

        [Test]
        public void Constructor_Subscriber_ExposesProvidedSubscriber()
        {
            var subscriber = new FakeAsyncSubscriber<ShopVO[]>();
            var ports = new ShopPorts(subscriber, new FakeAsyncPublisher<ShopVO[]>());

            Assert.AreSame(subscriber, ports.shopSubscriber);
        }
    }
}

