using UnityEngine;

namespace Core.Invaders
{
    public class InvaderLink : MonoBehaviour
    {
        public IDamageable Damageable { get; private set; }

        public void Initialize(IDamageable damageable)
        {
            Damageable = damageable;
        }
        
        public void Reset()
        {
            Damageable = null;
        }
    }
}