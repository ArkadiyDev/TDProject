using System.Collections.Generic;
using UnityEngine;

namespace Core.Arenas
{
    public class Waypoint : MonoBehaviour
    {
        [SerializeField] private List<Waypoint> _nextWaypoints;

        public bool TryGetNextWaypoints(out Waypoint waypoint)
        {
            if (_nextWaypoints.Count == 0)
            {
                waypoint = default;
                return false;
            }

            waypoint = _nextWaypoints[Random.Range(0, _nextWaypoints.Count)];

            return true;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position, 0.2f);

            if (_nextWaypoints != null)
                foreach (var nextWaypoint in _nextWaypoints)
                    Gizmos.DrawLine(transform.position, nextWaypoint.transform.position);

            Gizmos.color = Color.white;
        }
    }
}
