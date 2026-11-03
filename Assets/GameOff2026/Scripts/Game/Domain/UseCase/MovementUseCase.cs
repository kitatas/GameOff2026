using GameOff2026.Game.Application;
using GameOff2026.Game.Domain.Ports;
using MessagePipe;

namespace GameOff2026.Game.Domain.UseCase
{
    public sealed class MovementUseCase
    {
        private readonly MovementPorts _movementPorts;

        public MovementUseCase(MovementPorts movementPorts)
        {
            _movementPorts = movementPorts;
        }

        public IAsyncSubscriber<MovementVO> movement => _movementPorts.movementSubscriber;
    }
}