using System;
using GameOff2026.Common.Application;
using GameOff2026.Game.Application;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Application
{
    [TestFixture]
    public sealed class FieldPatternVOTests
    {
        [Test]
        public void Constructor_ValidPattern_StoresIdAndRotatedFieldTypes()
        {
            var pattern = new FieldPatternVO(7, new[] { 1, 2, 3, 4, 5, 6, 1, 2, 3 });

            Assert.AreEqual(7, pattern.id);
            Assert.AreEqual(FieldConfig.HALF_FILE * FieldConfig.HALF_RANK, pattern.types.Length);
            Assert.IsTrue(Array.TrueForAll(pattern.types, type => type != FieldType.None));
        }

        [Test]
        public void Constructor_InvalidLength_ThrowsQuitException()
        {
            Assert.Throws<QuitExceptionVO>(() => new FieldPatternVO(1, new[] { 1, 2, 3, 4, 5, 6, 7, 8 }));
        }
    }
}
