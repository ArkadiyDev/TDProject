using System;
using UnityEngine;

namespace Core.Towers
{
    public class ProjectileView : MonoBehaviour
    {
        private Transform _targetBodyPoint;
        private float _speed;
        private Action _onHitTarget;
        private Action _onCompletion;
        private bool _launched;

        public void SetOnCompletionAction(Action onLoseTarget)
        {
            _onCompletion = onLoseTarget;
        }
        
        public void Launch(Transform targetBodyPoint, float speed, Action onHitCallback)
        {
            _targetBodyPoint = targetBodyPoint;
            _speed = speed;
            _onHitTarget = onHitCallback;
            _launched = true;
        }

        private void Update()
        {
            if(!_launched)
                return;
            
            if (!_targetBodyPoint)
            {
                _onCompletion?.Invoke();
                return;
            }
            
            UpdateRotation();
            UpdatePosition();

            if (IsTargetAtDistance())
                return;

            _onHitTarget?.Invoke();
            _launched = false;
            _onCompletion?.Invoke();
            _onCompletion = null;
        }

        private void UpdateRotation()
        {
            var direction = (_targetBodyPoint.position - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(direction);
        }

        private void UpdatePosition() =>
            transform.position = Vector3.MoveTowards(transform.position, _targetBodyPoint.position, _speed * Time.deltaTime);

        private bool IsTargetAtDistance() =>
            Vector3.Distance(transform.position, _targetBodyPoint.position) > 0f;
    }
}