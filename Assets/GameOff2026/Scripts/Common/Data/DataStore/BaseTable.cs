using System.Collections.Generic;
using UnityEngine;

namespace GameOff2026.Common.Data.DataStore
{
    public abstract class BaseTable<T> : ScriptableObject where T : ScriptableObject
    {
        [SerializeField] private List<T> list = default;

        public IEnumerable<T> all => list;
    }
}