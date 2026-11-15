using GameOff2026.Common.Application;
using GameOff2026.Game.Application;
using GameOff2026.Game.Data.Entity;
using GameOff2026.Tests.EditMode.TestHelpers;
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
            var fieldSquare = FieldSquareVO.Create(2, 3, new FieldVO(FieldType.Empty, TestSpriteFactory.Valid, 1.0f));

            entity.Add(fieldSquare);

            Assert.AreEqual(1, entity.fieldSquares.Length);
            Assert.AreSame(fieldSquare, entity.fieldSquares[0]);
            Assert.IsTrue(entity.IsMovable(new SquareVO(2, 3), DateType.Day));
            Assert.IsFalse(entity.IsMovable(new SquareVO(3, 3), DateType.Day));
        }

        [Test]
        public void Clear_AfterAddingFieldSquares_RemovesAllSquaresAndMovability()
        {
            var entity = new FieldEntity();
            entity.Add(FieldSquareVO.Create(2, 3, new FieldVO(FieldType.Empty, TestSpriteFactory.Valid, 1.0f)));

            entity.Clear();

            Assert.IsEmpty(entity.fieldSquares);
            Assert.IsFalse(entity.IsMovable(new SquareVO(2, 3), DateType.Day));
        }

        [Test]
        public void SetField_ExistingIndex_ReplacesFieldAndPreservesSquare()
        {
            var entity = new FieldEntity();
            var originalFieldSquare =
                FieldSquareVO.Create(2, 3, new FieldVO(FieldType.Empty, TestSpriteFactory.Valid, 1.0f));
            var replacementField = new FieldVO(FieldType.Treasure, TestSpriteFactory.Valid, 1.0f);
            entity.Add(originalFieldSquare);

            entity.SetField(0, replacementField);

            Assert.AreEqual(1, entity.fieldSquares.Length);
            Assert.AreSame(originalFieldSquare.square, entity.fieldSquares[0].square);
            Assert.AreSame(replacementField, entity.fieldSquares[0].field);
        }

        [Test]
        public void SetField_InvalidIndex_ThrowsQuitException()
        {
            var entity = new FieldEntity();

            Assert.Throws<QuitExceptionVO>(() =>
                entity.SetField(0, new FieldVO(FieldType.Empty, TestSpriteFactory.Valid, 1.0f)));
        }

        [Test]
        public void Find_ExistingSquare_ReturnsFieldSquareAndIndex()
        {
            var entity = new FieldEntity();
            var expected = FieldSquareVO.Create(2, 3, new FieldVO(FieldType.Empty, TestSpriteFactory.Valid, 1.0f));
            entity.Add(FieldSquareVO.Create(1, 3, new FieldVO(FieldType.Empty, TestSpriteFactory.Valid, 1.0f)));
            entity.Add(expected);

            var (fieldSquare, index) = entity.Find(new SquareVO(2, 3));

            Assert.AreSame(expected, fieldSquare);
            Assert.AreEqual(1, index);
        }

        [Test]
        public void Find_MissingSquare_ReturnsNullAndMinusOne()
        {
            var entity = new FieldEntity();
            entity.Add(FieldSquareVO.Create(2, 3, new FieldVO(FieldType.Empty, TestSpriteFactory.Valid, 1.0f)));

            var (fieldSquare, index) = entity.Find(new SquareVO(3, 3));

            Assert.IsNull(fieldSquare);
            Assert.AreEqual(-1, index);
        }

        [Test]
        public void IsMovable_DayEnemySquare_ReturnsFalse()
        {
            var entity = new FieldEntity();
            entity.Add(FieldSquareVO.Create(2, 3, new FieldVO(FieldType.EnemyDay, TestSpriteFactory.Valid, 1.0f)));

            Assert.IsFalse(entity.IsMovable(new SquareVO(2, 3), DateType.Day));
        }

        [TestCase(FieldType.EnemyRouteActive)]
        [TestCase(FieldType.EnemyRouteDeactive)]
        public void IsMovable_EnemyRouteSquare_ReturnsTrue(FieldType fieldType)
        {
            var entity = new FieldEntity();
            entity.Add(FieldSquareVO.Create(2, 3, new FieldVO(fieldType, TestSpriteFactory.Valid, 1.0f)));

            Assert.IsTrue(entity.IsMovable(new SquareVO(2, 3), DateType.Day));
        }

        [Test]
        public void IsEnemyRouteActive_ActiveRoute_ReturnsTrue()
        {
            var entity = new FieldEntity();
            entity.Add(FieldSquareVO.Create(2, 3,
                new FieldVO(FieldType.EnemyRouteActive, TestSpriteFactory.Valid, 1.0f)));

            Assert.IsTrue(entity.IsEnemyRouteActive(new SquareVO(2, 3)));
        }

        [TestCase(FieldType.EnemyRouteDeactive)]
        [TestCase(FieldType.Empty)]
        public void IsEnemyRouteActive_NonActiveRoute_ReturnsFalse(FieldType fieldType)
        {
            var entity = new FieldEntity();
            entity.Add(FieldSquareVO.Create(2, 3, new FieldVO(fieldType, TestSpriteFactory.Valid, 1.0f)));

            Assert.IsFalse(entity.IsEnemyRouteActive(new SquareVO(2, 3)));
        }

        [Test]
        public void IsEnemyRouteActive_MissingSquare_ReturnsFalse()
        {
            var entity = new FieldEntity();

            Assert.IsFalse(entity.IsEnemyRouteActive(new SquareVO(2, 3)));
        }
    }
}
