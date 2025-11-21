using GameState.Interfaces;

namespace GameState
{
    public class BootstrapState : IState
    {
        private const string Startup = "Startup";
        private const string Gameplay = "Gameplay";

        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;

        public BootstrapState(GameStateMachine gameStateMachine, SceneLoader sceneLoader)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
        }

        public void Enter()
        {
            RegisterServices();
            _sceneLoader.Load(Startup, onLoaded: EnterLoadLevel);
        }

        private void EnterLoadLevel() =>
            _gameStateMachine.Enter<LoadLevelState, string>(Gameplay);

        public void Exit()
        {
        }

        private void RegisterServices()
        {
            //TODO: InputService
        }
    }
}