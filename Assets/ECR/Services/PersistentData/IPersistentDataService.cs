using ECR.Data;

namespace ECR.Services.PersistentData
{
    public interface IPersistentDataService
    {
        PlayerSettingsData Settings { get; set; }
        PlayerProgressData Progress { get; set; }
    }
}