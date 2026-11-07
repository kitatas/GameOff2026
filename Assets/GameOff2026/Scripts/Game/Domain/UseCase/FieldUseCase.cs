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

        public FieldUseCase(FieldEntity fieldEntity, FieldPorts fieldPorts, FieldRepository fieldRepository)
        {
            _fieldEntity = fieldEntity;
            _fieldPorts = fieldPorts;
            _fieldRepository = fieldRepository;
        }

        public IAsyncSubscriber<RenderFieldSquareVO> renderField => _fieldPorts.renderFieldSquareSubscriber;

        public UniTask BuildFieldAsync(CancellationToken token)
        {
            _fieldEntity.Clear();

            // TODO
            var length = FieldConfig.MAX_FILE * FieldConfig.MAX_RANK;
            for (int i = 0; i < length; i++)
            {
                var type =
                    i == 35 ? FieldType.GroundDoor :
                    i >= 30 ? FieldType.GroundWall :
                    i < 6 ? FieldType.Treasure : FieldType.Ground;
                var field = _fieldRepository.Find(type);
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