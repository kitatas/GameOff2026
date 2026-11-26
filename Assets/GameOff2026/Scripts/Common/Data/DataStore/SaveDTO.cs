using GameOff2026.Common.Application;

namespace GameOff2026.Common.Data.DataStore
{
    public sealed class SaveDTO
    {
        public LocalUserDTO user;
        public VolumeDTO masterVolume;
        public VolumeDTO bgmVolume;
        public VolumeDTO seVolume;

        public SaveDTO()
        {
            user = new LocalUserDTO();
            masterVolume = new VolumeDTO();
            bgmVolume = new VolumeDTO();
            seVolume = new VolumeDTO();
        }

        public static SaveDTO Recreate(SaveDTO dto)
        {
            return new SaveDTO
            {
                user = new LocalUserDTO(),
                masterVolume = dto.masterVolume,
                bgmVolume = dto.bgmVolume,
                seVolume = dto.seVolume,
            };
        }

        public SaveVO ToVO() => new(user.ToVO(), masterVolume.ToVO(), bgmVolume.ToVO(), seVolume.ToVO());
    }
}