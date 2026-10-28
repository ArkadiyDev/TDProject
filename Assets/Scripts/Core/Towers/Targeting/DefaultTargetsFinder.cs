using Core.Damaging;
using UnityEngine;

namespace Core.Towers.Targeting
{
    [CreateAssetMenu(fileName = "NewDefaultTargetsFinder", menuName = "TD/Settings/Targeting/" + nameof(DefaultTargetsFinder))]
    public class DefaultTargetsFinder : TargetsFinder
    {
        public override bool TryFindTarget(Vector3 position, float radius, out IDamageable target)
        {
            if (!TryFindTargetsInRadius(position, radius, out var targets))
            {
                target = null;
                return false;
            }

            target = targets[0];
            return true;
        }
    }
}