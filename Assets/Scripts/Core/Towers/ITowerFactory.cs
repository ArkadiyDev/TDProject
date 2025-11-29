using UnityEngine;

namespace Core.Towers
{
    public interface ITowerFactory
    {
        Tower CreateTower(Vector3 position);
    }
}