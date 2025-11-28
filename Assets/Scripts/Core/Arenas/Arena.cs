using System;
using Core.Castles;
using Core.Invaders;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Arenas
{
    public class Arena
    {
        private readonly ArenaSettings _arenaSettings;
        private readonly ArenaModel _arenaModel;
        private readonly Castle _castle; 

        public Arena(ArenaView arenaView, ArenaSettings arenaSettings, InvaderFactory invaderFactory, CastleSettings castleSettings)
        {
            _arenaSettings = arenaSettings;
            _arenaModel = new ArenaModel(arenaSettings, invaderFactory, arenaView.Routes);
            _castle = new Castle(castleSettings, arenaView.CastleView);

            _castle.Destroyed += OnCastleDestroyed;
        }

        public async void RunWaves()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_arenaSettings.FirstWaveDelay));
            
            _arenaModel.StartNextWave();
        }

        private void OnCastleDestroyed()
        {
            _arenaModel.StopWave();
            Debug.Log("Game Over");
            _castle.Destroyed -= OnCastleDestroyed;
        }
    }
}
