using UnityEngine;

namespace Core.Abilities
{
    public interface ITarget
    {
        Transform Transform { get; }
        bool IsDead { get; }
    }
}