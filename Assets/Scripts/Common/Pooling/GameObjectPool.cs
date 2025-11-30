using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Common.Pooling
{
    public class GameObjectPool<T> : MonoBehaviour where T : Component
    {
        [SerializeField] private int defaultCapacity = 16;
        [SerializeField] private int maxSize = 150;
        
        private IObjectPool<T> _pool;
        private AsyncOperationHandle<GameObject> _loadHandle;
        
        private T _prefab;
        
        public void Init(AssetReference assetReference)
        {
            LoadAddressablePrefab(assetReference);
            InitPool();
        }

        public T Get() => _pool.Get();

        public void Release(T obj) => _pool.Release(obj);

        private async void LoadAddressablePrefab(AssetReference assetReference)
        {
            _loadHandle = Addressables.LoadAssetAsync<GameObject>(assetReference);
            await _loadHandle.Task;

            if (_loadHandle.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log("Addressable Prefab loaded successfully for pooling.");
                _prefab = _loadHandle.Result.GetComponent<T>();
                if(!_prefab)
                    Debug.Log($"Prefab component {typeof(T)} missing");
            }
            else
            {
                Debug.LogError($"Failed to load Addressable Prefab: {_loadHandle.OperationException}");
            }
        }

        private void InitPool()
        {
            _pool = new ObjectPool<T>(
                createFunc: CreateView,
                actionOnGet: OnGet,
                actionOnRelease: OnRelease,
                actionOnDestroy: OnDestroyItem,
                collectionCheck: true,
                defaultCapacity: defaultCapacity,
                maxSize: maxSize);
        }

        private T CreateView()
        {
            var obj = Instantiate(_prefab, transform, true);
            
            return obj;
        }

        private void OnGet(T obj)
        {
        }

        private void OnRelease(T obj)
        {
            obj.gameObject.SetActive(false);
        }

        private void OnDestroyItem(T obj)
        {
            Destroy(obj);
        }
        
        private void OnDestroy()
        {
            _pool.Clear();
            
            if (_loadHandle.IsValid())
                Addressables.Release(_loadHandle);
        }
    }
}