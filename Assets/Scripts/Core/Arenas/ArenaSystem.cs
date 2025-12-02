using System;
using Core.Castles;
using Core.Invaders;
using Economy;
using Economy.Wallets;
using UnityEngine;
using Zenject;

namespace Core.Arenas
{
    public class ArenaSystem : MonoBehaviour
    {
        [SerializeField] private ArenaView _arenaView;
        
        private Arena _arena;
        private IWalletService _walletService;
        
        [Inject]
        private void Construct(ArenaSettings arenaSettings, InvaderFactory invaderFactory, IWalletService walletService,
            CastleSettings castleSettings)
        {
            _arena = new Arena(_arenaView, arenaSettings, invaderFactory, castleSettings);
            
            _arena.OnGameOver += OnArenaGameOver;
            _arena.OnGameWon += OnArenaGameWon;

            _walletService = walletService;
            _walletService.CurrencyChanged += OnCurrencyChanged;
            
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

        private void OnCurrencyChanged(string id, int delta)
        {
            Debug.Log($"Currency {id} changed, delta: {delta}, current value: {_walletService.GetCurrency(id)}");
        }
    }
}
