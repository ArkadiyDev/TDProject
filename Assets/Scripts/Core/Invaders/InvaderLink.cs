using Core.Abilities;
using Core.Damaging;
using UnityEngine;

namespace Core.Invaders
{
    public class InvaderLink : MonoBehaviour, IDamageable
    {
        [SerializeField] private Transform _transform;

        public Transform Transform => _transform;
        public bool IsDead => Damageable == null || Damageable.IsDead;
        public IDamageable Damageable { get; private set; }

        public void Initialize(IDamageable damageable)
        {
            Damageable = damageable;
        }
        
        public void Reset()
        {
            Damageable = null;
        }
        
        public void TakeDamage(float damageAmount)
        {
            Damageable.TakeDamage(damageAmount);
        }
    }
}