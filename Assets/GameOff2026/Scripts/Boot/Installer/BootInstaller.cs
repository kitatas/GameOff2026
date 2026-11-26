using GameOff2026.Boot.Data.DataStore;
using GameOff2026.Boot.Data.Entity;
using GameOff2026.Boot.Domain.Ports;
using GameOff2026.Boot.Domain.Repository;
using GameOff2026.Boot.Domain.UseCase;
using GameOff2026.Boot.Presentation.Facade;
using GameOff2026.Boot.Presentation.Presenter;
using GameOff2026.Boot.Presentation.State;
using GameOff2026.Boot.Presentation.View;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace GameOff2026.Boot.Installer
{
    public sealed class BootInstaller : LifetimeScope
    {
        [SerializeField] private SplashTable splashTable = default;

        protected override void Configure(IContainerBuilder builder)
        {
            // DataStore
            builder.RegisterInstance<SplashTable>(splashTable);

            // Entity
            builder.Register<BootStateEntity>(Lifetime.Scoped);

            // Ports
            builder.Register<BootStatePorts>(Lifetime.Scoped);
            builder.Register<SplashPorts>(Lifetime.Scoped);

            // Repository
            builder.Register<SplashRepository>(Lifetime.Scoped);

            // UseCase
            builder.Register<BootStateUseCase>(Lifetime.Scoped);
            builder.Register<LoginUseCase>(Lifetime.Scoped);
            builder.Register<ResourceUseCase>(Lifetime.Scoped);
            builder.Register<SplashUseCase>(Lifetime.Scoped);

            // State
            builder.Register<BaseBootState, BootInitState>(Lifetime.Scoped);
            builder.Register<BaseBootState, BootLoadState>(Lifetime.Scoped);
            builder.Register<BaseBootState, BootLoginState>(Lifetime.Scoped);
            builder.Register<BaseBootState, BootSplashState>(Lifetime.Scoped);

            // Presenter
            builder.UseEntryPoints(Lifetime.Scoped, entryPoints =>
            {
                entryPoints.Add<BootStatePresenter>();
                entryPoints.Add<SplashPresenter>();
            });

            // Facade
            builder.Register<SplashFacade>(Lifetime.Scoped);

            // View
            builder.RegisterComponentInHierarchy<SplashView>();
        }
    }
}