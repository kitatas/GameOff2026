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
        private readonly FieldEntity _fieldEntity;
        private readonly FieldPorts _fieldPorts;
        private readonly FieldRepository _fieldRepository;
        private readonly FieldPatternRepository _fieldPatternRepository;

        public FieldUseCase(FieldEntity fieldEntity, FieldPorts fieldPorts, FieldRepository fieldRepository,
            FieldPatternRepository fieldPatternRepository)
        {
            _fieldEntity = fieldEntity;
            _fieldPorts = fieldPorts;
            _fieldRepository = fieldRepository;
            _fieldPatternRepository = fieldPatternRepository;
        }

        public IAsyncSubscriber<RenderFieldSquareVO> renderField => _fieldPorts.renderFieldSquareSubscriber;

        public UniTask BuildFieldAsync(CancellationToken token)
        {
            _fieldEntity.Clear();

            var patterns = _fieldPatternRepository.GetRandom4();
            for (int rank = 0; rank < FieldConfig.MAX_RANK; rank++)
            {
                var patternRankIndex = rank / FieldConfig.HALF_RANK;

                for (int file = 0; file < FieldConfig.MAX_FILE; file++)
                {
                    var patternOffset = (file / FieldConfig.HALF_FILE) * (FieldConfig.MAX_FILE / FieldConfig.HALF_FILE);
                    var typeOffset = (file % FieldConfig.HALF_FILE) * FieldConfig.HALF_RANK;

                    var pattern = patterns[patternRankIndex + patternOffset];
                    var type = pattern.types[(rank % FieldConfig.HALF_RANK) + typeOffset];

                    var field = _fieldRepository.Find(type);
                    var fieldSquare = FieldSquareVO.Create(file + 1, rank + 1, field);
                    _fieldEntity.Add(fieldSquare);
                }
            }

            // TODO
            var length = FieldConfig.MAX_FILE * FieldConfig.MAX_RANK;
            for (int i = length; i < length + FieldConfig.MAX_FILE; i++)
            {
                var fieldSquare = _fieldEntity.fieldSquares[i - FieldConfig.MAX_FILE];
                var fieldType = fieldSquare.field.type is FieldType.GroundDoor ? FieldType.WallDoor : FieldType.Wall;
                var field = _fieldRepository.Find(fieldType);
                var eventSquare =
                    FieldSquareVO.Create(i % FieldConfig.MAX_FILE + 1, i / FieldConfig.MAX_FILE + 1, field);
                _fieldEntity.Add(eventSquare);
            }

            return PublishRenderAsync(token);
        }

        public UniTask PublishRenderAsync(CancellationToken token)
        {
            var renderFieldSquare = new RenderFieldSquareVO(RenderType.Refresh, _fieldEntity.fieldSquares);
            return _fieldPorts.PublishRenderFieldSquareAsync(renderFieldSquare, token);
        }
    }
}