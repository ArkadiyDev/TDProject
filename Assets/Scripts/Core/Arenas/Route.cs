using System;
using System.Collections.Generic;
using System.Linq;
using Core.Castles;
using UnityEngine;

namespace Core.Arenas
{
    [Serializable]
    public class Route
    {
        [SerializeField] private Spawner _spawner;
        [SerializeField] private List<Waypoint> _waypoints;
        [SerializeField] private CastleView _castleView;

        public Spawner Spawner => _spawner;
        public List<Waypoint> Waypoints => _waypoints;
        public CastleView CastleView => _castleView;

        public bool TryGetFirstWaypoint(out Waypoint firstWaypoint)
        {
            firstWaypoint = null;

            if (_waypoints.Count == 0)
            {
                Debug.Log($"First waypoint not found");
                return false;
            }
            
            firstWaypoint = _waypoints.First();
            return true;
        }

        public bool TryGetNextWaypoints(Waypoint currentWaypoint, out Waypoint nextWaypoint)
        {
            nextWaypoint = null;
            
            if(_waypoints.Count == 0)
                return false;
            
            if(currentWaypoint == _waypoints.Last())
                return false;
            
            if (!currentWaypoint)
            {
                nextWaypoint = _waypoints.First();
                return true;
            }
            
            var index = _waypoints.IndexOf(currentWaypoint);
            if (index + 1 >= _waypoints.Count)
                return false;
            
            nextWaypoint = _waypoints[index + 1];
            return true;
        }
    }
}
