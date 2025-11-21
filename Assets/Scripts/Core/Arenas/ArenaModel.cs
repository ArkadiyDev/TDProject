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
        private readonly List<Spawner> _spawners;
        
        private int _currentWaveIndex;

        public ArenaModel(ArenaSettings arenaSettings, InvaderFactory invaderFactory, List<Spawner> spawners)
        {
            _arenaSettings = arenaSettings;
            _invaderFactory = invaderFactory;
            _spawners = spawners;
        }
        
        public void StartNextWave()
        {
            var invadersWave = new InvadersWave(GetCurrentWave(), _invaderFactory, _spawners);
            invadersWave.WaveCompleted += OnWaveCompleted;
            
            invadersWave.StartWave();
            Debug.Log($"Wave {_currentWaveIndex} started");
        }

        private InvadersWaveData GetCurrentWave()
        {
            return _arenaSettings.Waves[_currentWaveIndex];
        }

        private async void OnWaveCompleted(InvadersWave invadersWave)
        {
            Debug.Log($"Wave {_currentWaveIndex} completed");
            invadersWave.WaveCompleted -= OnWaveCompleted;
            
            _currentWaveIndex++;

            if (_currentWaveIndex >= _arenaSettings.Waves.Count)
                return;
            
            await UniTask.Delay(TimeSpan.FromSeconds(_arenaSettings.WavesInterval));
                
            StartNextWave();
        }
    }
}
