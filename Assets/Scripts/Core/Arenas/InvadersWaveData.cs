using System;
using System.Collections.Generic;
using Core.Invaders;
using UnityEngine;

namespace Core.Arenas
{
    [Serializable]
    public class InvadersWaveData
    {
        [SerializeField] private List<InvadersGroup> _invadersGroup;

        public List<InvadersGroup> InvadersGroup => _invadersGroup;
    }
}
