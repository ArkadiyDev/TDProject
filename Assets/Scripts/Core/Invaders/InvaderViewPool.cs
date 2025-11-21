using UnityEngine;
using UnityEngine.Pool;

namespace Core.Invaders
{
    public class InvaderViewPool : MonoBehaviour
    {
        private InvaderSettings _invaderSettings;
        private IObjectPool<InvaderView> _invaderPool;

        public void Init(InvaderSettings invaderSettings)
        {
            _invaderSettings = invaderSettings;
            
            _invaderPool = new ObjectPool<InvaderView>(
                createFunc: CreateItem,
                actionOnGet: OnGet,
                actionOnRelease: OnRelease,
                actionOnDestroy: OnDestroyItem,
                collectionCheck: true,
                defaultCapacity: 16,
                maxSize: 150);
        }

        public InvaderView Get() => _invaderPool.Get();

        public void Release(InvaderView invaderView) => _invaderPool.Release(invaderView);

        private InvaderView CreateItem()
        {
            //TODO: Replace with Addressables
            var view = Instantiate(_invaderSettings.ViewPrefab, transform, true);

            return view;
        }

        private void OnGet(InvaderView invader)
        {
            invader.gameObject.SetActive(true);
        }

        private void OnRelease(InvaderView invader)
        {
            invader.gameObject.SetActive(false);
        }

        private void OnDestroyItem(InvaderView invader)
        {
            Destroy(invader);
        }
    }
}
