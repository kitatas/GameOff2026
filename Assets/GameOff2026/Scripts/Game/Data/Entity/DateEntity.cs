using GameOff2026.Common.Application;
using GameOff2026.Common.Data.Entity;
using GameOff2026.Game.Application;

namespace GameOff2026.Game.Data.Entity
{
    public sealed class DateEntity : BaseEntity<DateType>
    {
        public void Reset() => Set(DateType.Day);

        public void Shift()
        {
            var next = value switch
            {
                DateType.Day => DateType.Night,
                DateType.Night => DateType.Day,
                _ => throw new QuitExceptionVO(ExceptionConfig.INVALID_DATE),
            };
            Set(next);
        }
    }
}