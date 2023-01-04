using System.Threading.Tasks;
using ECR.Infrastructure.SceneManagement;
using UnityEngine.ResourceManagement.ResourceProviders;
using Zenject;

namespace ECR.Infrastructure.AssetManagement
{
    public interface IAssetProvider: IInitializable
    {
        public Task<T> Load<T>(string key) where T : class;
        public Task<SceneInstance> LoadScene(SceneName sceneName);
        public void Release(string key);
        public void Cleanup();
    }
}