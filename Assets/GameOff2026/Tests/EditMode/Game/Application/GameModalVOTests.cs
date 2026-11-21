using GameOff2026.Common.Application;
using GameOff2026.Game.Application;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Application
{
    [TestFixture]
    public sealed class GameModalVOTests
    {
        [Test]
        public void Constructor_ValidValues_KeepsTypeAndFade()
        {
            var vo = new GameModalVO(GameModalType.Menu, Fade.In);

            Assert.AreEqual(GameModalType.Menu, vo.type);
            Assert.AreEqual(Fade.In, vo.fade);
        }

        [Test]
        public void Constructor_TypeIsNone_ThrowsQuitException()
        {
            Assert.Throws<QuitExceptionVO>(() => new GameModalVO(GameModalType.None, Fade.In));
        }

        [Test]
        public void Constructor_FadeIsNone_ThrowsQuitException()
        {
            Assert.Throws<QuitExceptionVO>(() => new GameModalVO(GameModalType.Menu, Fade.None));
        }
    }
}

