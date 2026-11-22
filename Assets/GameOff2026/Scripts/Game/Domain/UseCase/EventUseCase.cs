using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Common.Application;
using GameOff2026.Game.Application;
using GameOff2026.Game.Data.Entity;
using GameOff2026.Game.Domain.Ports;
using GameOff2026.Game.Domain.Repository;
using MessagePipe;

namespace GameOff2026.Game.Domain.UseCase
{
    public sealed class EventUseCase
    {
        private readonly FieldEntity _fieldEntity;
        private readonly PlayerEntity _playerEntity;
        private readonly EventPorts _eventPorts;
        private readonly FieldRepository _fieldRepository;

        public EventUseCase(FieldEntity fieldEntity, PlayerEntity playerEntity, EventPorts eventPorts,
            FieldRepository fieldRepository)
        {
            _fieldEntity = fieldEntity;
            _playerEntity = playerEntity;
            _eventPorts = eventPorts;
            _fieldRepository = fieldRepository;
        }

        public IAsyncSubscriber<EventResultVO> eventResult => _eventPorts.eventResultSubscriber;

        public async UniTask<EventResultVO> ApplyAsync(CancellationToken token)
        {
            var (fieldSquare, index) = _fieldEntity.Find(_playerEntity.square);
            await (fieldSquare.field.type switch
            {
                FieldType.Empty => UniTask.Yield(token),
                FieldType.Treasure => OverrideFieldSquareAsync(index, FieldType.Empty, token),
                FieldType.EnemyNight => UniTask.Yield(token),
                FieldType.EnemyRouteActive => UniTask.Yield(token),
                FieldType.EnemyRouteDeactive => UniTask.Yield(token),
                _ => throw new QuitExceptionVO(ExceptionConfig.INVALID_FIELD),
            });

            var result = BuildEventResult(fieldSquare.field.type);
            await _eventPorts.PublishEventResultAsync(result, token);
            return result;
        }

        private UniTask OverrideFieldSquareAsync(int index, FieldType type, CancellationToken token)
        {
            _fieldEntity.SetField(index, _fieldRepository.Find(type), Direction.None);
            return UniTask.Yield(token);
        }

        private static EventResultVO BuildEventResult(FieldType type)
        {
            return new EventResultVO(
                type,
                type is FieldType.Treasure ? 1 : 0
            );
        }
    }
}