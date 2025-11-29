using System.Collections.Generic;
using Core.Invaders;
using UnityEngine;

namespace Core.Towers
{
    public static class TargetFinderHelper
    {
        public static bool TryFindTarget(Vector3 position, float radius, out InvaderView targetView, LayerMask layer)
        {
            var results = new Collider[64];
            Physics.OverlapSphereNonAlloc(position, radius, results, layer);

            var views = new List<InvaderView>();

            for (var i = 0; i < results.Length; i++)
            {
                var result = results[i];
                
                if (!result)
                    break;

                var view = result.GetComponent<InvaderView>();

                if (!view)
                    continue;

                views.Add(view);
            }

            if (views.Count == 0)
            {
                targetView = null;
                return false;
            }
            
            targetView = views[0];
            return true;
        }
    }
}