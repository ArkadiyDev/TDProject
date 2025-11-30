using System;
using System.Collections.Generic;
using Core.Invaders;
using UnityEngine;

namespace Core.Arenas
{
    public class ArenaModel
    {
        public event Action WaveCompleted;
        
        private readonly ArenaSettings _arenaSettings;
        private readonly InvaderFactory _invaderFactory;
        private readonly List<Route> _routes;
        
        private int _currentWaveIndex;
        private bool _waveStopped;
        
        public bool IsWaveStopped => _waveStopped;
        public int CurrentWaveIndex => _currentWaveIndex;
        public bool IsLastWave => _currentWaveIndex >= _arenaSettings.Waves.Count;

        public ArenaModel(ArenaSettings arenaSettings, InvaderFactory invaderFactory, List<Route> routes)
        {
            _arenaSettings = arenaSettings;
            _invaderFactory = invaderFactory;
            _routes = routes;
        }
        
        public void StartNextWave()
        {
            var invadersWave = new InvadersWave(GetCurrentWave(), _invaderFactory, _routes);
            invadersWave.WaveCompleted += OnInvadersWaveCompleted;
            
            invadersWave.StartWave();
            Debug.Log($"Wave {_currentWaveIndex} started");
        }

        public void StopWave()
        {
            _waveStopped = true;
            Debug.Log($"Wave {_currentWaveIndex} stopped");
        }
        
        public void AdvanceToNextWave()
        {
            if(!IsLastWave)
                _currentWaveIndex++;
        }

        private InvadersWaveData GetCurrentWave()
        {
            return _arenaSettings.Waves[_currentWaveIndex];
        }
        
        private void OnInvadersWaveCompleted(InvadersWave invadersWave)
        {
            invadersWave.WaveCompleted -= OnInvadersWaveCompleted;
            WaveCompleted?.Invoke();
        }
    }
}
