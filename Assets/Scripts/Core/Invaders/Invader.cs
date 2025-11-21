using System;
using Core.Arenas;
using UnityEngine;

namespace Core.Invaders
{
    public class Invader
    {
        public event Action<Invader> Removed;
        
        private float _health;
        private InvaderView _view;
        private Waypoint _currentWaypoint;
        private InvaderSettings _invaderSettings;
        private Action<InvaderView> _onRemove;

        public Invader(InvaderSettings invaderSettings, InvaderView view, Action<InvaderView> onRemove)
        {
            _invaderSettings = invaderSettings;
            _health = invaderSettings.BaseHealth;
            _view = view;
            _onRemove = onRemove;

            _view.MoveComplete += OnMoveCompeted;
        }

        private void OnMoveCompeted()
        {
            MoveToNextWaypoint();
        }

        public void SetStartPosition(Vector3 position)
        {
            _view.transform.position = position;
        }

        public void SetStartWaypoint(Waypoint waypoint)
        {
            SetCurrentWaypoint(waypoint);
        }

        public void MoveToNextWaypoint()
        {
            if (_currentWaypoint.TryGetNextWaypoints(out var waypoint))
                SetCurrentWaypoint(waypoint);
            else
                HandleReachedCastle();
        }

        private void HandleReachedCastle()
        {
            Debug.Log($"{_view.name} dealt {_invaderSettings.Damage} damage to the Castle");

            OnRemove();
        }

        private void SetCurrentWaypoint(Waypoint waypoint)
        {
            _currentWaypoint = waypoint;
            _view.MoveTo(_currentWaypoint.transform.position, _invaderSettings.Speed);
        }

        private void OnRemove()
        {
            _view.MoveComplete -= OnMoveCompeted;
            _onRemove?.Invoke(_view);
            
            Removed?.Invoke(this);
        }
    }
}
