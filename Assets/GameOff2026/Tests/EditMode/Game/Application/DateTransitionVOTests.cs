using GameOff2026.Common.Application;
using GameOff2026.Game.Application;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Application
{
    [TestFixture]
    public sealed class DateTransitionVOTests
    {
        [Test]
        public void Constructor_ValidValues_RetainsDateAndTransition()
        {
            var transition = new TransitionVO(Fade.InOut, 0.25f);

            var dateTransition = new DateTransitionVO(DateType.Night, transition);

            Assert.AreEqual(DateType.Night, dateTransition.date);
            Assert.AreSame(transition, dateTransition.transition);
        }

        [Test]
        public void Create_ValidValues_CreatesTransitionWithFadeAndDuration()
        {
            var dateTransition = DateTransitionVO.Create(DateType.Day, Fade.InOut, 0.5f);

            Assert.AreEqual(DateType.Day, dateTransition.date);
            Assert.AreEqual(Fade.InOut, dateTransition.transition.fade);
            Assert.AreEqual(0.5f, dateTransition.transition.duration);
        }
    }
}