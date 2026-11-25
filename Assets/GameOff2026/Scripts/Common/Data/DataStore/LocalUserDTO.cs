using System;
using GameOff2026.Common.Application;

namespace GameOff2026.Common.Data.DataStore
{
    [Serializable]
    public sealed class LocalUserDTO
    {
        public string id;

        public LocalUserDTO()
        {
            id = "";
        }

        public LocalUserDTO(LocalUserVO user)
        {
            id = user.id;
        }

        public LocalUserVO ToVO() => new(id);
    }
}