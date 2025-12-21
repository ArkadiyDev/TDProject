using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Core.Arenas;
using Core.Invaders;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Waves
{
    public class InvadersWave
    {
        public Action<InvadersWave> WaveCompleted;
        
        private const int SpawnInterval = 1;

        private readonly InvadersWaveData _waveData;
        private readonly InvaderFactory _invaderFactory;
        private readonly List<Route> _routes;
        private readonly InvaderSystem _invaderSystem;
        
        private int _totalInvaders;
        private CancellationTokenSource _waveCts;

        public InvadersWave(InvadersWaveData waveData, InvaderFactory invaderFactory, List<Route> routes, InvaderSystem invaderSystem)
        {
            _waveData = waveData;
            _invaderFactory = invaderFactory;
            _routes = routes;
            _invaderSystem = invaderSystem;
        }

        public void StartWave()
        {
            _waveCts = new CancellationTokenSource();
            _totalInvaders = _waveData.InvadersGroup.Sum(g => g.Count);
            Debug.Log($"Total invaders in current wave: {_totalInvaders}");
            
            foreach (var invadersGroup in _waveData.InvadersGroup)
                SpawnInvadersGroup(invadersGroup, _waveCts.Token).Forget();
        }
        
        public void StopWave()
        {
            _waveCts?.Cancel();
            _waveCts?.Dispose();
            _waveCts = null;
            
            _invaderSystem.RemoveAll();
        }

        private async UniTaskVoid SpawnInvadersGroup(InvadersGroup invadersGroup, CancellationToken waveCtsToken)
        {
            var routeIndex = invadersGroup.RouteIndex;
                
            if (invadersGroup.RouteIndex >= _routes.Count || _routes.Count == 0)
            {
                Debug.Log($"Route with index {invadersGroup.RouteIndex} not exist");
                return;
            }

            var route = _routes[routeIndex];
            
            if (!route.HasWaypoints)
                return;
            
            if (invadersGroup.DelayBeforeStart > 0)
                await UniTask.Delay(TimeSpan.FromSeconds(invadersGroup.DelayBeforeStart));
            
            for (int i = 0; i < invadersGroup.Count; i++)
            {
                if (waveCtsToken.IsCancellationRequested)
                    return;
                
                var invader = _invaderFactory.Create(_routes[invadersGroup.RouteIndex]);
                invader.Removed += OnInvaderRemoved;
                await UniTask.Delay(TimeSpan.FromSeconds(SpawnInterval));
            }
        }

        private void OnInvaderRemoved(Invader invader)
        {
            _totalInvaders--;
            invader.Removed -= OnInvaderRemoved;
            
            if(CheckWaveCompleted())
                WaveCompleted?.Invoke(this);
        }
        
        private bool CheckWaveCompleted() =>
            _totalInvaders <= 0 && _invaderSystem.ActiveCount == 0;
    }
}