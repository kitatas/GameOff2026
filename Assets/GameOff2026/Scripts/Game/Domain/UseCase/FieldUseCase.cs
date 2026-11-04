using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Game.Application;
using GameOff2026.Game.Data.Entity;
using GameOff2026.Game.Domain.Ports;
using GameOff2026.Game.Domain.Repository;

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

        public UniTask BuildFieldAsync(CancellationToken token)
        {
            _fieldEntity.Clear();

            // TODO
            for (int i = 0; i < FieldConfig.MAX_FILE * FieldConfig.MAX_RANK; i++)
            {
                var field = _fieldRepository.Find(FieldType.Ground);
                var eventSquare =
                    FieldSquareVO.Create(i % FieldConfig.MAX_FILE + 1, i / FieldConfig.MAX_FILE + 1, field);
                _fieldEntity.Add(eventSquare);
            }

            var renderFieldSquare = new RenderFieldSquareVO(RenderType.Refresh, _fieldEntity.fieldSquares);
            return _fieldPorts.PublishRenderFieldSquareAsync(renderFieldSquare, token);
        }
    }
}