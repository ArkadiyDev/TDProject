using Core.Damaging;
using Core.Invaders;
using UnityEngine;

namespace Core.Towers.Targeting
{
    [CreateAssetMenu(fileName = "NewClosestTargetsFinder", menuName = "TD/Settings/Targeting/" + nameof(ClosestTargetsFinder))]
    public class ClosestTargetsFinder : TargetsFinder
    {
        public override bool TryFindTarget(Vector3 position, float radius, out IDamageable target)
        {
            if (!TryFindTargetsInRadius(position, radius, out var damageables))
            {
                target = null;
                return false;
            }
            
            var minDistanceSqr = float.MaxValue;
            IDamageable closestTarget = null;

            foreach (var potentialTarget in damageables)
            {
                var currentDistanceSqr = (potentialTarget.BodyPoint.position - position).sqrMagnitude;

                if (!(currentDistanceSqr < minDistanceSqr))
                    continue;
                
                minDistanceSqr = currentDistanceSqr;
                closestTarget = potentialTarget;
            }
            
            target = closestTarget;
            return true;
        }
    }
}