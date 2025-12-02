using Economy;
using Economy.Rewards;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Core.Invaders
{
    [CreateAssetMenu(fileName = "NewInvaderSettings", menuName = "TD/Settings/" + nameof(InvaderSettings))]
    public class InvaderSettings : ScriptableObject
    {
        [SerializeField] private float _baseHealth;
        [SerializeField] private float _damage;
        [SerializeField] private float _speed;
        [SerializeField] private AssetReference _assetReference;
        [SerializeField] private RewardData _rewards;

        public string Name => name;
        public float BaseHealth => _baseHealth;
        public float Damage => _damage;
        public float Speed => _speed;
        public AssetReference AssetReference => _assetReference;
        public RewardData Rewards => _rewards;
    }
}
