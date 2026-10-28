namespace Core.Abilities
{
    public abstract class AbilityTargeting
    {
        protected AbilitySettings _abilitySettings;
        protected TargetingProcessor _targetingProcessor;
        protected bool _isTargeting;

        public bool IsTargeting => _isTargeting;

        public abstract void Start(AbilitySettings abilitySettings, TargetingProcessor targetingProcessor);
        public virtual void Update() {}
        public virtual void Stop() {}
    }
}