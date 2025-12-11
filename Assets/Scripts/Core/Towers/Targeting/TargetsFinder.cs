using System.Collections.Generic;
using Core.Invaders;
using UnityEngine;

namespace Core.Towers.Targeting
{
    public abstract class TargetsFinder : ScriptableObject
    {
        [SerializeField] private LayerMask _layerMask;
        public abstract bool TryFindTarget(Vector3 position, float radius, out IDamageable target);

        protected bool TryFindTargetsInRadius(Vector3 position, float radius, out List<IDamageable> damageables)
        {
            var results = new Collider[64];
            Physics.OverlapSphereNonAlloc(position, radius, results, _layerMask);

            damageables = new List<IDamageable>();

            for (var i = 0; i < results.Length; i++)
            {
                var result = results[i];
                
                if (!result)
                    break;
                
                var invaderLink = result.GetComponent<InvaderLink>();
                
                if(!invaderLink)
                    continue;
                
                if(invaderLink.Damageable.IsDead)
                    continue;

                damageables.Add(invaderLink.Damageable);
            }

            return damageables.Count > 0;
        }
    }
}