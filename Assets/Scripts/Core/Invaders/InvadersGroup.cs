using System;
using UnityEngine;

namespace Core.Invaders
{
    [Serializable]
    public class InvadersGroup
    {
        [SerializeField] private int _spawnerIndex;
        [SerializeField] private InvaderSettings _invaderSettings;
        [SerializeField] private int _count;

        public int SpawnerIndex => _spawnerIndex;
        public InvaderSettings InvaderSettings => _invaderSettings;
        public int Count => _count;
    }
}
