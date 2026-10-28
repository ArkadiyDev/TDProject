using System;
using InputSystem;
using UnityEngine;

namespace Core.Abilities
{
    public class TargetingProcessor : MonoBehaviour, ICaster
    {
        public event Action<RaycastHit> Clicked;
        
        [SerializeField] private Camera _camera;
        [SerializeField] private CoreInputService _input;
        [SerializeField] private LayerMask _clickMask = 1;

        private AbilityTargeting _currentTargeting;

        public Camera Camera => _camera;

        private void Update()
        {
            if(_currentTargeting != null && _currentTargeting.IsTargeting)
                _currentTargeting.Update();

            if (Input.GetMouseButtonDown(0))
                Raycast();

            if (Input.GetMouseButtonUp(1))
                StopTargeting();
        }

        public void SetTargeting(AbilityTargeting targeting) => _currentTargeting = targeting;
        public void RemoveTargeting() => _currentTargeting = null;

        private void Raycast()
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            
            if(Physics.Raycast(ray, out var hit, float.MaxValue, _clickMask))
                Clicked?.Invoke(hit);
        }

        private void StopTargeting()
        {
            _currentTargeting.Stop();
        }
    }
}