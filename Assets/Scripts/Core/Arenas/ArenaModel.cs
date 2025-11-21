using System;
using System.Collections.Generic;
using Core.Invaders;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Arenas
{
    public class ArenaModel
    {
        private readonly ArenaSettings _arenaSettings;
        private readonly InvaderFactory _invaderFactory;
        private readonly List<Route> _routes;
        
        private int _currentWaveIndex;
        private bool _waveStopped;

        public ArenaModel(ArenaSettings arenaSettings, InvaderFactory invaderFactory, List<Route> routes)
        {
            _arenaSettings = arenaSettings;
            _invaderFactory = invaderFactory;
            _routes = routes;
        }
        
        public void StartNextWave()
        {
            var invadersWave = new InvadersWave(GetCurrentWave(), _invaderFactory, _routes);
            invadersWave.WaveCompleted += OnWaveCompleted;
            
            invadersWave.StartWave();
            Debug.Log($"Wave {_currentWaveIndex} started");
        }

        public void StopWave()
        {
            _waveStopped = true;
            Debug.Log($"Wave {_currentWaveIndex} stopped");
        }

        private InvadersWaveData GetCurrentWave()
        {
            return _arenaSettings.Waves[_currentWaveIndex];
        }

        private async void OnWaveCompleted(InvadersWave invadersWave)
        {
            invadersWave.WaveCompleted -= OnWaveCompleted;
            
            if(_waveStopped)
                return;
                
            Debug.Log($"Wave {_currentWaveIndex} completed");
            
            _currentWaveIndex++;

            if (_currentWaveIndex >= _arenaSettings.Waves.Count)
                return;
            
            await UniTask.Delay(TimeSpan.FromSeconds(_arenaSettings.WavesInterval));
                
            StartNextWave();
        }
    }
}
