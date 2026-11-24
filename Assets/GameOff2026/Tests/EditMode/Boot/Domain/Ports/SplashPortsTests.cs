using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameOff2026.Boot.Application;
using GameOff2026.Boot.Domain.Ports;
using GameOff2026.Common.Application;
using GameOff2026.Tests.EditMode.TestHelpers;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Boot.Domain.Ports
{
    [TestFixture]
    public sealed class SplashPortsTests
    {
        [Test]
        public async Task PublishSplashTransitionAsync_SplashTransition_PublishesSameTransition()
        {
            var publisher = new FakeAsyncPublisher<SplashTransitionVO>();
            var ports = new SplashPorts(new FakeAsyncSubscriber<SplashTransitionVO>(), publisher);
            var splashTransition = SplashTransitionVO.Create(null, Fade.Out, 0.0f);
            using var source = new CancellationTokenSource();

            await ports.PublishSplashTransitionAsync(splashTransition, source.Token).AsTask();

            Assert.AreSame(splashTransition, publisher.LastPublished);
            Assert.AreEqual(source.Token, publisher.tokens[0]);
        }

        [Test]
        public void Constructor_Subscriber_ExposesProvidedSubscriber()
        {
            var subscriber = new FakeAsyncSubscriber<SplashTransitionVO>();
            var ports = new SplashPorts(subscriber, new FakeAsyncPublisher<SplashTransitionVO>());

            Assert.AreSame(subscriber, ports.splashTransitionSubscriber);
        }
    }
}

