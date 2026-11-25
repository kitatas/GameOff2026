using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Common.Application;
using GameOff2026.Common.Data.Entity;
using GameOff2026.Common.Domain.Repository;

namespace GameOff2026.Boot.Domain.UseCase
{
    public sealed class LoginUseCase
    {
        private readonly UserEntity _userEntity;
        private readonly SaveRepository _saveRepository;

        public LoginUseCase(UserEntity userEntity, SaveRepository saveRepository)
        {
            _userEntity = userEntity;
            _saveRepository = saveRepository;
        }

        public async UniTask InitAsync(CancellationToken token)
        {
            var save = await _saveRepository.LoadAsync(token);
            var user = new UserVO(save.user);
            _userEntity.Set(user);
        }
    }
}