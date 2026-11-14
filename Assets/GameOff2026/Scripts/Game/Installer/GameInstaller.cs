using GameOff2026.Game.Data.DataStore;
using GameOff2026.Game.Data.Entity;
using GameOff2026.Game.Domain.Ports;
using GameOff2026.Game.Domain.Repository;
using GameOff2026.Game.Domain.UseCase;
using GameOff2026.Game.Presentation.Facade;
using GameOff2026.Game.Presentation.Presenter;
using GameOff2026.Game.Presentation.State;
using GameOff2026.Game.Presentation.View;
using GameOff2026.Game.Presentation.View.Button;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace GameOff2026.Game.Installer
{
    public sealed class GameInstaller : LifetimeScope
    {
        [SerializeField] private TextAsset memoryFile = default;
        [SerializeField] private FieldTable fieldTable = default;

        protected override void Configure(IContainerBuilder builder)
        {
            // DataStore
            builder.RegisterInstance<MemoryDatabase>(new MemoryDatabase(memoryFile.bytes));
            builder.RegisterInstance<FieldTable>(fieldTable);

            // Entity
            builder.Register<DateEntity>(Lifetime.Scoped);
            builder.Register<EnemyEntity>(Lifetime.Scoped);
            builder.Register<FieldEntity>(Lifetime.Scoped);
            builder.Register<FieldPatternEntity>(Lifetime.Scoped);
            builder.Register<GameStateEntity>(Lifetime.Scoped);
            builder.Register<PlayerEntity>(Lifetime.Scoped);
            builder.Register<TreasureEntity>(Lifetime.Scoped);

            // Ports
            builder.Register<DatePorts>(Lifetime.Scoped);
            builder.Register<EventPorts>(Lifetime.Scoped);
            builder.Register<FieldPorts>(Lifetime.Scoped);
            builder.Register<GameStatePorts>(Lifetime.Scoped);
            builder.Register<MovementPorts>(Lifetime.Scoped);

            // Repository
            builder.Register<FieldPatternRepository>(Lifetime.Scoped);
            builder.Register<FieldRepository>(Lifetime.Scoped);

            // UseCase
            builder.Register<DateUseCase>(Lifetime.Scoped);
            builder.Register<EventUseCase>(Lifetime.Scoped);
            builder.Register<FieldUseCase>(Lifetime.Scoped);
            builder.Register<GameStateUseCase>(Lifetime.Scoped);
            builder.Register<InputUseCase>(Lifetime.Scoped);
            builder.Register<MovementUseCase>(Lifetime.Scoped);
            builder.Register<TreasureUseCase>(Lifetime.Scoped);

            // State
            builder.Register<BaseGameState, GameEventState>(Lifetime.Scoped);
            builder.Register<BaseGameState, GameInitState>(Lifetime.Scoped);
            builder.Register<BaseGameState, GameInputState>(Lifetime.Scoped);
            builder.Register<BaseGameState, GameSetUpState>(Lifetime.Scoped);

            // Presenter
            builder.UseEntryPoints(Lifetime.Scoped, entryPoints =>
            {
                entryPoints.Add<DatePresenter>();
                entryPoints.Add<EventPresenter>();
                entryPoints.Add<FieldPresenter>();
                entryPoints.Add<GameStatePresenter>();
                entryPoints.Add<InputPresenter>();
                entryPoints.Add<MovementPresenter>();
                entryPoints.Add<PlayerPresenter>();
                entryPoints.Add<TreasurePresenter>();
            });

            // Facade
            builder.Register<DateFacade>(Lifetime.Scoped);
            builder.Register<FieldFacade>(Lifetime.Scoped);
            builder.Register<InputFacade>(Lifetime.Scoped);
            builder.Register<MovementFacade>(Lifetime.Scoped);
            builder.Register<PlayerFacade>(Lifetime.Scoped);
            builder.Register<TreasureFacade>(Lifetime.Scoped);

            // View
            builder.RegisterComponentInHierarchy<DateShiftButtonView>();
            builder.RegisterComponentInHierarchy<DateView>();
            builder.RegisterComponentInHierarchy<DoorView>();
            builder.RegisterComponentInHierarchy<FieldView>();
            builder.RegisterComponentInHierarchy<MovementView>();
            builder.RegisterComponentInHierarchy<PlayerView>();
            builder.RegisterComponentInHierarchy<TreasureView>();
        }
    }
}