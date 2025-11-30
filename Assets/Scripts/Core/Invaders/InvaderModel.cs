using Core.Arenas;

namespace Core.Invaders
{
    public class InvaderModel
    {
        private readonly InvaderSettings _invaderSettings;
        
        private Route _route;
        private Waypoint _currentWaypoint;
        private float _health;
        private bool _IsDead;

        public string Name => _invaderSettings.Name;
        public float Damage => _invaderSettings.Damage;
        public float Speed => _invaderSettings.Speed;
        public float Health => _health;
        public float MaxHealth => _invaderSettings.BaseHealth;
        public bool IsAlive => !_IsDead;
        public Route Route => _route;
        
        public InvaderModel(InvaderSettings invaderSettings)
        {
            _invaderSettings = invaderSettings;
            
            _health = _invaderSettings.BaseHealth;
        }
        
        public void ReduceHealth(float damageAmount)
        {
            _health -= damageAmount;

            if (_health > 0)
                return;
            
            _health = 0;
            _IsDead = true;
        }
        
        public void SetRoute(Route route)
        {
            _route = route;
        }
        
        public void SetCurrentWaypoint(Waypoint waypoint)
        {
            _currentWaypoint = waypoint;
        }

        public bool TryGetNextWaypoint(out Waypoint nextWaypoint)
        {
            if(_route.TryGetNextWaypoints(_currentWaypoint, out var waypoint))
            {
                nextWaypoint = waypoint;
                return true;
            }

            nextWaypoint = null;
            return false;
        }
    }
}