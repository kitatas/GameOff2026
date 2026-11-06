using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Common.Application;
using GameOff2026.Game.Application;
using GameOff2026.Game.Data.Entity;
using GameOff2026.Game.Domain.Repository;

namespace GameOff2026.Game.Domain.UseCase
{
    public sealed class EventUseCase
    {
        private readonly FieldEntity _fieldEntity;
        private readonly PlayerEntity _playerEntity;
        private readonly FieldRepository _fieldRepository;

        public EventUseCase(FieldEntity fieldEntity, PlayerEntity playerEntity, FieldRepository fieldRepository)
        {
            _fieldEntity = fieldEntity;
            _playerEntity = playerEntity;
            _fieldRepository = fieldRepository;
        }

        public UniTask ApplyAsync(CancellationToken token)
        {
            var (fieldSquare, index) = _fieldEntity.Find(_playerEntity.square);
            return (fieldSquare.field.type switch
            {
                FieldType.Ground => UniTask.Yield(token),
                FieldType.Treasure => OverrideFieldSquareAsync(index, FieldType.Ground, token),
                _ => throw new QuitExceptionVO(ExceptionConfig.INVALID_FIELD),
            });
        }

        private UniTask OverrideFieldSquareAsync(int index, FieldType type, CancellationToken token)
        {
            _fieldEntity.SetField(index, _fieldRepository.Find(type));
            return UniTask.Yield(token);
        }
    }
}