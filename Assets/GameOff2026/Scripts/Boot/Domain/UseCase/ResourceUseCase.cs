using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Common.Domain.Repository;

namespace GameOff2026.Boot.Domain.UseCase
{
    public sealed class ResourceUseCase
    {
        private readonly LocaleRepository _localeRepository;

        public ResourceUseCase(LocaleRepository localeRepository)
        {
            _localeRepository = localeRepository;
        }

        public UniTask InitAsync(CancellationToken token)
        {
            return _localeRepository.LoadAsync(token);
        }
    }
}