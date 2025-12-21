using System;
using System.Collections.Generic;
using Core.Invaders;
using Core.Waves;
using UnityEngine;

namespace Core.Arenas
{
    public class ArenaModel
    {
        public event Action WaveCompleted;
        
        private readonly ArenaSettings _arenaSettings;
        private readonly InvaderFactory _invaderFactory;
        private readonly InvaderSystem _invaderSystem;
        private readonly List<Route> _routes;

        private InvadersWave _currentWave;
        private int _currentWaveIndex;
        private bool _waveStopped;
        
        public bool WaveStopped => _waveStopped;
        public int CurrentWaveIndex => _currentWaveIndex;
        public bool IsLastWave => _currentWaveIndex >= _arenaSettings.Waves.Count;

        public ArenaModel(ArenaSettings arenaSettings, InvaderFactory invaderFactory, InvaderSystem invaderSystem, List<Route> routes)
        {
            _arenaSettings = arenaSettings;
            _invaderFactory = invaderFactory;
            _invaderSystem = invaderSystem;
            _routes = routes;
        }
        
        public void StartNextWave()
        {
            var waveData = _arenaSettings.Waves[_currentWaveIndex];
            
            _currentWave = new InvadersWave(waveData, _invaderFactory, _routes, _invaderSystem);
            _currentWave.WaveCompleted += OnInvadersWaveCompleted;
            _currentWave.StartWave();
            
            Debug.Log($"Wave {_currentWaveIndex} started");
        }

        public void StopWave()
        {
            _waveStopped = true;
            _currentWave.StopWave();
            Debug.Log($"Wave {_currentWaveIndex} stopped");
        }
        
        public void AdvanceToNextWave()
        {
            if(!IsLastWave)
                _currentWaveIndex++;
        }

        private void OnInvadersWaveCompleted(InvadersWave invadersWave)
        {
            invadersWave.WaveCompleted -= OnInvadersWaveCompleted;
            WaveCompleted?.Invoke();
        }
    }
}
