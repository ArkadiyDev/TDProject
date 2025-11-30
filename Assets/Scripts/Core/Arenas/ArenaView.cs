using System.Collections.Generic;
using Core.Castles;
using UnityEngine;

namespace Core.Arenas
{
    public class ArenaView : MonoBehaviour
    {
        [SerializeField] private CastleView _castleView;
        [SerializeField] private List<Route> _routes;

        public CastleView CastleView => _castleView;
        public List<Route> Routes => _routes;
        
        private void OnDrawGizmos()
        {
            if(_routes.Count == 0)
                return;
            
            foreach (var route in _routes)
            {
                Gizmos.color = route.CastleView && route.Spawner ? Color.blue : Color.red;
                
                Waypoint lastWaypoint = null;
                
                foreach (var waypoint in route.Waypoints)
                {
                    if(lastWaypoint)
                        Gizmos.DrawLine(lastWaypoint.Position, waypoint.Position);

                    lastWaypoint = waypoint;
                }
                
                if(route.Spawner &&  route.TryGetFirstWaypoint(out var firstWaypoint))
                    Gizmos.DrawLine(firstWaypoint.Position, route.Spawner.transform.position);
                
                if(route.CastleView &&  lastWaypoint)
                    Gizmos.DrawLine(lastWaypoint.Position, route.CastleView.transform.position);
            } 

            Gizmos.color = Color.white;
        }
    }
}
