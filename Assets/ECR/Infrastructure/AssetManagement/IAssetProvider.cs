using System.Threading;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using ECR.Infrastructure.SceneManagement;
using ECR.Services.Interfaces;

namespace ECR.Infrastructure.AssetManagement
{
    public interface IAssetProvider: IInitializableAsync
    {
        public Task<T> Load<T>(string key, CancellationToken cancellationToken = default) where T : class;
        public Task<T> Load<T>(AssetReference assetReference, CancellationToken cancellationToken = default) where T : class;
        
        public Task<SceneInstance> LoadScene(SceneName sceneName, CancellationToken cancellationToken = default);

        public void Release(string key);
        public void Release(AssetReference assetReference);
        
        public void Cleanup();
    }
}