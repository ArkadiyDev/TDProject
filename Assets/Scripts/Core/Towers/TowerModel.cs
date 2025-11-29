using UnityEngine;

namespace Core.Towers
{
    public class TowerModel
    {
        private TowerSettings _settings;
        
        public float СurrentFireTimer { get; set; }
        public float Range => _settings.Range;
        public float Damage => _settings.Damage;
        public LayerMask LayerMask => _settings.LayerMask;

        public TowerModel(TowerSettings settings)
        {
            _settings = settings;
            СurrentFireTimer = 0f;
        }

        public bool CanShoot()
        {
            return СurrentFireTimer >= _settings.FireRate;
        }

        public void ResetFireTimer()
        {
            СurrentFireTimer = 0f;
        }
    }
}