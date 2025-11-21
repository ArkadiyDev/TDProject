using UnityEngine;

namespace Core.Castles
{
    [CreateAssetMenu(fileName = "NewCastleSettings", menuName = "TD/Settings/" + nameof(CastleSettings))]
    public class CastleSettings : ScriptableObject
    {
        [SerializeField] private  float _baseHealth;
        public float BaseHealth => _baseHealth;
    }
}