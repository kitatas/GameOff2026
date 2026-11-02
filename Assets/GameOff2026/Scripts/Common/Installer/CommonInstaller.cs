using GameOff2026.Common.Data.Entity;
using GameOff2026.Common.Domain.Ports;
using GameOff2026.Common.Domain.UseCase;
using GameOff2026.Common.Presentation.Facade;
using GameOff2026.Common.Presentation.Presenter;
using GameOff2026.Common.Presentation.View;
using GameOff2026.Common.Utility;
using MessagePipe;
using VContainer;
using VContainer.Unity;

namespace GameOff2026.Common.Installer
{
    public sealed class CommonInstaller : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterMessagePipe();

            // Entity
            builder.Register<RetryCountEntity>(Lifetime.Singleton);

            // Ports
            builder.Register<ExceptionPorts>(Lifetime.Singleton);

            // UseCase
            builder.Register<ExceptionUseCase>(Lifetime.Singleton);
            builder.Register<SceneUseCase>(Lifetime.Singleton);

            // Presenter
            builder.UseEntryPoints(Lifetime.Singleton, entryPoints =>
            {
                entryPoints.Add<ExceptionPresenter>();
                entryPoints.Add<ScenePresenter>();
            });

            // Facade
            builder.Register<ExceptionFacade>(Lifetime.Singleton);
            builder.Register<SceneFacade>(Lifetime.Singleton);

            // View
            builder.RegisterFindFirstObjectByType<ExceptionView>();
            builder.RegisterFindFirstObjectByType<TransitionView>();
        }
    }
}