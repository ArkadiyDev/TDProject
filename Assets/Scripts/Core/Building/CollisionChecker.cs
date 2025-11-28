using System.Collections.Generic;
using UnityEngine;

namespace Core.Building
{
    public class CollisionChecker : MonoBehaviour
    {
        [SerializeField] private LayerMask _layerToCheck;

        private readonly HashSet<Collider> _colliders = new();
        private Collider _triggerCollider;

        public bool IsIntersecting => _colliders.Count > 0;
        
        private void Awake()
        {
            _triggerCollider = GetComponent<BoxCollider>();
        }

        private void OnEnable()
        {
            _colliders.Clear();

            if (_triggerCollider is not BoxCollider boxCollider)
                return;
            
            var hits = Physics.OverlapBox(
                transform.position + boxCollider.center, 
                boxCollider.size / 2f, 
                transform.rotation, 
                _layerToCheck
            );

            foreach (var hit in hits)
            {
                if (hit != _triggerCollider)
                    _colliders.Add(hit);
            }
        }

        private void OnDisable()
        {
            _colliders.Clear();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (!IsInLayerMask(other.gameObject, _layerToCheck))
                return;

            _colliders.Add(other);
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (!IsInLayerMask(other.gameObject, _layerToCheck))
                return;
            
            if(_colliders.Contains(other))
                _colliders.Remove(other);
        }

        private static bool IsInLayerMask(GameObject obj, LayerMask layerMask)
        {
            return (layerMask.value & (1 << obj.layer)) != 0;
        }
    }
}