using GameOff2026.Boot.Application;
using GameOff2026.Common.Application;
using GameOff2026.Tests.EditMode.TestHelpers;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Boot.Application
{
    [TestFixture]
    public sealed class SplashVOTests
    {
        [Test]
        public void Constructor_NoneType_ThrowsInvalidSplashException()
        {
            Assert.Throws<QuitExceptionVO>(() => new SplashVO(SplashType.None, TestSpriteFactory.Valid));
        }

        [Test]
        public void Constructor_ValidValues_RetainsValues()
        {
            var splash = new SplashVO(SplashType.Developer, TestSpriteFactory.Valid);

            Assert.AreEqual(SplashType.Developer, splash.type);
            Assert.AreSame(TestSpriteFactory.Valid, splash.sprite);
        }
    }
}

