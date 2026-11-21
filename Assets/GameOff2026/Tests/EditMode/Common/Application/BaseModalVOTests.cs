using GameOff2026.Common.Application;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Common.Application
{
    [TestFixture]
    public sealed class BaseModalVOTests
    {
        private enum TestModalType
        {
            None,
            Menu,
        }

        [Test]
        public void Constructor_ValidValues_KeepsTypeAndFade()
        {
            var vo = new TestModalVO(TestModalType.Menu, Fade.In);

            Assert.AreEqual(TestModalType.Menu, vo.type);
            Assert.AreEqual(Fade.In, vo.fade);
        }

        [Test]
        public void Constructor_FadeIsNone_ThrowsQuitException()
        {
            Assert.Throws<QuitExceptionVO>(() => new TestModalVO(TestModalType.Menu, Fade.None));
        }

        private sealed class TestModalVO : BaseModalVO<TestModalType>
        {
            public TestModalVO(TestModalType type, Fade fade) : base(type, fade)
            {
            }
        }
    }
}

