namespace Core.Invaders
{
    public class InvaderModel
    {
        public InvaderSettings Settings { get; }
        public bool IsDead { get; private set; }
        public float Health { get; private set; }

        public float MaxHealth => Settings.BaseHealth;
        public string Name => Settings.Name;
        public float Damage => Settings.Damage;
        public float Speed => Settings.Speed;
        
        public InvaderModel(InvaderSettings invaderSettings)
        {
            Settings = invaderSettings;
            Health = Settings.BaseHealth;
        }
        
        public void TakeDamage(float damageAmount)
        {
            Health -= damageAmount;

            if (Health > 0)
                return;
            
            Health = 0;
            IsDead = true;
        }
    }
}