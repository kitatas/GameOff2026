using GameOff2026.Game.Application;
using UnityEngine;

namespace GameOff2026.Game.Data.DataStore
{
    [CreateAssetMenu(fileName = nameof(FieldData), menuName = "DataTable/" + nameof(FieldData))]
    public sealed class FieldData : ScriptableObject
    {
        [SerializeField] private FieldType fieldType = default;
        [SerializeField] private GameObject fieldObject = default;

        public FieldType type => fieldType;
        public FieldVO ToVO() => new(type, fieldObject);
    }
}