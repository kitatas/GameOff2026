using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Game.Application;
using R3;

namespace GameOff2026.Game.Domain.UseCase
{
    public sealed class InputUseCase : IDisposable
    {
        private readonly Subject<InputVO> _input;

        public InputUseCase()
        {
            _input = new Subject<InputVO>();
        }

        public void HandleInput(InputVO input)
        {
            _input?.OnNext(input);
        }

        public async UniTask<InputVO> InputAsync(CancellationToken token)
        {
            return await _input.FirstAsync(cancellationToken: token);
        }

        void IDisposable.Dispose()
        {
            _input?.Dispose();
        }
    }
}