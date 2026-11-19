using GameOff2026.Common.Application;
using GameOff2026.Game.Application;
using GameOff2026.Game.Data.DataStore;
using GameOff2026.Game.Data.DataStore.Tables;

namespace GameOff2026.Game.Domain.Repository
{
    public sealed class TreasurePriceRepository
    {
        private readonly TreasurePriceMasterTable _treasurePriceMasterTable;

        public TreasurePriceRepository(MemoryDatabase memoryDatabase)
        {
            _treasurePriceMasterTable = memoryDatabase.TreasurePriceMasterTable;
        }

        public TreasurePriceVO Find(int floor)
        {
            var treasurePrice = _treasurePriceMasterTable.FindClosestByFloor(floor);
            return treasurePrice == null
                ? throw new QuitExceptionVO(ExceptionConfig.NOT_FOUND_TREASURE_PRICE)
                : treasurePrice.ToVO();
        }
    }
}