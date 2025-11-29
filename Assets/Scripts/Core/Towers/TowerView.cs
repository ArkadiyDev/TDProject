using UnityEngine;

namespace Core.Towers
{
    public class TowerView : MonoBehaviour
    {
        [SerializeField] private Transform _projectileStartPoint;
        public Transform ProjectileStartPoint => _projectileStartPoint;

        private float _detectionRadius;
        
        private void OnDrawGizmos()
        {
            if(Mathf.Approximately(_detectionRadius, 0))
                return;
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _detectionRadius);
        }
        
        public void InitializeVisuals(float detectionRadius)
        {
            _detectionRadius = detectionRadius;
        }
    }
}