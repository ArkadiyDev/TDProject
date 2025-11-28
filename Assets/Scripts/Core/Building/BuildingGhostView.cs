using System.Collections.Generic;
using UnityEngine;

namespace Core.Building
{
    public class BuildingGhostView : MonoBehaviour
    {
        private static readonly int ColorId = Shader.PropertyToID("_Color");
        
        [SerializeField] private List<Renderer> _renderers;
        [SerializeField] private Color _availableColor = Color.green;
        [SerializeField] private Color _unavailableColor = Color.red;
        [SerializeField] private Vector3 _defaultPosition = new Vector3(0f, -1000f, 0f);
        
        [SerializeField] private CollisionChecker _buildableCollisionChecker;
        [SerializeField] private CollisionChecker _defencesCollisionChecker;

        public bool OnBuildableSite => _buildableCollisionChecker.IsIntersecting;
        public bool SpaceIsFree => !_defencesCollisionChecker.IsIntersecting;

        public void SetAvailable(bool available)
        {
            foreach (var renderer in _renderers)
                renderer.material.SetColor(ColorId, available ? _availableColor : _unavailableColor);
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
            
            if(!active)
                SetDefaultPosition();
        }

        private void SetDefaultPosition()
        {
            transform.position = _defaultPosition;
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }
    }
}