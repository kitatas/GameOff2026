using System;
using System.Collections.Generic;
using System.Linq;

namespace GameOff2026.Common.Utility
{
    public static class CollectionHelper
    {
        public static T WeightedRandom<T>(this IReadOnlyList<T> source, Func<T, float> weightSelector)
        {
            var totalWeight = source.Sum(weightSelector);
            var random = UnityEngine.Random.Range(0.0f, totalWeight);

            foreach (var item in source)
            {
                random -= weightSelector(item);
                if (random < 0f) return item;
            }

            return source[^1];
        }
    }
}