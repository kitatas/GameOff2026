using System;
using Cysharp.Threading.Tasks;
using GameOff2026.Game.Domain.UseCase;
using MessagePipe;
using R3;
using VContainer.Unity;

namespace GameOff2026.Game.Presentation.Presenter
{
    public sealed class EventPresenter : IStartable, IDisposable
    {
        private readonly EventUseCase _eventUseCase;
        private readonly TreasureUseCase _treasureUseCase;
        private readonly CompositeDisposable _disposable;

        public EventPresenter(EventUseCase eventUseCase, TreasureUseCase treasureUseCase)
        {
            _eventUseCase = eventUseCase;
            _treasureUseCase = treasureUseCase;
            _disposable = new CompositeDisposable();
        }

        void IStartable.Start()
        {
            _eventUseCase.eventResult
                .Subscribe((x, _) =>
                {
                    _treasureUseCase.Add(x.treasureNum);
                    return UniTask.CompletedTask;
                }, x => x.treasureNum > 0)
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}