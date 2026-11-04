using GameOff2026.Game.Data.DataStore;
using GameOff2026.Game.Data.Entity;
using GameOff2026.Game.Domain.Ports;
using GameOff2026.Game.Domain.Repository;
using GameOff2026.Game.Domain.UseCase;
using GameOff2026.Game.Presentation.Facade;
using GameOff2026.Game.Presentation.Presenter;
using GameOff2026.Game.Presentation.State;
using GameOff2026.Game.Presentation.View;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace GameOff2026.Game.Installer
{
    public sealed class GameInstaller : LifetimeScope
    {
        [SerializeField] private FieldTable fieldTable = default;

        protected override void Configure(IContainerBuilder builder)
        {
            // DataStore
            builder.RegisterInstance<FieldTable>(fieldTable);

            // Entity
            builder.Register<FieldEntity>(Lifetime.Scoped);
            builder.Register<GameStateEntity>(Lifetime.Scoped);
            builder.Register<PlayerEntity>(Lifetime.Scoped);

            // Ports
            builder.Register<FieldPorts>(Lifetime.Scoped);
            builder.Register<GameStatePorts>(Lifetime.Scoped);
            builder.Register<MovementPorts>(Lifetime.Scoped);

            // Repository
            builder.Register<FieldRepository>(Lifetime.Scoped);

            // UseCase
            builder.Register<FieldUseCase>(Lifetime.Scoped);
            builder.Register<GameStateUseCase>(Lifetime.Scoped);
            builder.Register<MovementUseCase>(Lifetime.Scoped);

            // State
            builder.Register<BaseGameState, GameInitState>(Lifetime.Scoped);
            builder.Register<BaseGameState, GameInputState>(Lifetime.Scoped);

            // Presenter
            builder.UseEntryPoints(Lifetime.Scoped, entryPoints =>
            {
                entryPoints.Add<GameStatePresenter>();
                entryPoints.Add<MovementPresenter>();
                entryPoints.Add<PlayerPresenter>();
            });

            // Facade
            builder.Register<MovementFacade>(Lifetime.Scoped);
            builder.Register<PlayerFacade>(Lifetime.Scoped);

            // View
            builder.RegisterComponentInHierarchy<MovementView>();
            builder.RegisterComponentInHierarchy<PlayerView>();
        }
    }
}