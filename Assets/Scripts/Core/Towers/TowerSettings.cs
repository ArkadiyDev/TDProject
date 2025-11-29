using Core.Building;
using UnityEngine;

namespace Core.Towers
{
    [CreateAssetMenu(fileName = "NewTowerSettings", menuName = "TD/Settings/" + nameof(TowerSettings))]
    public class TowerSettings : BuildingSettings
    {
        [SerializeField] private ProjectileSettings _projectile;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private float _range;
        [SerializeField] private float _damage;
        [SerializeField] private float _fireRate;

        public ProjectileSettings Projectile => _projectile;
        public LayerMask LayerMask => _layerMask;
        public float Range => _range;
        public float Damage => _damage;
        public float FireRate => _fireRate;
    }
}