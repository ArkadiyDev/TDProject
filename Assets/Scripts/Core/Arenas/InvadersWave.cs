using System;
using System.Collections.Generic;
using Core.Invaders;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Arenas
{
    public class InvadersWave
    {
        public Action<InvadersWave> WaveCompleted;
        
        private const int SpawnInterval = 1;

        private readonly InvadersWaveData _waveData;
        private readonly InvaderFactory _invaderFactory;
        private readonly List<Spawner> _spawners;

        private List<Invader> _invaders = new();
        private int _spawnedGroups;

        public InvadersWave(InvadersWaveData waveData, InvaderFactory invaderFactory, List<Spawner> spawners)
        {
            _waveData = waveData;
            _invaderFactory = invaderFactory;
            _spawners = spawners;
        }

        public void StartWave()
        {
            foreach (var invadersGroup in _waveData.InvadersGroup)
            {
                if (invadersGroup.SpawnerIndex >= _spawners.Count)
                {
                    Debug.Log($"Spawner with index {invadersGroup.SpawnerIndex} not exist");
                    continue;
                } 
                    
                SpawnInvadersGroup(invadersGroup);
            }
        }

        private async void SpawnInvadersGroup(InvadersGroup invadersGroup)
        {
            for (int i = 0; i < invadersGroup.Count; i++)
            {
                var startWaypoint = _spawners[invadersGroup.SpawnerIndex].StartWaypoint;

                var invader = _invaderFactory.Create();
                invader.SetStartPosition(_spawners[invadersGroup.SpawnerIndex].gameObject.transform.position);
                invader.SetStartWaypoint(startWaypoint);
                invader.MoveToNextWaypoint();
                
                _invaders.Add(invader);
                invader.Removed += OnInvaderRemoved;

                await UniTask.Delay(TimeSpan.FromSeconds(SpawnInterval));
            }

            _spawnedGroups++;
        }

        private void OnInvaderRemoved(Invader invader)
        {
            invader.Removed -= OnInvaderRemoved;
            
            _invaders.Remove(invader);
            if(CheckWaveCompleted())
                WaveCompleted?.Invoke(this);
        }
        
        private bool CheckWaveCompleted()
        {
            return _spawnedGroups >= _waveData.InvadersGroup.Count && _invaders.Count == 0;
        }
    }
}