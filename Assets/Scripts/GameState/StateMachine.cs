using Assets.Scripts.GameState.Interfaces;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.GameState
{
    public abstract class StateMachine
    {
        protected Dictionary<Type, IExitableState> _states;
        private IExitableState _activeState;

        public void SetState(IState state)
        {
            _activeState?.Exit();
            _activeState = state;
            state.Enter();
        }

        public void Enter<TState>() where TState : class, IState
        {
            var state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            var state = ChangeState<TState>();
            state.Enter(payload);
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activeState?.Exit();

            var state = GetState<TState>();
            _activeState = state;

            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState =>
            _states[typeof(TState)] as TState;
    }
}