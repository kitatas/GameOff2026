using System;
using GameOff2026.Common.Application;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Common.Application
{
    [TestFixture]
    public sealed class ExceptionNotifyVOTests
    {
        [Test]
        public void Constructor_ValidValues_KeepsReferences()
        {
            var ex = new QuitExceptionVO("failed");
            var transition = new TransitionVO(Fade.In, 0.5f);

            var vo = new ExceptionNotifyVO(ex, transition);

            Assert.AreSame(ex, vo.exception);
            Assert.AreSame(transition, vo.transition);
        }

        [Test]
        public void Create_ValidValues_BuildsTransitionFromArguments()
        {
            var ex = new RetryExceptionVO("failed");

            var vo = ExceptionNotifyVO.Create(ex, Fade.Out, 1.5f);

            Assert.AreSame(ex, vo.exception);
            Assert.AreEqual(Fade.Out, vo.transition.fade);
            Assert.AreEqual(1.5f, vo.transition.duration);
        }

        [Test]
        public void Create_ExceptionIsNull_DoesNotThrow()
        {
            ExceptionNotifyVO vo = null;

            Assert.DoesNotThrow(() => vo = ExceptionNotifyVO.Create(null, Fade.Out, 0.5f));
            Assert.IsNull(vo.exception);
        }

        [Test]
        public void Create_FadeIsNone_Throws()
        {
            Assert.Catch<Exception>(() => ExceptionNotifyVO.Create(null, Fade.None, 0.5f));
        }

        [Test]
        public void Create_DurationIsNegative_Throws()
        {
            Assert.Catch<Exception>(() => ExceptionNotifyVO.Create(null, Fade.In, -0.1f));
        }
    }
}
