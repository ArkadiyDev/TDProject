using System.Collections.Generic;
using Core.Damaging;
using UnityEngine;

namespace Core.Towers.Targeting
{
    public abstract class TargetsFinder : ScriptableObject
    {
        [SerializeField] private LayerMask _layerMask;
        public abstract bool TryFindTarget(Vector3 position, float radius, out IDamageable target);

        protected bool TryFindTargetsInRadius(Vector3 position, float radius, out List<IDamageable> targets)
        {
            var results = new Collider[64];
            Physics.OverlapSphereNonAlloc(position, radius, results, _layerMask);

            targets = new List<IDamageable>();

            for (var i = 0; i < results.Length; i++)
            {
                var result = results[i];
                
                if (!result)
                    break;
                
                if(!result.TryGetComponent<IDamageable>(out var target))
                    continue;
                
                if(target.IsDead)
                    continue;

                targets.Add(target);
            }

            return targets.Count > 0;
        }
    }
}