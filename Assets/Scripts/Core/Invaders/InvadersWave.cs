using System;
using System.Collections.Generic;
using Core.Arenas;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Invaders
{
    public class InvadersWave
    {
        public Action<InvadersWave> WaveCompleted;
        
        private const int SpawnInterval = 1;

        private readonly InvadersWaveData _waveData;
        private readonly InvaderFactory _invaderFactory;
        private readonly List<Route> _routes;
        private readonly List<Invader> _invaders = new();
        
        private int _spawnedGroups;

        public InvadersWave(InvadersWaveData waveData, InvaderFactory invaderFactory, List<Route> routes)
        {
            _waveData = waveData;
            _invaderFactory = invaderFactory;
            _routes = routes;
        }

        public void StartWave()
        {
            foreach (var invadersGroup in _waveData.InvadersGroup)
                SpawnInvadersGroup(invadersGroup);
        }

        private async void SpawnInvadersGroup(InvadersGroup invadersGroup)
        {
            var routeIndex = invadersGroup.RouteIndex;
                
            if (invadersGroup.RouteIndex >= _routes.Count || _routes.Count == 0)
            {
                Debug.Log($"Route with index {invadersGroup.RouteIndex} not exist");
                return;
            }

            var route = _routes[routeIndex];
            
            if (!route.TryGetFirstWaypoint(out var startWaypoint))
                return;
            
            for (int i = 0; i < invadersGroup.Count; i++)
            {
                var invader = _invaderFactory.Create();
                invader.SetRoute(route);
                invader.SetStartPosition(route.Spawner.Position);
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