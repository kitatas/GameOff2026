using GameOff2026.Common.Data.Entity;
using GameOff2026.Common.Domain.Ports;
using GameOff2026.Common.Domain.Repository;
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
            builder.Register<UserEntity>(Lifetime.Singleton);

            // Ports
            builder.Register<ExceptionPorts>(Lifetime.Singleton);
            builder.Register<LoadingPorts>(Lifetime.Singleton);

            // Repository
            builder.Register<CriSoundRepository>(Lifetime.Singleton).As<ISoundRepository>();
            builder.Register<LocaleRepository>(Lifetime.Singleton);
            builder.Register<SaveRepository>(Lifetime.Singleton);

            // UseCase
            builder.Register<BgmUseCase>(Lifetime.Singleton);
            builder.Register<ExceptionUseCase>(Lifetime.Singleton);
            builder.Register<LoadingUseCase>(Lifetime.Singleton);
            builder.Register<SceneUseCase>(Lifetime.Singleton);
            builder.Register<SeUseCase>(Lifetime.Singleton);

            // Presenter
            builder.UseEntryPoints(Lifetime.Singleton, entryPoints =>
            {
                entryPoints.Add<BgmPresenter>();
                entryPoints.Add<ExceptionPresenter>();
                entryPoints.Add<LoadingPresenter>();
                entryPoints.Add<ScenePresenter>();
                entryPoints.Add<SePresenter>();
            });

            // Facade
            builder.Register<BgmFacade>(Lifetime.Singleton);
            builder.Register<ExceptionFacade>(Lifetime.Singleton);
            builder.Register<LoadingFacade>(Lifetime.Singleton);
            builder.Register<SceneFacade>(Lifetime.Singleton);
            builder.Register<SeFacade>(Lifetime.Singleton);

            // View
            builder.RegisterFindFirstObjectByType<CriSoundView>().As<ISoundView>();
            builder.RegisterFindFirstObjectByType<ExceptionView>();
            builder.RegisterFindFirstObjectByType<LoadingView>();
            builder.RegisterFindFirstObjectByType<TransitionView>();
        }
    }
}