using System;
using Core.Arenas;
using UnityEngine;

namespace Core.Invaders
{
    public class InvaderMovementProcessor
    {
        public event Action ReachedLastWaypoint;
        
        private readonly InvaderView _view;
        private readonly InvaderModel _model;
    
        private Route _route;
        private Waypoint _currentWaypoint;
        private bool _isMoving;

        public InvaderMovementProcessor(InvaderView view, InvaderModel model)
        {
            _view = view;
            _model = model;
        }

        public void StartRoute(Route route)
        {
            _route = route;

            MoveToNextWaypoint();
        }
        
        public void StopMoving()
            => _isMoving = false;

        public void ProcessMovement(float deltaTime)
        {
            if (!_isMoving || _model.IsDead)
                return;

            var currentPosition = _view.transform.position;
            var targetPosition = _currentWaypoint.transform.position;
            var newPosition = Vector3.MoveTowards(currentPosition, targetPosition, _model.Speed * deltaTime);

            _view.SetPosition(newPosition);

            if (Mathf.Approximately(Vector3.Distance(newPosition, targetPosition), 0))
                OnMoveCompeted();
        }

        private void OnMoveCompeted()
        {
            _isMoving = false;
            MoveToNextWaypoint();
        }

        private void MoveToNextWaypoint()
        {
            if (_route.TryGetNextWaypoints(_currentWaypoint, out var nextWaypoint))
            {
                _currentWaypoint = nextWaypoint;
                _isMoving = true;
            }
            else
            {
                ReachedLastWaypoint?.Invoke();
            }
        }
    }
}