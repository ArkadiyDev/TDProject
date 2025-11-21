using System;
using System.Collections.Generic;
using GameState.Interfaces;

namespace GameState
{
    public class GameStateMachine : StateMachine
    {
        public GameStateMachine(SceneLoader sceneLoader, LoadingScreen loadingScreen)
        {
            _states = new Dictionary<Type, IExitableState>()
            {
                [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader),
                [typeof(LoadLevelState)] = new LoadLevelState(this, sceneLoader, loadingScreen),
                [typeof(GameCoreLoopState)] = new GameCoreLoopState(this),
            };
        }
    }
}