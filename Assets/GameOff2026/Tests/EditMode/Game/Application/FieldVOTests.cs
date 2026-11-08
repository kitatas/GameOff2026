using GameOff2026.Common.Application;
using GameOff2026.Game.Application;
using GameOff2026.Tests.EditMode.TestHelpers;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Application
{
    [TestFixture]
    public sealed class FieldVOTests
    {
        [Test]
        public void Constructor_ValidValues_RetainsFieldData()
        {
            var field = new FieldVO(FieldType.Ground, TestSpriteFactory.Valid, 1.0f);

            Assert.AreEqual(FieldType.Ground, field.type);
            Assert.AreSame(TestSpriteFactory.Valid, field.sprite);
            Assert.AreEqual(1.0f, field.weight);
        }

        [Test]
        public void Constructor_NoneType_ThrowsQuitException()
        {
            Assert.Throws<QuitExceptionVO>(() => new FieldVO(FieldType.None, null, 1.0f));
        }

        [Test]
        public void Constructor_NullSprite_ThrowsQuitException()
        {
            Assert.Throws<QuitExceptionVO>(() => new FieldVO(FieldType.Ground, null, 1.0f));
        }

        [TestCase(0.0f)]
        [TestCase(-1.0f)]
        public void Constructor_NonPositiveWeight_ThrowsQuitException(float weight)
        {
            Assert.Throws<QuitExceptionVO>(() => new FieldVO(FieldType.Ground, TestSpriteFactory.Valid, weight));
        }
    }
}
