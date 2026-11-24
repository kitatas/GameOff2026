using GameOff2026.Common.Data.DataStore;
using UnityEngine;

namespace GameOff2026.Boot.Data.DataStore
{
    [CreateAssetMenu(fileName = nameof(SplashTable), menuName = "DataTable/" + nameof(SplashTable))]
    public sealed class SplashTable : BaseTable<SplashData>
    {
    }
}