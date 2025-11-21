using System.Collections.Generic;
using UnityEngine;

namespace Core.Arenas
{
    public class ArenaView : MonoBehaviour
    {
        [SerializeField] private List<Waypoint> _waypoints;
        [SerializeField] private List<Spawner> _spawners;

        public List<Spawner> Spawners => _spawners;
    }
}
