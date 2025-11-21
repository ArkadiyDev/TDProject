using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Invaders
{
    [Serializable]
    public class InvadersWaveData
    {
        [SerializeField] private List<InvadersGroup> _invadersGroup;

        public List<InvadersGroup> InvadersGroup => _invadersGroup;
    }
}
