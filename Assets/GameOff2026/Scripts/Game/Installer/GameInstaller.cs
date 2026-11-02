using GameOff2026.Game.Data.Entity;
using GameOff2026.Game.Domain.Ports;
using GameOff2026.Game.Domain.UseCase;
using GameOff2026.Game.Presentation.Presenter;
using GameOff2026.Game.Presentation.State;
using VContainer;
using VContainer.Unity;

namespace GameOff2026.Game.Installer
{
    public sealed class GameInstaller : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // Entity
            builder.Register<GameStateEntity>(Lifetime.Scoped);

            // Ports
            builder.Register<GameStatePorts>(Lifetime.Scoped);

            // UseCase
            builder.Register<GameStateUseCase>(Lifetime.Scoped);

            // State
            builder.Register<BaseGameState, GameInitState>(Lifetime.Scoped);

            // Presenter
            builder.UseEntryPoints(Lifetime.Scoped, entryPoints =>
            {
                entryPoints.Add<GameStatePresenter>();
            });
        }
    }
}