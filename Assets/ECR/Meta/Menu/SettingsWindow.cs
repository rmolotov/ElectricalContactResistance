using ECR.StaticData;
using ECR.UI;
using RSG;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace ECR.Meta.Menu
{
    public class SettingsWindow : OneButtonWindow
    {
        [Title("Settings controls")]
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider sfxSlider;
        [SerializeField] private Toggle hapticToggle;
        [SerializeField] private Toggle debugMonitorToggle;

        protected override void Construct()
        {
            // _soundService, _hapticService, _graphyManager, progressService
            print("settings window constructed");
            base.Construct();
        }

        public override Promise<bool> InitAndShow<T>(T data, string titleText = "")
        {
            var userData = data as PlayerSettingsStaticData;
            
            SetupControls(userData);
            
            return base.InitAndShow(data, titleText);
        }
        

        private void SetupControls(PlayerSettingsStaticData data)
        {
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
            sfxSlider.onValueChanged.AddListener(SetSfxVolume);
            hapticToggle.onValueChanged.AddListener(EnableHaptic);
            debugMonitorToggle.onValueChanged.AddListener(EnableDebugMonitor);

            musicSlider.value = data.MusicVolume / 100f;
            sfxSlider.value = data.SfxVolume / 100f;
            hapticToggle.isOn = data.HapticEnabled;
            debugMonitorToggle.isOn = data.DebugEnabled;
        }

        private void EnableDebugMonitor(bool value)
        {
            // _graphyManager.gameObject.SetActive(value);
        }

        private void EnableHaptic(bool value)
        {
            // _hapticService.enabled = value
        }

        private void SetMusicVolume(float value)
        {
            // _soundService.SetMusicVolume(value);
        }

        private void SetSfxVolume(float value)
        {
            // _soundService.SetSfxVolume(value);
        }
    }
}