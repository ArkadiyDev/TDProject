using Core.Invaders;
using UnityEngine;

namespace Core.Towers.Targeting
{
    [CreateAssetMenu(fileName = "NewDefaultTargetsFinder", menuName = "TD/Settings/Targeting/" + nameof(DefaultTargetsFinder))]
    public class DefaultTargetsFinder : TargetsFinder
    {
        public override bool TryFindTarget(Vector3 position, float radius, out IDamageable target)
        {
            if (!TryFindTargetsInRadius(position, radius, out var damageables))
            {
                target = null;
                return false;
            }

            target = damageables[0];
            return true;
        }
    }
}