using UnityEngine;

namespace Common.UI.FloatingText
{
    public class FloatingTextService : IFloatingTextService
    {
        private readonly FloatingTextPool _pool;
        private readonly Camera _camera;

        public FloatingTextService(FloatingTextPool pool, Camera camera)
        {
            _pool = pool;
            _camera = camera;

            _pool.InitAssetReference();
        }

        public void ShowText(Vector3 position, string text)
        {
            var floatingTextView = _pool.Get();
            floatingTextView.Set(position, text);
            floatingTextView.gameObject.SetActive(true);
            floatingTextView.StartAnimation(_camera.transform, () => _pool.Release(floatingTextView));
        }
    }
}