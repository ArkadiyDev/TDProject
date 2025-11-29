using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Core.Towers
{
    [CreateAssetMenu(fileName = "NewProjectileSettings", menuName = "TD/Settings/" + nameof(ProjectileSettings))]
    public class ProjectileSettings : ScriptableObject
    {
        [SerializeField] private AssetReference _assetReference;
        [SerializeField] private float _speed;
        
        public AssetReference AssetReference => _assetReference;
        public float Speed => _speed;
    }
}