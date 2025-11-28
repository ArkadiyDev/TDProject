using UnityEngine;

namespace Core.Building
{
    public class ConstructionSite : MonoBehaviour
    {
        private static readonly Color GizmosColor = new(0.3f, 0.7f, 0.3f, 1f);

        [SerializeField] private Vector2Int _siteSize = new(5, 5);
        [SerializeField] private float _gridSize = 0.5f;
        
        private void OnValidate()
        {
            AlignToGrid();
        }

        private void AlignToGrid()
        {
            var position = transform.position;

            position.x = Mathf.Round(position.x / _gridSize) * _gridSize;
            position.z = Mathf.Round(position.z / _gridSize) * _gridSize;

            transform.position = position;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = GizmosColor;
            
            for (float x = 0; x <= _siteSize.x; x++)
                Gizmos.DrawLine(new Vector3(x * _gridSize, -0.1f, 0) + transform.position,
                    new Vector3(x * _gridSize, -0.1f, _siteSize.y * _gridSize) + transform.position);
        
            for (float y = 0; y <= _siteSize.y; y++)
                Gizmos.DrawLine(new Vector3(0, -0.1f, y * _gridSize) + transform.position,
                    new Vector3(_siteSize.x * _gridSize, -0.1f, y * _gridSize) + transform.position);
        }
    }
}