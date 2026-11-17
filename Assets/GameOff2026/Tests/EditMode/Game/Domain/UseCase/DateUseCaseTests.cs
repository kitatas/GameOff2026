using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameOff2026.Common.Application;
using GameOff2026.Game.Application;
using GameOff2026.Game.Data.Entity;
using GameOff2026.Game.Domain.Ports;
using GameOff2026.Game.Domain.UseCase;
using GameOff2026.Tests.EditMode.TestHelpers;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Domain.UseCase
{
    [TestFixture]
    public sealed class DateUseCaseTests
    {
        private DateEntity _dateEntity;
        private FakeAsyncSubscriber<DateTransitionVO> _subscriber;
        private FakeAsyncPublisher<DateTransitionVO> _publisher;
        private DateUseCase _useCase;

        [SetUp]
        public void SetUp()
        {
            _dateEntity = new DateEntity();
            _subscriber = new FakeAsyncSubscriber<DateTransitionVO>();
            _publisher = new FakeAsyncPublisher<DateTransitionVO>();
            _useCase = new DateUseCase(_dateEntity, new DatePorts(_subscriber, _publisher));
        }

        [Test]
        public async Task InitAsync_InitializesDateAndPublishesImmediateTransition()
        {
            using var source = new CancellationTokenSource();

            await _useCase.InitAsync(source.Token).AsTask();

            Assert.AreEqual(DateType.Night, _dateEntity.value);
            Assert.AreEqual(1, _publisher.PublishCount);
            Assert.AreEqual(DateType.Night, _publisher.LastPublished.date);
            Assert.AreEqual(Fade.InOut, _publisher.LastPublished.transition.fade);
            Assert.AreEqual(0.0f, _publisher.LastPublished.transition.duration);
            Assert.AreEqual(source.Token, _publisher.tokens[0]);
        }

        [Test]
        public async Task SetUpAsync_ResetsDateAndPublishesConfiguredTransition()
        {
            _dateEntity.Set(DateType.Night);
            using var source = new CancellationTokenSource();

            await _useCase.SetUpAsync(source.Token).AsTask();

            Assert.AreEqual(DateType.Day, _dateEntity.value);
            Assert.AreEqual(1, _publisher.PublishCount);
            Assert.AreEqual(DateType.Day, _publisher.LastPublished.date);
            Assert.AreEqual(Fade.InOut, _publisher.LastPublished.transition.fade);
            Assert.AreEqual(DateConfig.FADE_DURATION, _publisher.LastPublished.transition.duration);
            Assert.AreEqual(source.Token, _publisher.tokens[0]);
        }

        [TestCase(DateType.Day, DateType.Night)]
        [TestCase(DateType.Night, DateType.Day)]
        public async Task ShiftAsync_ShiftRequested_TogglesDateAndPublishesTransition(
            DateType date, DateType expectedDate)
        {
            _dateEntity.Set(date);
            using var source = new CancellationTokenSource();

            await _useCase.ShiftAsync(source.Token).AsTask();

            Assert.AreEqual(expectedDate, _dateEntity.value);
            Assert.AreEqual(1, _publisher.PublishCount);
            Assert.AreEqual(expectedDate, _publisher.LastPublished.date);
            Assert.AreEqual(DateConfig.FADE_DURATION, _publisher.LastPublished.transition.duration);
            Assert.AreEqual(source.Token, _publisher.tokens[0]);
        }

        [Test]
        public void DateTransition_AfterConstruction_ExposesPortsSubscriber()
        {
            Assert.AreSame(_subscriber, _useCase.dateTransition);
        }
    }
}