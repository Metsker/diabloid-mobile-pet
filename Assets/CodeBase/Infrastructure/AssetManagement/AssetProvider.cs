using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CodeBase.Infrastructure.AssetManagement
{
    public class AssetProvider : IAssetProvider
    {
        private readonly Dictionary<string, AsyncOperationHandle> _completedCache = new();
        private readonly Dictionary<string, List<AsyncOperationHandle>> _handles = new();

        public void Instantiate() =>
            Addressables.InitializeAsync();

        public async Task<T> Load<T>(AssetReference assetReference) where T : class
        {
            if (_completedCache.TryGetValue(assetReference.AssetGUID, out AsyncOperationHandle completedHandle))
                return completedHandle.Result as T;

            return await RunWithCashOnComplete(
                Addressables.LoadAssetAsync<T>(assetReference),
                cashKey: assetReference.AssetGUID);
        }

        public async Task<T> Load<T>(string address) where T : class
        {
            if (_completedCache.TryGetValue(address, out AsyncOperationHandle completedHandle))
                return completedHandle.Result as T;

            return await RunWithCashOnComplete(
                Addressables.LoadAssetAsync<T>(address),
                cashKey: address);
        }

        public Task<GameObject> Instantiate(string address, Vector3 at) =>
            Addressables.InstantiateAsync(address, at, Quaternion.identity).Task;

        public Task<GameObject> Instantiate(string address) =>
            Addressables.InstantiateAsync(address).Task;

        public void Cleanup()
        {
            foreach (var resourceHandles in _handles.Values)
                foreach (var handle in resourceHandles)
                    Addressables.Release(handle);
            
            _completedCache.Clear();
            _handles.Clear();
        }

        private async Task<T> RunWithCashOnComplete<T>(AsyncOperationHandle<T> handle, string cashKey) where T : class
        {
            handle.Completed += operationHandle =>
                _completedCache[cashKey] = operationHandle;

            AddHandle(cashKey, handle);

            return await handle.Task;
        }

        private void AddHandle<T>(string key, AsyncOperationHandle<T> handle) where T : class
        {
            if (!_handles.TryGetValue(key, out List<AsyncOperationHandle> resourceHandle))
            {
                resourceHandle = new List<AsyncOperationHandle>();
                _handles[key] = resourceHandle;
            }
            resourceHandle.Add(handle);
        }
    }
}