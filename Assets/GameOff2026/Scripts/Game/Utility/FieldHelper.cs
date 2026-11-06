using GameOff2026.Game.Application;

namespace GameOff2026.Game.Utility
{
    public static class FieldHelper
    {
        public static SquareVO AddDirection(this SquareVO self, Direction direction)
        {
            var dir = direction.ToSquareVO();
            return new SquareVO(self.file + dir.file, self.rank + dir.rank);
        }
    }
}