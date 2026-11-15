using System;
using GameOff2026.Game.Application;
using GameOff2026.Game.Data.Entity;
using GameOff2026.Game.Domain.UseCase;
using NUnit.Framework;
using R3;

namespace GameOff2026.Tests.EditMode.Game.Domain.UseCase
{
    [TestFixture]
    public sealed class DayUseCaseTests
    {
        private DateEntity _dateEntity;
        private DayEntity _dayEntity;
        private DayUseCase _useCase;

        [SetUp]
        public void SetUp()
        {
            _dateEntity = new DateEntity();
            _dayEntity = new DayEntity();
            _useCase = new DayUseCase(_dateEntity, _dayEntity);
        }

        [TearDown]
        public void TearDown()
        {
            ((IDisposable)_useCase).Dispose();
        }

        [Test]
        public void SetUp_ResetsDayAndPublishesDayOne()
        {
            _dayEntity.Set(5);
            using var day = _useCase.day;

            _useCase.SetUp();

            Assert.AreEqual(1, _dayEntity.value);
            Assert.AreEqual(1, ((IReadOnlyBindableReactiveProperty<int>)day).Value);
        }

        [Test]
        public void Dawn_DayDate_IncrementsAndPublishesDay()
        {
            _dateEntity.Set(DateType.Day);
            _useCase.SetUp();
            using var day = _useCase.day;

            _useCase.Dawn();

            Assert.AreEqual(2, _dayEntity.value);
            Assert.AreEqual(2, ((IReadOnlyBindableReactiveProperty<int>)day).Value);
        }

        [Test]
        public void Dawn_NightDate_DoesNotChangeOrPublishDay()
        {
            _dateEntity.Set(DateType.Night);
            _useCase.SetUp();
            using var day = _useCase.day;

            _useCase.Dawn();

            Assert.AreEqual(1, _dayEntity.value);
            Assert.AreEqual(1, ((IReadOnlyBindableReactiveProperty<int>)day).Value);
        }

        [TestCase(DateType.Day, 7, false)]
        [TestCase(DateType.Night, 6, false)]
        [TestCase(DateType.Night, 7, true)]
        [TestCase(DateType.Night, 8, false)]
        public void IsMaxDay_DateAndDay_ReturnsWhetherNightIsAtMaximum(
            DateType date, int day, bool expected)
        {
            _dateEntity.Set(date);
            _dayEntity.Set(day);

            Assert.AreEqual(expected, _useCase.IsMaxDay());
        }
    }
}