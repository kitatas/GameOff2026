using GameOff2026.Game.Application;
using GameOff2026.Game.Data.Entity;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Data.Entity
{
    [TestFixture]
    public sealed class FieldEntityTests
    {
        [Test]
        public void Constructor_FieldSquares_IsEmpty()
        {
            var entity = new FieldEntity();

            Assert.IsEmpty(entity.fieldSquares);
        }

        [Test]
        public void Add_FieldSquare_StoresSquareAndAllowsMovementOnIt()
        {
            var entity = new FieldEntity();
            var fieldSquare = FieldSquareVO.Create(2, 3, new FieldVO(FieldType.Ground, null, 1.0f));

            entity.Add(fieldSquare);

            Assert.AreEqual(1, entity.fieldSquares.Length);
            Assert.AreSame(fieldSquare, entity.fieldSquares[0]);
            Assert.IsTrue(entity.IsMovable(new SquareVO(2, 3)));
            Assert.IsFalse(entity.IsMovable(new SquareVO(3, 3)));
        }

        [Test]
        public void Clear_AfterAddingFieldSquares_RemovesAllSquaresAndMovability()
        {
            var entity = new FieldEntity();
            entity.Add(FieldSquareVO.Create(2, 3, new FieldVO(FieldType.Ground, null, 1.0f)));

            entity.Clear();

            Assert.IsEmpty(entity.fieldSquares);
            Assert.IsFalse(entity.IsMovable(new SquareVO(2, 3)));
        }
    }
}
