using Assets.Scripts.GameState.Interfaces;

namespace Assets.Scripts.GameState
{
    public abstract class State : IState
    {
        public abstract void Enter();

        public abstract void Exit();
    }
}