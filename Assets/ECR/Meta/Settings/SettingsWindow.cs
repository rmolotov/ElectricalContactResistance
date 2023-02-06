using ECR.Data;
using ECR.UI;
using ECR.UI.Windows;
using RSG;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ECR.Meta.Settings
{
    public class SettingsWindow : OneButtonWindow
    {
        [Title("Settings controls")]
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider sfxSlider;
        [SerializeField] private Toggle hapticToggle;
        [SerializeField] private Toggle debugMonitorToggle;

        private PlayerSettingsData _userSettings;

        [Inject]
        private void Construct()
        {
            // {_soundService, _hapticService} = from windowBase
            // _graphyManager
        }

        public override Promise<bool> InitAndShow<T>(T data, string titleText = "")
        {
            _userSettings = data as PlayerSettingsData;
            
            SetupControls();
            
            return base.InitAndShow(data, titleText);
        }


        private void SetupControls()
        {
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
            sfxSlider.onValueChanged.AddListener(SetSfxVolume);
            hapticToggle.onValueChanged.AddListener(EnableHaptic);
            debugMonitorToggle.onValueChanged.AddListener(EnableDebugMonitor);

            musicSlider.value       = _userSettings.MusicVolume;
            sfxSlider.value         = _userSettings.SfxVolume;
            hapticToggle.isOn       = _userSettings.HapticEnabled;
            debugMonitorToggle.isOn = _userSettings.DebugEnabled;
        }

        private void EnableDebugMonitor(bool value)
        {
            _userSettings.DebugEnabled = value;
            // _graphyManager.gameObject.SetActive(value);
        }

        private void EnableHaptic(bool value)
        {
            _userSettings.HapticEnabled = value;
            // _hapticService.enabled = value
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