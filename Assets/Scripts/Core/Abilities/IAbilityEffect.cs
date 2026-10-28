namespace Core.Abilities
{
    public interface IAbilityEffect
    {
        void Apply(ICaster caster, ITarget target);
    }
}