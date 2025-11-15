using Assets.Scripts.GameState.Interfaces;

namespace Assets.Scripts.GameState
{
    internal class GameCoreLoopState : IState
    {
        private readonly GameStateMachine _stateMachine;

        public GameCoreLoopState(GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
        }

        public void Exit()
        {
        }
    }
}