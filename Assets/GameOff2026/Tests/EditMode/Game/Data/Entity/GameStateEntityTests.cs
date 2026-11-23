using GameOff2026.Game.Application;
using GameOff2026.Game.Data.Entity;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Data.Entity
{
    [TestFixture]
    public sealed class GameStateEntityTests
    {
        [Test]
        public void Set_GameState_UpdatesValue()
        {
            var entity = new GameStateEntity();

            entity.Set(GameState.Wander);

            Assert.AreEqual(GameState.Wander, entity.value);
        }
    }
}

