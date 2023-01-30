namespace ECR.StaticData
{
    public record PlayerSettingsStaticData
    {
        public byte MusicVolume { get; set; }
        public byte SfxVolume { get; set; }
        public bool HapticEnabled { get; set; }
        public bool DebugEnabled { get; set; }   
    }
}