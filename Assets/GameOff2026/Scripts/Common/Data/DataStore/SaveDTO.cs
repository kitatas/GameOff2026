using GameOff2026.Common.Application;

namespace GameOff2026.Common.Data.DataStore
{
    public sealed class SaveDTO
    {
        public LocalUserDTO user;

        public SaveDTO()
        {
            user = new LocalUserDTO();
        }

        public SaveVO ToVO() => new(user.ToVO());
    }
}