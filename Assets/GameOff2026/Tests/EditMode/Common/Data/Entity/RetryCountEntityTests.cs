using GameOff2026.Common.Application;
using GameOff2026.Common.Data.Entity;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Common.Data.Entity
{
    [TestFixture]
    public sealed class RetryCountEntityTests
    {
        private RetryCountEntity _entity;

        [SetUp]
        public void SetUp()
        {
            _entity = new RetryCountEntity();
        }

        [Test]
        public void Constructor_Default_ValueIsZero()
        {
            Assert.AreEqual(0, _entity.value);
        }

        [Test]
        public void Increment_Called_AddsOne()
        {
            _entity.Increment();

            Assert.AreEqual(1, _entity.value);
        }

        [Test]
        public void Increment_CalledMultipleTimes_Accumulates()
        {
            _entity.Increment();
            _entity.Increment();
            _entity.Increment();

            Assert.AreEqual(3, _entity.value);
        }

        [Test]
        public void Reset_AfterIncrement_ValueIsZero()
        {
            _entity.Increment();
            _entity.Increment();

            _entity.Reset();

            Assert.AreEqual(0, _entity.value);
        }

        [Test]
        public void Update_IsEqualTrue_Increments()
        {
            _entity.Set(2);

            _entity.Update(true);

            Assert.AreEqual(3, _entity.value);
        }

        [Test]
        public void Update_IsEqualFalse_ResetsToOne()
        {
            _entity.Set(2);

            _entity.Update(false);

            Assert.AreEqual(1, _entity.value);
        }

        [Test]
        public void Update_IsEqualFalseFromZero_BecomesOne()
        {
            _entity.Update(false);

            Assert.AreEqual(1, _entity.value);
        }

        [Test]
        public void IsMaxRetry_Initial_ReturnsFalse()
        {
            Assert.IsFalse(_entity.IsMaxRetry());
        }

        [Test]
        public void IsMaxRetry_ValueEqualsMax_ReturnsFalse()
        {
            _entity.Set(ExceptionConfig.MAX_RETRY_COUNT);

            Assert.IsFalse(_entity.IsMaxRetry());
        }

        [Test]
        public void IsMaxRetry_ValueExceedsMax_ReturnsTrue()
        {
            _entity.Set(ExceptionConfig.MAX_RETRY_COUNT + 1);

            Assert.IsTrue(_entity.IsMaxRetry());
        }

        [Test]
        public void IsMaxRetry_ReachedByRepeatedUpdate_ReturnsTrueOnlyAfterExceedingMax()
        {
            for (var i = 0; i < ExceptionConfig.MAX_RETRY_COUNT; i++)
            {
                _entity.Increment();
            }

            Assert.IsFalse(_entity.IsMaxRetry());

            _entity.Increment();

            Assert.IsTrue(_entity.IsMaxRetry());
        }
    }
}
