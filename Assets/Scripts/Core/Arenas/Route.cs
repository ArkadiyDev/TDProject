using System.Collections.Generic;

namespace Core.Arenas
{
    public class Route
    {
        public List<Waypoint> _waypoints;

        public Route(List<Waypoint> waypoints)
        {
            _waypoints = waypoints;
        }
    }
}
