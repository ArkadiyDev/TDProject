using UnityEngine;

namespace Core.Arenas
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private Waypoint _startWaypoint;

        public Waypoint StartWaypoint => _startWaypoint;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, 0.5f);

            if (_startWaypoint != null)
                Gizmos.DrawLine(transform.position, _startWaypoint.transform.position);

            Gizmos.color = Color.white;
        }
    }
}
