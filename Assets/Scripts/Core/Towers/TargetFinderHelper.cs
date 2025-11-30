using System.Collections.Generic;
using Core.Invaders;
using UnityEngine;

namespace Core.Towers
{
    public static class TargetFinderHelper
    {
        public static bool TryFindTarget(Vector3 position, float radius, out IDamageable target, LayerMask layer)
        {
            var results = new Collider[64];
            Physics.OverlapSphereNonAlloc(position, radius, results, layer);

            var targets = new List<IDamageable>();

            for (var i = 0; i < results.Length; i++)
            {
                var result = results[i];
                
                if (!result)
                    break;
                
                var invaderLink = result.GetComponent<InvaderLink>();
                
                if(!invaderLink)
                    continue;
                
                if(!invaderLink.Damageable.IsAlive)
                    continue;

                targets.Add(invaderLink.Damageable);
            }

            if (targets.Count == 0)
            {
                target = null;
                return false;
            }
            
            target = targets[0];
            return true;
        }
    }
}