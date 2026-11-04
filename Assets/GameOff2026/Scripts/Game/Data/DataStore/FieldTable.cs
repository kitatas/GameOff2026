using GameOff2026.Common.Data.DataStore;
using UnityEngine;

namespace GameOff2026.Game.Data.DataStore
{
    [CreateAssetMenu(fileName = nameof(FieldTable), menuName = "DataTable/" + nameof(FieldTable))]
    public sealed class FieldTable : BaseTable<FieldData>
    {
    }
}