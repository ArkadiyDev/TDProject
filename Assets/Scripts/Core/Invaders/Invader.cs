using System;
using Core.Arenas;
using Economy.Rewards;
using UnityEngine;

namespace Core.Invaders
{
    public class Invader : IDamageable
    {
        public event Action<InvaderSettings> Died; 
        public event Action<Invader> Removed;

        private readonly InvaderView _view;
        private readonly InvaderLink _link;
        private readonly InvaderModel _model;

        public string Name => _model.Name;
        public float Damage => _model.Damage;
        private float Speed => _model.Speed;
        public bool IsAlive => _model.IsAlive;
        public RewardData Rewards => _model.Rewards;
        public Transform BodyPoint => _view.BodyTargetPoint;
        public InvaderView View => _view;
        
        public Invader(InvaderSettings invaderSettings, InvaderView view)
        {
            _model = new InvaderModel(invaderSettings);
            _view = view;

            _view.MoveComplete += OnMoveCompeted;
            
            _link = _view.gameObject.GetComponent<InvaderLink>();
            _link.Initialize(this);
        }

        public void SetRoute(Route route)
        {
            _model.SetRoute(route);
        }

        public void SetStartPosition(Vector3 position)
        {
            _view.transform.position = position;
        }

        public void SetActiveView(bool active)
        {
            _view.gameObject.SetActive(active);
        }

        public void SetStartWaypoint(Waypoint waypoint)
        {
            _model.SetCurrentWaypoint(waypoint);
            _view.MoveTo(waypoint.transform.position, Speed);
        }

        public void MoveToNextWaypoint()
        {
            if (_model.TryGetNextWaypoint(out var nextWaypoint))
            {
                _model.SetCurrentWaypoint(nextWaypoint);
                _view.MoveTo(nextWaypoint.transform.position, Speed);
            }
            else
                HandleReachedLastWaypoint();
        }
        
        public void TakeDamage(float damageAmount)
        {
            if(!IsAlive)
                return;
            
            Debug.Log($"{Name} take damage {damageAmount}");
            _model.ReduceHealth(damageAmount);
            
            if(IsAlive)
                return;
            
            Debug.Log($"{Name} died");
            
            Died?.Invoke(_model.Settings);
            _view.StopMoving();
            Remove();
        }

        private void OnMoveCompeted()
        {
            MoveToNextWaypoint();
        }

        private void HandleReachedLastWaypoint()
        {
            if (_model.Route.CastleView)
                _model.Route.CastleView.Enter(this);
            
            Remove();
        }

        private void Remove()
        {
            _link.Reset();
            _view.MoveComplete -= OnMoveCompeted;
            
            Removed?.Invoke(this);
        }
    }
}
