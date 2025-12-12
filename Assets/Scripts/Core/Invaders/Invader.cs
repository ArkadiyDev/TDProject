using System;
using Core.Arenas;
using Core.Damaging;
using Economy.Rewards;
using UnityEngine;

namespace Core.Invaders
{
    public class Invader : IDamageable
    {
        public event Action<Invader> Died; 
        public event Action<Invader> Removed;

        private readonly InvaderView _view;
        private readonly InvaderLink _link;
        private readonly InvaderModel _model;
        private readonly InvaderMovementProcessor _movementProcessor;
        
        private Route _route;
        private Waypoint _currentWaypoint;
        private bool _isMoving;

        public string Name => _model.Name;
        public float Damage => _model.Damage;
        public bool IsDead => _model.IsDead;
        public Transform BodyPoint => _view.BodyTargetPoint;
        public InvaderView View => _view;
        public RewardData Rewards => _model.Settings.Rewards;


        public Invader(InvaderSettings invaderSettings, InvaderView view)
        {
            _model = new InvaderModel(invaderSettings);
            _view = view;
            
            _movementProcessor = new InvaderMovementProcessor(_view, _model);
            _movementProcessor.ReachedLastWaypoint += OnReachedLastWaypoint;
            
            _link = _view.gameObject.GetComponent<InvaderLink>();
            _link.Initialize(this);
        }

        public void SetStartPosition(Vector3 position) =>
            _view.transform.position = position;

        public void StartRoute(Route route)
        {
            _route = route;
            _movementProcessor.StartRoute(route);
        }

        public void SetActiveView(bool active) =>
            _view.gameObject.SetActive(active);

        public void Tick(float deltaTime)
        {
            _movementProcessor.ProcessMovement(deltaTime);
        }

        private void OnReachedLastWaypoint()
        {
            if (_route.CastleView)
                _route.CastleView.Enter(this);
            
            Remove();
        }

        public void TakeDamage(float damageAmount)
        {
            if(IsDead)
                return;
            
            Debug.Log($"{Name} take damage {damageAmount}");
            _model.TakeDamage(damageAmount);
            
            if(!IsDead)
                return;
            
            Debug.Log($"{Name} died");
            
            Died?.Invoke(this);
            Remove();
        }

        private void Remove()
        {
            _link.Reset();
            
            Removed?.Invoke(this);
        }
    }
}
