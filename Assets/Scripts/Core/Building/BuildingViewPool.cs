using Common.Pooling;
using UnityEngine;

namespace Core.Building
{
    public class BuildingViewPool : GameObjectPool<BuildingView>
    {
        [SerializeField] private BuildingSettings _buildingSettings;

        private void Awake()
        {
            //TODO: Move to bootstrap
            Init(_buildingSettings.AssetReference);
        }
    }
}