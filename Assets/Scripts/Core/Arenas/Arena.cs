using Core.Castles;
using Core.Invaders;
using UnityEngine;

namespace Core.Arenas
{
    public class Arena
    {
        private readonly ArenaModel _arenaModel;
        private readonly Castle _castle; 

        public Arena(ArenaView arenaView, ArenaSettings arenaSettings, InvaderFactory invaderFactory, CastleSettings castleSettings)
        {
            _arenaModel = new ArenaModel(arenaSettings, invaderFactory, arenaView.Routes);
            _castle = new Castle(castleSettings, arenaView.CastleView);

            _castle.Destroyed += OnCastleDestroyed;
        }

        public void RunWaves()
        {
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
