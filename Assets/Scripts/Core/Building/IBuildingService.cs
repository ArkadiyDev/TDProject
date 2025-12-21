namespace Core.Building
{
    public interface IBuildingService
    {
        bool IsBuildingState { get; }
        void SwitchBuildingMode();
        void TryBuildTower();
        void RemoveBuilding(IBuilding building);
    }
}