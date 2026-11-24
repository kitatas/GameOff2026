using System.Collections.Generic;
using GameOff2026.Boot.Application;
using GameOff2026.Boot.Domain.UseCase;
using GameOff2026.Boot.Presentation.State;
using GameOff2026.Common.Domain.UseCase;
using GameOff2026.Common.Presentation.Presenter;

namespace GameOff2026.Boot.Presentation.Presenter
{
    public sealed class BootStatePresenter : BaseStatePresenter<BootState>
    {
        public BootStatePresenter(ExceptionUseCase exceptionUseCase, BootStateUseCase stateUseCase,
            IEnumerable<BaseBootState> states) : base(exceptionUseCase, stateUseCase, states)
        {
        }
    }
}