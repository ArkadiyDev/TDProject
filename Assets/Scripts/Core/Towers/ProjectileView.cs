using System;
using UnityEngine;

namespace Core.Towers
{
    public class ProjectileView : MonoBehaviour
    {
        private Transform _target;
        private float _speed;
        private bool _launched;
        private Action _onHit;

        public void Launch(Transform targetTransform, float speed, Action onHit)
        {
            _target = targetTransform;
            _speed = speed;
            _launched = true;
            _onHit = onHit;
        }

        private void Update()
        {
            if(!_launched)
                return;
            
            if (!_target)
            {
                _onHit?.Invoke();
                return;
            }
            
            UpdateRotation();
            UpdatePosition();

            if (IsTargetAtDistance())
                return;
            
            _launched = false;
            _onHit?.Invoke();
            _onHit = null;
        }

        private void UpdateRotation()
        {
            var direction = (_target.position - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(direction);
        }

        private void UpdatePosition()
        {
            transform.position = Vector3.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);
        }

        private bool IsTargetAtDistance()
        {
            return Vector3.Distance(transform.position, _target.position) > 0f;
        }
    }
}