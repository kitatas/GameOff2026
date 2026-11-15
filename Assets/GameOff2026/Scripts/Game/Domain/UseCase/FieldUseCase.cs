using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Game.Application;
using GameOff2026.Game.Data.Entity;
using GameOff2026.Game.Domain.Ports;
using GameOff2026.Game.Domain.Repository;
using MessagePipe;

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
                if (field.type.IsEnemy()) SetEnemy(file, rank, fieldSquare);
                _fieldEntity.Add(fieldSquare);
            });

            return PublishRenderAsync(token);
        }

        public UniTask ShiftAsync(CancellationToken token)
        {
            for (int i = 0; i < _fieldEntity.fieldSquares.Length; i++)
            {
                var fieldSquare = _fieldEntity.fieldSquares[i];
                if (!fieldSquare.field.type.IsRoute()) continue;

                var type = fieldSquare.field.type.ShiftRoute();
                var field = _fieldRepository.Find(type);
                _fieldEntity.SetField(i, field);
            }

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

        private void SetEnemy(int file, int rank, FieldSquareVO fieldSquare)
        {
            _enemyEntity.Set(fieldSquare.field.type.ToDate(), CalcPatternIndex(file, rank), fieldSquare.square);
        }

        public UniTask PublishRenderAsync(CancellationToken token)
        {
            var renderFieldSquare = new RenderFieldSquareVO(RenderType.Refresh, _fieldEntity.fieldSquares);
            return _fieldPorts.PublishRenderFieldSquareAsync(renderFieldSquare, token);
        }
    }
}