using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Game.Application;
using GameOff2026.Game.Data.Entity;
using GameOff2026.Game.Domain.Ports;
using GameOff2026.Game.Domain.Repository;
using MessagePipe;
using UniEx;
using ZLinq;

namespace GameOff2026.Game.Domain.UseCase
{
    public sealed class FieldUseCase
    {
        private readonly DateEntity _dateEntity;
        private readonly EnemyEntity _enemyEntity;
        private readonly FieldEntity _fieldEntity;
        private readonly FieldPatternEntity _fieldPatternEntity;
        private readonly FieldPorts _fieldPorts;
        private readonly FieldRepository _fieldRepository;
        private readonly FieldPatternRepository _fieldPatternRepository;

        public FieldUseCase(DateEntity dateEntity, EnemyEntity enemyEntity, FieldEntity fieldEntity,
            FieldPatternEntity fieldPatternEntity, FieldPorts fieldPorts, FieldRepository fieldRepository,
            FieldPatternRepository fieldPatternRepository)
        {
            _dateEntity = dateEntity;
            _enemyEntity = enemyEntity;
            _fieldEntity = fieldEntity;
            _fieldPatternEntity = fieldPatternEntity;
            _fieldPorts = fieldPorts;
            _fieldRepository = fieldRepository;
            _fieldPatternRepository = fieldPatternRepository;
        }

        public IAsyncSubscriber<RenderFieldSquareVO> renderField => _fieldPorts.renderFieldSquareSubscriber;

        public UniTask BuildFieldAsync(CancellationToken token)
        {
            _fieldEntity.Clear();

            var patterns = _fieldPatternRepository.GetRandom4(_dateEntity.value);
            _fieldPatternEntity.SetUp(patterns);
            BuildField((file, rank, field) =>
            {
                var fieldSquare = FieldSquareVO.Create(file + 1, rank + 1, field);
                if (field.type.IsEnemy()) SetEnemy(file, rank, fieldSquare.square);
                _fieldEntity.Add(fieldSquare);
            });

            return PublishRenderAsync(token);
        }

        public UniTask ShiftAsync(CancellationToken token)
        {
            var patterns = Enumerable.Range(0, FieldConfig.LOT_PATTERN_COUNT)
                .AsValueEnumerable()
                .Select(x => _fieldPatternEntity.Get(x))
                .Select(x => _fieldPatternRepository.Get(x.id, x.date.Shift(), x.rotation))
                .ToArray();
            _fieldPatternEntity.SetUp(patterns);

            BuildField((file, rank, field) =>
            {
                var index = rank * FieldConfig.MAX_FILE + file;
                if (field.type.IsEnemy()) SetEnemy(file, rank, _fieldEntity.fieldSquares[index].square);
                _fieldEntity.SetField(index, field);
            });

            return PublishRenderAsync(token);
        }

        private static int CalcPatternIndex(int file, int rank)
        {
            var patternRankIndex = rank / FieldConfig.HALF_RANK;
            var patternOffset = (file / FieldConfig.HALF_FILE) * (FieldConfig.MAX_FILE / FieldConfig.HALF_FILE);
            return patternRankIndex + patternOffset;
        }

        private void BuildField(Action<int, int, FieldVO> action)
        {
            for (int rank = 0; rank < FieldConfig.MAX_RANK; rank++)
            {
                for (int file = 0; file < FieldConfig.MAX_FILE; file++)
                {
                    var typeOffset = (file % FieldConfig.HALF_FILE) * FieldConfig.HALF_RANK;
                    var pattern = _fieldPatternEntity.Get(CalcPatternIndex(file, rank));
                    var type = pattern.types[(rank % FieldConfig.HALF_RANK) + typeOffset];

                    var field = _fieldRepository.Find(type);
                    action?.Invoke(file, rank, field);
                }
            }
        }

        private void SetEnemy(int file, int rank, SquareVO square)
        {
            _enemyEntity.Set(_dateEntity.value, CalcPatternIndex(file, rank), square);
        }

        public UniTask PublishRenderAsync(CancellationToken token)
        {
            var renderFieldSquare = new RenderFieldSquareVO(RenderType.Refresh, _fieldEntity.fieldSquares);
            return _fieldPorts.PublishRenderFieldSquareAsync(renderFieldSquare, token);
        }
    }
}