using Lofelt.NiceVibrations;

namespace ECR.Infrastructure.Haptic
{
    public class HapticProvider
    {
        private readonly HapticReceiver _hapticReceiver;

        public HapticProvider(HapticReceiver hapticReceiver)
        {
            _hapticReceiver = hapticReceiver;
        }

        public bool HapticsEnabled
        {
            get => HapticController.hapticsEnabled;
            set => HapticController.hapticsEnabled = _hapticReceiver.hapticsEnabled = value;
        }

        public void PlayPreset(HapticPatterns.PresetType type) => 
            HapticPatterns.PlayPreset(type);

        public void PlayConstant(float amplitude, float frequency, float duration = 1f) =>
            HapticPatterns.PlayConstant(amplitude, frequency, duration);
    }
}