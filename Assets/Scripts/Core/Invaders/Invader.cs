using System;
using Core.Arenas;
using UnityEngine;

namespace Core.Invaders
{
    public class Invader
    {
        public event Action<Invader> Removed;
        
        private readonly InvaderView _view;
        private readonly InvaderSettings _invaderSettings;
        private readonly Action<InvaderView> _onRemove;
        
        private Route _route;
        private Waypoint _currentWaypoint;

        public string Name => _invaderSettings.name;
        public float Damage => _invaderSettings.Damage;
        private float Speed => _invaderSettings.Speed;
        

        public Invader(InvaderSettings invaderSettings, InvaderView view, Action<InvaderView> onRemove)
        {
            _invaderSettings = invaderSettings;
            _view = view;
            _onRemove = onRemove;

            _view.MoveComplete += OnMoveCompeted;
        }

        public void SetRoute(Route route)
        {
            _route = route;
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
            if (_route.TryGetNextWaypoints(_currentWaypoint, out var nextWaypoint))
                SetCurrentWaypoint(nextWaypoint);
            else
                HandleReachedLastWaypoint();
        }

        private void OnMoveCompeted()
        {
            MoveToNextWaypoint();
        }

        private void HandleReachedLastWaypoint()
        {
            if (_route.CastleView)
                _route.CastleView.Enter(this);
            
            OnRemove();
        }

        private void SetCurrentWaypoint(Waypoint waypoint)
        {
            _currentWaypoint = waypoint;
            _view.MoveTo(_currentWaypoint.transform.position, Speed);
        }

        private void OnRemove()
        {
            _view.MoveComplete -= OnMoveCompeted;
            _onRemove?.Invoke(_view);
            
            Removed?.Invoke(this);
        }
    }
}
