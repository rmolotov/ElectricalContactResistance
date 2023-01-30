using System;

namespace ECR.Data
{
    [Serializable]
    public class PlayerSettingsData
    {
        public byte MusicVolume { get; set; }
        public byte SfxVolume { get; set; }
        public bool HapticEnabled { get; set; }
        public bool DebugEnabled { get; set; }   
    }
}