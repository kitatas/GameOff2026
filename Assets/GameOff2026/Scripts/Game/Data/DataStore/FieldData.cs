using GameOff2026.Game.Application;
using UnityEngine;

namespace GameOff2026.Game.Data.DataStore
{
    [CreateAssetMenu(fileName = nameof(FieldData), menuName = "DataTable/" + nameof(FieldData))]
    public sealed class FieldData : ScriptableObject
    {
        [SerializeField] private FieldType fieldType = default;
        [SerializeField] private Sprite sprite = default;
        [SerializeField] private float weight = 0.0f;

        public FieldType type => fieldType;
        public FieldVO ToVO() => new(type, sprite, weight);
    }
}