using System;
using System.Threading.Tasks;
using ECR.Infrastructure.AssetManagement;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace ECR.Infrastructure.SceneManagement
{
    public class SceneLoader
    {
        private readonly IAssetProvider _assetProvider;
        
        public SceneLoader(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }
        
        public async Task<SceneInstance> Load(SceneName sceneName, Action<SceneName> onLoaded = null)
        {
            var scene = await _assetProvider.LoadScene(sceneName);
            scene.ActivateAsync();
            
            onLoaded?.Invoke(sceneName);
            return scene;
        }
    }
}