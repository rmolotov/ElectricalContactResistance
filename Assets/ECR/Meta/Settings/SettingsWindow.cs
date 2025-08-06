using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using Zenject;
using ECR.Data;
using ECR.Infrastructure.Haptic;
using ECR.UI.Windows;

namespace ECR.Meta.Settings
{
    public class SettingsWindow : OneButtonWindow
    {
        [Title("Settings controls")]
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider sfxSlider;
        [SerializeField] private Toggle hapticToggle;
        [SerializeField] private Toggle debugMonitorToggle;

        private HapticProvider _hapticProvider;
        private PlayerSettingsData _userSettings;
        private bool _initialized;

        [Inject]
        private void Construct(HapticProvider hapticProvider)
        {
            _hapticProvider = hapticProvider;
            // TODO:
            // {_soundService} = from windowBase
            // _graphyManager
        }

        public override TaskCompletionSource<bool> InitAndShow<T>(T data, string titleText = "")
        {
            _userSettings = data as PlayerSettingsData;
            
            SetupControls();
            
            return base.InitAndShow(data, titleText);
        }


        private void SetupControls()
        {
            musicSlider.value       = _userSettings.MusicVolume;
            sfxSlider.value         = _userSettings.SfxVolume;
            hapticToggle.isOn       = _userSettings.HapticEnabled;
            debugMonitorToggle.isOn = _userSettings.DebugEnabled;

            if (_initialized == false)
            {
                SubscribeAndInvokeControls();
                _initialized = true;
            }
        }

        private void SubscribeAndInvokeControls()
        {
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
            sfxSlider.onValueChanged.AddListener(SetSfxVolume);
            hapticToggle.onValueChanged.AddListener(EnableHaptic);
            debugMonitorToggle.onValueChanged.AddListener(EnableDebugMonitor);

            SetMusicVolume(musicSlider.value);
            SetSfxVolume(sfxSlider.value);
            EnableDebugMonitor(debugMonitorToggle.isOn);
        }

        private void EnableDebugMonitor(bool value)
        {
            _userSettings.DebugEnabled = value;
            // _graphyManager.gameObject.SetActive(value);
        }

        private void EnableHaptic(bool value)
        {
            _userSettings.HapticEnabled = value;
            _hapticProvider.HapticsEnabled = value;
        }

        private void SetMusicVolume(float value)
        {
            _userSettings.MusicVolume = (byte) value;
            // _soundService.SetMusicVolume(value);
        }

        private void SetSfxVolume(float value)
        {
            _userSettings.SfxVolume = (byte) value;
            // _soundService.SetSfxVolume(value);
        }
    }
}