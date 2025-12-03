using System;
using Core.Invaders;
using UnityEngine;

namespace Core.Waves
{
    [Serializable]
    public class InvadersGroup
    {
        [SerializeField] private int _routeIndex;
        [SerializeField] private InvaderSettings _invaderSettings;
        [SerializeField] private int _count;
        [SerializeField] private float _delayBeforeStart;

        public int RouteIndex => _routeIndex;
        public InvaderSettings InvaderSettings => _invaderSettings;
        public int Count => _count;
        public float DelayBeforeStart => _delayBeforeStart;
    }
}
