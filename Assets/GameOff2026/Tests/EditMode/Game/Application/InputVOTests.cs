using GameOff2026.Common.Application;
using GameOff2026.Game.Application;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Application
{
    [TestFixture]
    public sealed class InputVOTests
    {
        [TestCase(InputType.Move, Direction.Up)]
        [TestCase(InputType.Shift, Direction.None)]
        public void Constructor_ValidInput_PreservesValues(InputType input, Direction direction)
        {
            var inputVO = new InputVO(input, direction);

            Assert.AreEqual(input, inputVO.input);
            Assert.AreEqual(direction, inputVO.direction);
        }

        [Test]
        public void Constructor_NoneInput_ThrowsQuitException()
        {
            Assert.Throws<QuitExceptionVO>(() => new InputVO(InputType.None, Direction.Up));
        }

        [Test]
        public void Constructor_MoveWithoutDirection_ThrowsQuitException()
        {
            Assert.Throws<QuitExceptionVO>(() => new InputVO(InputType.Move, Direction.None));
        }
    }
}