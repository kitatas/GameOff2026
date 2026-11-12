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
        private readonly FieldEntity _fieldEntity;
        private readonly FieldPatternEntity _fieldPatternEntity;
        private readonly FieldPorts _fieldPorts;
        private readonly FieldRepository _fieldRepository;
        private readonly FieldPatternRepository _fieldPatternRepository;

        public FieldUseCase(DateEntity dateEntity, FieldEntity fieldEntity, FieldPatternEntity fieldPatternEntity,
            FieldPorts fieldPorts, FieldRepository fieldRepository, FieldPatternRepository fieldPatternRepository)
        {
            _dateEntity = dateEntity;
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
            BuildField();

            // 壁沿いマスの算出
            const int length = FieldConfig.MAX_FILE * FieldConfig.MAX_RANK;
            var groundWalls = _fieldEntity.fieldSquares
                .AsValueEnumerable()
                .Skip(length - FieldConfig.MAX_FILE)
                .Take(FieldConfig.MAX_FILE)
                .Select((v, i) => (fieldSquare: v, index: i + length - FieldConfig.MAX_FILE))
                .Where(x => x.fieldSquare.field.type is FieldType.Ground)
                .ToArray();

            // 壁沿いマスからドア位置を決定
            var groundDoor = groundWalls.GetRandom();
            _fieldEntity.SetField(groundDoor.index, _fieldRepository.Find(FieldType.GroundDoor));

            // その他は通常の壁沿いマスに
            foreach (var groundWall in groundWalls)
            {
                if (groundWall.index == groundDoor.index) continue;
                _fieldEntity.SetField(groundWall.index, _fieldRepository.Find(FieldType.GroundWall));
            }

            // 壁
            for (int i = length; i < length + FieldConfig.MAX_FILE; i++)
            {
                var fieldSquare = _fieldEntity.fieldSquares[i - FieldConfig.MAX_FILE];
                var fieldType = fieldSquare.field.type is FieldType.GroundDoor ? FieldType.WallDoor : FieldType.Wall;
                var field = _fieldRepository.Find(fieldType);
                var wallSquare =
                    FieldSquareVO.Create(i % FieldConfig.MAX_FILE + 1, i / FieldConfig.MAX_FILE + 1, field);
                _fieldEntity.Add(wallSquare);
            }

            return PublishRenderAsync(token);
        }

        private void BuildField()
        {
            for (int rank = 0; rank < FieldConfig.MAX_RANK; rank++)
            {
                var patternRankIndex = rank / FieldConfig.HALF_RANK;

                for (int file = 0; file < FieldConfig.MAX_FILE; file++)
                {
                    var patternOffset = (file / FieldConfig.HALF_FILE) * (FieldConfig.MAX_FILE / FieldConfig.HALF_FILE);
                    var typeOffset = (file % FieldConfig.HALF_FILE) * FieldConfig.HALF_RANK;

                    var pattern = _fieldPatternEntity.Get(patternRankIndex + patternOffset);
                    var type = pattern.types[(rank % FieldConfig.HALF_RANK) + typeOffset];

                    var field = _fieldRepository.Find(type);
                    var fieldSquare = FieldSquareVO.Create(file + 1, rank + 1, field);
                    _fieldEntity.Add(fieldSquare);
                }
            }
        }

        public UniTask PublishRenderAsync(CancellationToken token)
        {
            var renderFieldSquare = new RenderFieldSquareVO(RenderType.Refresh, _fieldEntity.fieldSquares);
            return _fieldPorts.PublishRenderFieldSquareAsync(renderFieldSquare, token);
        }
    }
}