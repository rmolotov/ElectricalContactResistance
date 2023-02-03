using System.Threading.Tasks;
using ECR.Data;

namespace ECR.Services.SaveLoad
{
    public interface ISaveLoadService
    {
        void SaveProgress();
        Task<PlayerProgressData> LoadProgress();
        void SaveEconomy();
        Task<PlayerEconomyData> LoadEconomy();
        void SaveSettings();
        Task<PlayerSettingsData> LoadSettings();
    }
}