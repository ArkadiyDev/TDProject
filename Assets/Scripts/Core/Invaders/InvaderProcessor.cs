using System.Collections.Generic;
using UnityEngine;

namespace Core.Invaders
{
    public class InvaderProcessor : MonoBehaviour
    {
        private readonly List<Invader> _activeInvaders = new();
        private List<Invader> _removedInvaders = new();

        public void RegisterInvader(Invader invader)
        {
            _activeInvaders.Add(invader);
        }

        public void UnregisterInvader(Invader invader)
        {
            _removedInvaders.Add(invader);
        }

        private void Update()
        {
            var deltaTime = Time.deltaTime;
            
            foreach (var invader in _activeInvaders)
                invader.Tick(deltaTime);
            
            foreach (var invader in _removedInvaders)
                _activeInvaders.Remove(invader);
        }
    }
}