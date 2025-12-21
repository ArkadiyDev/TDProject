using System;
using Core.Arenas;
using Core.Invaders;
using UnityEngine;

namespace Core.Castles
{
    public class Castle : IResettable
    {
        public event Action Destroyed;
        
        private readonly CastleSettings _settings;

        private float _health;
        private CastleView _castleView;

        public float Health => _health;

        public Castle(CastleSettings settings, CastleView castleView)
        {
            _settings = settings;
            
            _health = _settings.BaseHealth;
            _castleView = castleView;

            _castleView.InvaderEntered += OnInvaderEntered;
        }

        private void OnInvaderEntered(Invader invader)
        {
            TakeDamage(invader.Damage);
            
            Debug.Log($"{invader.Name} dealt {invader.Damage} damage to the Castle, Castle health is {_health}");
        }

        public void TakeDamage(float amount)
        {
            _health -= amount;
            if (_health <= 0)
                Die();
        }

        public void Reset()
        {
            _health = _settings.BaseHealth;
            Debug.Log($"Reset Castle, Castle health is {_health}");
        }

        private void Die()
        {
            Debug.Log($"Castle destroyed");
            Destroyed?.Invoke();
        }
    }
}
