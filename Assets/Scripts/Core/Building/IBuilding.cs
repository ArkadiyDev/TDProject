using System;

namespace Core.Building
{
    public interface IBuilding
    {
        event Action Demolished;
        void Tick(float deltaTime);
        void Demolish();
    }
}