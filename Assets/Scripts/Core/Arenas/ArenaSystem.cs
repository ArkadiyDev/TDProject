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
        
        [Inject]
        private void Construct(ArenaSettings arenaSettings, InvaderFactory invaderFactory, CastleSettings castleSettings)
        {
            _arena = new Arena(_arenaView, arenaSettings, invaderFactory, castleSettings);
            
            _arena.OnGameOver += OnArenaGameOver;
            _arena.OnGameWon += OnArenaGameWon;
            
            RunWaves();
        }

        private void RunWaves()
        {
            _arena.RunGameFlow().Forget();
        }

        private void OnArenaGameOver()
        {
            Debug.Log("ArenaSystem: Game Over!");
        }
    
        private void OnArenaGameWon()
        {
            Debug.Log("ArenaSystem: Victory!");
        }
    }
}
