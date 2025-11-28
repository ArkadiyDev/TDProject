namespace Common.GameSpeed
{
    public interface IGameSpeedService
    {
        bool IsPaused { get; }
        void SetPaused();
    }
}