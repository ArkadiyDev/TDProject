using System.Collections.Generic;
using UnityEngine;

namespace Core.Invaders
{
    public class InvaderSystem : MonoBehaviour
    {
        private readonly List<Invader> _activeInvaders = new();
        private readonly HashSet<Invader> _toRemove = new();

        public int ActiveCount => _activeInvaders.Count - _toRemove.Count;
        
        public void Add(Invader invader)
        {
            _activeInvaders.Add(invader);
            invader.Removed += OnInvaderRemoved;
        }
        
        public void RemoveAll()
        {
            for (int i = _activeInvaders.Count - 1; i >= 0; i--)
                _activeInvaders[i].Remove();
        
            _activeInvaders.Clear();
        }

        private void OnInvaderRemoved(Invader invader)
        {
            _toRemove.Add(invader);
            
            invader.Removed -= OnInvaderRemoved;
            _activeInvaders.Remove(invader);
        }

        private void Update()
        {
            var deltaTime = Time.deltaTime;
            
            for (int i = 0; i < _activeInvaders.Count; i++)
            {
                var invader = _activeInvaders[i];
                
                if (_toRemove.Contains(invader))
                    continue;
            
                invader.Tick(deltaTime);
            }
            
            if (_toRemove.Count > 0)
            {
                _activeInvaders.RemoveAll(invader => _toRemove.Contains(invader));
                _toRemove.Clear();
            }
        }
    }
}