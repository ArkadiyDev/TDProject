using Common.Pooling;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Common.UI.FloatingText
{
    public class FloatingTextPool : GameObjectPool<FloatingTextView>
    {
        [SerializeField] private AssetReference _assetReference;

        public void InitAssetReference()
        {
            Init(_assetReference);
        }
    }
}