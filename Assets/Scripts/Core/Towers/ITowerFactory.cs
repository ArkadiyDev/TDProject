using System.Collections.Generic;
using Economy.Currencies;
using UnityEngine;

namespace Core.Towers
{
    public interface ITowerFactory
    {
        Tower CreateTower(Vector3 position);
        List<CurrencyData> GetTowerCost();
    }
}