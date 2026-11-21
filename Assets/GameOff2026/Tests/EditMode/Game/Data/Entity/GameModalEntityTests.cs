using GameOff2026.Common.Application;
using GameOff2026.Game.Application;
using GameOff2026.Game.Data.Entity;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Data.Entity
{
    [TestFixture]
    public sealed class GameModalEntityTests
    {
        [Test]
        public void Constructor_NoValue_ValueIsNull()
        {
            var entity = new GameModalEntity();

            Assert.IsNull(entity.value);
        }

        [Test]
        public void Set_ValidTransition_StoresValue()
        {
            var entity = new GameModalEntity();
            var transition = GameModalTransitionVO.Create(GameModalType.Menu, Fade.In, 0.25f);

            entity.Set(transition);

            Assert.AreSame(transition, entity.value);
        }
    }
}

