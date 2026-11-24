using GameOff2026.Boot.Data.Entity;
using GameOff2026.Boot.Domain.Ports;
using GameOff2026.Boot.Domain.UseCase;
using GameOff2026.Boot.Presentation.Presenter;
using GameOff2026.Boot.Presentation.State;
using VContainer;
using VContainer.Unity;

namespace GameOff2026.Boot.Installer
{
    public sealed class BootInstaller : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // Entity
            builder.Register<BootStateEntity>(Lifetime.Scoped);

            // Ports
            builder.Register<BootStatePorts>(Lifetime.Scoped);

            // UseCase
            builder.Register<BootStateUseCase>(Lifetime.Scoped);

            // State
            builder.Register<BaseBootState, BootInitState>(Lifetime.Scoped);

            // Presenter
            builder.UseEntryPoints(Lifetime.Scoped, entryPoints =>
            {
                entryPoints.Add<BootStatePresenter>();
            });
        }
    }
}