using System.Threading.Tasks;
using ECR.Data;
using ECR.Services.PersistentData;
using UnityEngine;
using static Newtonsoft.Json.JsonConvert;

namespace ECR.Services.SaveLoad
{
    public class SaveLoadLocalService : ISaveLoadService
    {
        private const string ProgressKey = "Progress";
        private const string EconomyKey = "Economy";
        private const string SettingsKey = "Settings";

        private readonly IPersistentDataService _persistentDataService;


        public SaveLoadLocalService(IPersistentDataService persistentDataService)
        {
            _persistentDataService = persistentDataService;
            // factories?
        }

        public void SaveProgress()
        {
            var progress = SerializeObject(_persistentDataService.Progress);
            PlayerPrefs.SetString(ProgressKey, progress);
        }

        public Task<PlayerProgressData> LoadProgress()
        {
            var progress = DeserializeObject<PlayerProgressData>(PlayerPrefs.GetString(ProgressKey));
            return Task.FromResult(progress);
        }

        public void SaveEconomy()
        {
            var economy = SerializeObject(_persistentDataService.Economy);
            PlayerPrefs.SetString(EconomyKey, economy);
        }

        public Task<PlayerEconomyData> LoadEconomy()
        {
            var economy = DeserializeObject<PlayerEconomyData>(PlayerPrefs.GetString(EconomyKey));
            return Task.FromResult(economy);
        }

        public void SaveSettings()
        {
            var settings = SerializeObject(_persistentDataService.Settings);
            PlayerPrefs.SetString(SettingsKey, settings);
        }

        public Task<PlayerSettingsData> LoadSettings()
        {
            var settings = DeserializeObject<PlayerSettingsData>(PlayerPrefs.GetString(SettingsKey));
            return Task.FromResult(settings);
        }
    }
}