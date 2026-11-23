using System.Collections.Generic;
using GameOff2026.Common.Utility;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Common.Utility
{
    [TestFixture]
    public sealed class CollectionHelperTests
    {
        [Test]
        public void WeightedRandom_ZeroWeightItems_SelectsPositiveWeightItem()
        {
            IReadOnlyList<string> source = new[] { "zero", "target" };

            var result = source.WeightedRandom(item => item == "target" ? 1.0f : 0.0f);

            Assert.AreEqual("target", result);
        }

        [Test]
        public void WeightedRandom_EmptyList_ThrowsArgumentOutOfRangeException()
        {
            IReadOnlyList<string> source = new string[0];

            Assert.Throws<System.ArgumentOutOfRangeException>(() => source.WeightedRandom(_ => 1.0f));
        }

        [Test]
        public void WeightedRandom_SingleItem_ReturnsItem()
        {
            IReadOnlyList<string> source = new[] { "only" };

            var result = source.WeightedRandom(_ => 1.0f);

            Assert.AreEqual("only", result);
        }
    }
}