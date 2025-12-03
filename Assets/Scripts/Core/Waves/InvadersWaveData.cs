using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Waves
{
    [Serializable]
    public class InvadersWaveData
    {
        [SerializeField] private List<InvadersGroup> _invadersGroup;

        public List<InvadersGroup> InvadersGroup => _invadersGroup;
    }
}
