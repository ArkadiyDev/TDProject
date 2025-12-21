using Core.Castles;
using Core.Invaders;
using UnityEngine;
using Zenject;

namespace Core.Arenas
{
    public class ArenaSystem : MonoBehaviour
    {
        [SerializeField] private ArenaView _arenaView;
        
        private Arena _arena;
        private ArenaRestartHandler _arenaRestartHandler;
        
        [Inject]
        private void Construct(ArenaSettings arenaSettings, InvaderFactory invaderFactory, InvaderSystem invaderSystem,
            CastleSettings castleSettings, ArenaRestartHandler arenaRestartHandler)
        {
            _arena = new Arena(_arenaView, arenaSettings, invaderFactory, castleSettings, invaderSystem);
            
            _arena.OnGameOver += OnArenaGameOver;
            _arena.OnGameWon += OnArenaGameWon;
            
            _arenaRestartHandler = arenaRestartHandler;
            
            RunWaves();
        }

        private void RunWaves()
        {
            _arena.RunGameFlow().Forget();
        }

        private void OnArenaGameOver()
        {
            Debug.Log("ArenaSystem: Game Over!");
            RestartArena();
        }
    
        private void OnArenaGameWon()
        {
            Debug.Log("ArenaSystem: Victory!");
            RestartArena();
        }

        private void RestartArena()
        {
            _arenaRestartHandler.Restart();
            _arena.Reset();
            
            _arena.RunGameFlow().Forget();
        }
    }
}
