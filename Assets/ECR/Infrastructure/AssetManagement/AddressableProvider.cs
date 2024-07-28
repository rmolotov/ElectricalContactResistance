using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ECR.Infrastructure.SceneManagement;
using ECR.Services.Logging;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace ECR.Infrastructure.AssetManagement
{
    public class AddressableProvider : IAssetProvider
    {
        private readonly ILoggingService _loggingService;
        private readonly Dictionary<string, AsyncOperationHandle> _completedCache = new();
        private readonly Dictionary<string, List<AsyncOperationHandle>> _handles = new();

        public AddressableProvider(ILoggingService loggingService) => 
            _loggingService = loggingService;

        public async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            await Addressables.InitializeAsync().Task;
            _loggingService.LogMessage("Addressables initialized", this);
        }

        public async Task<T> Load<T>(AssetReference assetReference, CancellationToken cancellationToken) where T : class
        {
            return await Load<T>(
                key: assetReference.AssetGUID,
                cancellationToken: cancellationToken);
        }

        public async Task<T> Load<T>(string key, CancellationToken cancellationToken) where T : class
        {
            if (_completedCache.TryGetValue(key, out var completedHandle))
                return completedHandle.Result as T;

            cancellationToken.ThrowIfCancellationRequested();
            var handle = Addressables.LoadAssetAsync<T>(key);

            cancellationToken.ThrowIfCancellationRequested();
            return await RunWithCacheOnComplete(
                handle: handle,
                cacheKey: key, 
                token: cancellationToken);
        }

        public async Task<SceneInstance> LoadScene(SceneName sceneName, CancellationToken cancellationToken)
        {
            var operationHandle = Addressables.LoadSceneAsync(sceneName.ToSceneString());

            return await operationHandle.Task;
        }

        public void Release(AssetReference assetReference)
        {
            Release(assetReference.AssetGUID);
        }

        public void Release(string key)
        {
            if (!_handles.TryGetValue(key, out var handlesForKey))
                return;
            
            foreach (var handle in handlesForKey)
                Addressables.Release(handle);

            _completedCache.Remove(key);
            _handles.Remove(key);
        }

        public void Cleanup()
        {
            if (_handles.Count == 0)
                return;
             
            foreach (List<AsyncOperationHandle> resourceHandles in _handles.Values)
                foreach (AsyncOperationHandle handle in resourceHandles)
                    Addressables.Release(handle);
      
            _completedCache.Clear();
            _handles.Clear();
        }

        
        private async Task<T> RunWithCacheOnComplete<T>(
            AsyncOperationHandle<T> handle, 
            string cacheKey, 
            CancellationToken token) where T : class
        {
            handle.Completed += completeHandle => 
                _completedCache[cacheKey] = completeHandle;

            token.ThrowIfCancellationRequested();
            AddHandle(cacheKey, handle);

            var result = await handle.Task;
            token.ThrowIfCancellationRequested();

            return result;
        }

        private void AddHandle(string key, AsyncOperationHandle handle)
        {
            if (!_handles.TryGetValue(key, out var resourceHandles))
            {
                resourceHandles = new List<AsyncOperationHandle>();
                _handles[key] = resourceHandles;
            }
            resourceHandles.Add(handle);
        }
    }
}