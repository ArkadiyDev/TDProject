using System;
using Core.Invaders;
using UnityEngine;

namespace Core.Towers
{
    public class ProjectileView : MonoBehaviour
    {
        private IDamageable _target;
        private float _speed;
        private float _damage;
        private Action _onHit;
        private bool _launched;

        public void SetOnHitAction(Action onHit)
        {
            _onHit = onHit;
        }
        
        public void Launch(IDamageable target, float speed, float damage)
        {
            _target = target;
            _speed = speed;
            _damage = damage;
            _launched = true;
        }

        private void Update()
        {
            if(!_launched)
                return;
            
            if (_target == null)
            {
                _onHit?.Invoke();
                return;
            }
            
            UpdateRotation();
            UpdatePosition();

            if (IsTargetAtDistance())
                return;

            _target.TakeDamage(_damage);
            _launched = false;
            _onHit?.Invoke();
            _onHit = null;
        }

        private void UpdateRotation()
        {
            var direction = (_target.BodyPoint.position - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(direction);
        }

        private void UpdatePosition()
        {
            transform.position = Vector3.MoveTowards(transform.position, _target.BodyPoint.position, _speed * Time.deltaTime);
        }

        private bool IsTargetAtDistance()
        {
            return Vector3.Distance(transform.position, _target.BodyPoint.position) > 0f;
        }
    }
}