using GameOff2026.Game.Application;
using GameOff2026.Game.Data.Entity;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Data.Entity
{
    [TestFixture]
    public sealed class PlayerEntityTests
    {
        [Test]
        public void Constructor_DefaultSquare_IsOrigin()
        {
            var entity = new PlayerEntity();

            Assert.AreEqual(0, entity.square.file);
            Assert.AreEqual(0, entity.square.rank);
        }

        [Test]
        public void SetSquare_NewSquare_ReplacesCurrentSquare()
        {
            var entity = new PlayerEntity();
            var square = new SquareVO(3, 5);

            entity.SetSquare(square);

            Assert.AreSame(square, entity.square);
        }
    }
}
