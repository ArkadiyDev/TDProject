using System;
using Core.Invaders;
using UnityEngine;

namespace Core.Castles
{
    public class Castle
    {
        public event Action Destroyed;
        
        private float _health;
        private CastleView _castleView;

        public float Health => _health;

        public Castle(CastleSettings settings, CastleView castleView)
        {
            _health = settings.BaseHealth;
            _castleView = castleView;

            _castleView.InvaderEntered += OnInvaderEntered;
        }

        private void OnInvaderEntered(Invader invader)
        {
            Debug.Log($"{invader.Name} dealt {invader.Damage} damage to the Castle, Castle health is {_health}");

            TakeDamage(invader.Damage);
        }

        public void TakeDamage(float amount)
        {
            _health -= amount;
            if (_health <= 0)
                Die();
        }

        private void Die()
        {
            Debug.Log($"Castle destroyed");
            Destroyed?.Invoke();
        }
    }
}
