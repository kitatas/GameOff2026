using GameOff2026.Common.Utility;
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
using GameOff2026.Game.Presentation.View.Modal;
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
            builder.Register<DayEntity>(Lifetime.Scoped);
            builder.Register<EnemyEntity>(Lifetime.Scoped);
            builder.Register<FieldEntity>(Lifetime.Scoped);
            builder.Register<FieldPatternEntity>(Lifetime.Scoped);
            builder.Register<FloorEntity>(Lifetime.Scoped);
            builder.Register<GameModalEntity>(Lifetime.Scoped);
            builder.Register<GameStateEntity>(Lifetime.Scoped);
            builder.Register<MaxStaminaEntity>(Lifetime.Scoped);
            builder.Register<MoneyEntity>(Lifetime.Scoped);
            builder.Register<PlayerEntity>(Lifetime.Scoped);
            builder.Register<PurchasedItemEntity>(Lifetime.Scoped);
            builder.Register<ShopPriceRateEntity>(Lifetime.Scoped);
            builder.Register<StaminaEntity>(Lifetime.Scoped);
            builder.Register<TreasureEntity>(Lifetime.Scoped);
            builder.Register<TreasurePriceRateEntity>(Lifetime.Scoped);

            // Ports
            builder.Register<DatePorts>(Lifetime.Scoped);
            builder.Register<EventPorts>(Lifetime.Scoped);
            builder.Register<FieldPorts>(Lifetime.Scoped);
            builder.Register<FinishPorts>(Lifetime.Scoped);
            builder.Register<GameModalPorts>(Lifetime.Scoped);
            builder.Register<GameStatePorts>(Lifetime.Scoped);
            builder.Register<HudRootPorts>(Lifetime.Scoped);
            builder.Register<MovementPorts>(Lifetime.Scoped);
            builder.Register<ShopPorts>(Lifetime.Scoped);
            builder.Register<TapScreenPorts>(Lifetime.Scoped);

            // Repository
            builder.Register<FieldPatternRepository>(Lifetime.Scoped);
            builder.Register<FieldRepository>(Lifetime.Scoped);
            builder.Register<ShopItemRepository>(Lifetime.Scoped);
            builder.Register<TreasurePriceRepository>(Lifetime.Scoped);

            // UseCase
            builder.Register<DateUseCase>(Lifetime.Scoped);
            builder.Register<DayUseCase>(Lifetime.Scoped);
            builder.Register<EnemyWanderUseCase>(Lifetime.Scoped);
            builder.Register<EventUseCase>(Lifetime.Scoped);
            builder.Register<FieldUseCase>(Lifetime.Scoped);
            builder.Register<FinishUseCase>(Lifetime.Scoped);
            builder.Register<FloorUseCase>(Lifetime.Scoped);
            builder.Register<GameModalUseCase>(Lifetime.Scoped);
            builder.Register<GameStateUseCase>(Lifetime.Scoped);
            builder.Register<HudRootUseCase>(Lifetime.Scoped);
            builder.Register<InputUseCase>(Lifetime.Scoped);
            builder.Register<ItemUseCase>(Lifetime.Scoped);
            builder.Register<MoneyUseCase>(Lifetime.Scoped);
            builder.Register<MovementUseCase>(Lifetime.Scoped);
            builder.Register<PlayerUseCase>(Lifetime.Scoped);
            builder.Register<ShopUseCase>(Lifetime.Scoped);
            builder.Register<StaminaUseCase>(Lifetime.Scoped);
            builder.Register<TapScreenUseCase>(Lifetime.Scoped);
            builder.Register<TreasureUseCase>(Lifetime.Scoped);

            // State
            builder.Register<BaseGameState, GameClearState>(Lifetime.Scoped);
            builder.Register<BaseGameState, GameEventState>(Lifetime.Scoped);
            builder.Register<BaseGameState, GameFailState>(Lifetime.Scoped);
            builder.Register<BaseGameState, GameFinishState>(Lifetime.Scoped);
            builder.Register<BaseGameState, GameInitState>(Lifetime.Scoped);
            builder.Register<BaseGameState, GameInputState>(Lifetime.Scoped);
            builder.Register<BaseGameState, GameReturnState>(Lifetime.Scoped);
            builder.Register<BaseGameState, GameSendState>(Lifetime.Scoped);
            builder.Register<BaseGameState, GameSetUpState>(Lifetime.Scoped);
            builder.Register<BaseGameState, GameShopState>(Lifetime.Scoped);
            builder.Register<BaseGameState, GameWanderState>(Lifetime.Scoped);

            // Presenter
            builder.UseEntryPoints(Lifetime.Scoped, entryPoints =>
            {
                entryPoints.Add<DatePresenter>();
                entryPoints.Add<DayPresenter>();
                entryPoints.Add<EventPresenter>();
                entryPoints.Add<FieldPresenter>();
                entryPoints.Add<FinishPresenter>();
                entryPoints.Add<FloorPresenter>();
                entryPoints.Add<GameModalPresenter>();
                entryPoints.Add<GameStatePresenter>();
                entryPoints.Add<HudRootPresenter>();
                entryPoints.Add<InputPresenter>();
                entryPoints.Add<MoneyPresenter>();
                entryPoints.Add<MovementPresenter>();
                entryPoints.Add<PlayerPresenter>();
                entryPoints.Add<ShopPresenter>();
                entryPoints.Add<StaminaPresenter>();
                entryPoints.Add<TapScreenPresenter>();
                entryPoints.Add<TreasurePresenter>();
            });

            // Facade
            builder.Register<DateFacade>(Lifetime.Scoped);
            builder.Register<DayFacade>(Lifetime.Scoped);
            builder.Register<FieldFacade>(Lifetime.Scoped);
            builder.Register<FinishFacade>(Lifetime.Scoped);
            builder.Register<FloorFacade>(Lifetime.Scoped);
            builder.Register<GameModalFacade>(Lifetime.Scoped);
            builder.Register<HudRootFacade>(Lifetime.Scoped);
            builder.Register<InputFacade>(Lifetime.Scoped);
            builder.Register<MoneyFacade>(Lifetime.Scoped);
            builder.Register<MovementFacade>(Lifetime.Scoped);
            builder.Register<PlayerFacade>(Lifetime.Scoped);
            builder.Register<ShopFacade>(Lifetime.Scoped);
            builder.Register<StaminaFacade>(Lifetime.Scoped);
            builder.Register<TapScreenFacade>(Lifetime.Scoped);
            builder.Register<TreasureFacade>(Lifetime.Scoped);

            // View
            builder.RegisterFindObjectsByType<GameModalButtonView>();
            builder.RegisterFindObjectsByType<BaseGameModalView>();
            builder.RegisterComponentInHierarchy<ClearView>();
            builder.RegisterComponentInHierarchy<DateShiftButtonView>();
            builder.RegisterComponentInHierarchy<DateView>();
            builder.RegisterComponentInHierarchy<DayView>();
            builder.RegisterComponentInHierarchy<FailView>();
            builder.RegisterComponentInHierarchy<FieldView>();
            builder.RegisterComponentInHierarchy<FloorView>();
            builder.RegisterComponentInHierarchy<HudRootView>();
            builder.RegisterComponentInHierarchy<MoneyView>();
            builder.RegisterComponentInHierarchy<MovementView>();
            builder.RegisterComponentInHierarchy<PlayerView>();
            builder.RegisterComponentInHierarchy<ShopView>();
            builder.RegisterComponentInHierarchy<StaminaView>();
            builder.RegisterComponentInHierarchy<TapScreenView>();
            builder.RegisterComponentInHierarchy<TreasureView>();
        }
    }
}