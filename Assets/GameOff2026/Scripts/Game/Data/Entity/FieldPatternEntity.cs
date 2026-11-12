using GameOff2026.Common.Application;
using GameOff2026.Game.Application;
using UniEx;

namespace GameOff2026.Game.Data.Entity
{
    public sealed class FieldPatternEntity
    {
        private readonly FieldPatternVO[] _fieldPatterns;

        public FieldPatternEntity()
        {
            _fieldPatterns = new FieldPatternVO[FieldConfig.LOT_PATTERN_COUNT];
        }

        public void SetUp(FieldPatternVO[] patterns)
        {
            if (patterns.Length != FieldConfig.LOT_PATTERN_COUNT)
                throw new QuitExceptionVO(ExceptionConfig.INVALID_FIELD_LENGTH);

            for (var i = 0; i < patterns.Length; i++) Set(i, patterns[i]);
        }

        public FieldPatternVO Get(int index)
        {
            return _fieldPatterns.TryGetValue(index, out var vo)
                ? vo
                : throw new QuitExceptionVO(ExceptionConfig.INVALID_FIELD_PATTERN);
        }

        public void Set(int index, FieldPatternVO vo)
        {
            _fieldPatterns[index] = vo;
        }
    }
}