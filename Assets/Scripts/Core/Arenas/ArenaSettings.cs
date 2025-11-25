using System.Collections.Generic;
using Core.Invaders;
using UnityEngine;

namespace Core.Arenas
{
    [CreateAssetMenu(fileName = "NewArenaSettings", menuName = "TD/Settings/" + nameof(ArenaSettings))]
    public class ArenaSettings : ScriptableObject
    {
        [SerializeField, Min(0)] private float _firstWaveDelay = 5;
        [SerializeField, Min(0)] private float _wavesInterval = 10;
        [SerializeField] private List<InvadersWaveData> _waves;

        public float FirstWaveDelay => _firstWaveDelay;
        public float WavesInterval => _wavesInterval;
        public List<InvadersWaveData> Waves => _waves;
    }
}
