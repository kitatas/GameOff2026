using GameOff2026.Common.Application;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Common.Application
{
    [TestFixture]
    public sealed class BaseModalTransitionVOTests
    {
        private enum TestModalType
        {
            None,
            Menu,
        }

        [Test]
        public void Constructor_ValidValues_KeepsTypeAndTransition()
        {
            var transition = new TransitionVO(Fade.In, 0.25f);

            var vo = new TestModalTransitionVO(TestModalType.Menu, transition);

            Assert.AreEqual(TestModalType.Menu, vo.type);
            Assert.AreSame(transition, vo.transition);
        }

        private sealed class TestModalTransitionVO : BaseModalTransitionVO<TestModalType>
        {
            public TestModalTransitionVO(TestModalType type, TransitionVO transition) : base(type, transition)
            {
            }
        }
    }
}

