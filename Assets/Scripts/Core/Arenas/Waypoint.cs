using System.Collections.Generic;
using Core.Castles;
using JetBrains.Annotations;
using UnityEngine;

namespace Core.Arenas
{
    public class Waypoint : MonoBehaviour
    {
        [SerializeField] private List<Waypoint> _nextWaypoints;
        [SerializeField] [CanBeNull] private CastleView _castleView;

        public Vector3 Position => transform.position;
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position, 0.2f);
            Gizmos.color = Color.white;
        }
    }
}
