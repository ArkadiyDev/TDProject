using System;
using Core.Castles;
using Core.Invaders;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Arenas
{
    public class Arena : IResettable
    {
        public event Action OnGameOver;
        public event Action OnGameWon;
        
        private readonly ArenaSettings _arenaSettings;
        private readonly ArenaModel _arenaModel;
        private readonly Castle _castle; 

        public Arena(ArenaView arenaView, ArenaSettings arenaSettings, InvaderFactory invaderFactory,
            CastleSettings castleSettings, InvaderSystem invaderSystem)
        {
            _arenaSettings = arenaSettings;
            _arenaModel = new ArenaModel(arenaSettings, invaderFactory, invaderSystem, arenaView.Routes);
            _castle = new Castle(castleSettings, arenaView.CastleView);

            _castle.Destroyed += OnCastleDestroyed;
            _arenaModel.WaveCompleted += OnWaveCompleted;
        }

        public async UniTaskVoid RunGameFlow()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_arenaSettings.FirstWaveDelay));
            
            _arenaModel.StartNextWave();
        }

        public void Reset()
        {
            _arenaModel.StopWave();
            _castle.Reset();
        }

        private void OnCastleDestroyed()
        {
            _arenaModel.StopWave();
            OnGameOver?.Invoke();
        }
        
        private void OnWaveCompleted()
        {
            HandleWaveCompletedAsync().Forget();
        }
        
        private async UniTaskVoid HandleWaveCompletedAsync()
        {
            if(_arenaModel.WaveStopped)
                return;
                
            Debug.Log($"Wave {_arenaModel.CurrentWaveIndex} completed");
            
            _arenaModel.AdvanceToNextWave();

            if (_arenaModel.IsLastWave)
            {
                OnGameWon?.Invoke();
                return;
            }
            
            await UniTask.Delay(TimeSpan.FromSeconds(_arenaSettings.WavesInterval));
                
            _arenaModel.StartNextWave();
        }
    }
}
