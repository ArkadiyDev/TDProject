using System.Collections.Generic;
using Core.Building;
using Core.Towers.Targeting;
using Economy.Currencies;
using UnityEngine;

namespace Core.Towers
{
    [CreateAssetMenu(fileName = "NewTowerSettings", menuName = "TD/Settings/" + nameof(TowerSettings))]
    public class TowerSettings : BuildingSettings
    {
        [SerializeField] private ProjectileSettings _projectile;
        [SerializeField] private TargetsFinder _targetsFinder;
        [SerializeField] private float _range;
        [SerializeField] private float _damage;
        [SerializeField] private float _fireRate;
        [SerializeField] private List<CurrencyData> _cost;

        public ProjectileSettings Projectile => _projectile;
        public TargetsFinder TargetsFinder => _targetsFinder;
        public float Range => _range;
        public float Damage => _damage;
        public float FireRate => _fireRate;
        public List<CurrencyData> Cost => _cost;
    }
}