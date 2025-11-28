using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Core.Building
{
    [CreateAssetMenu(fileName = "NewBuildingSettings", menuName = "TD/Settings/" + nameof(BuildingSettings))]
    public class BuildingSettings : ScriptableObject
    {
        [SerializeField] private AssetReference _assetReference;
        [SerializeField] private Vector2 _size = new(2f, 2f);

        public AssetReference AssetReference => _assetReference;
        public Vector2 Size => _size;
    }
}