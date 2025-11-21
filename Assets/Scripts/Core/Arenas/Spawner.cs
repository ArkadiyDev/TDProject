using UnityEngine;

namespace Core.Arenas
{
    public class Spawner : MonoBehaviour
    {
        public Vector3 Position => transform.position;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, 0.4f);

            Gizmos.color = Color.white;
        }
    }
}
