namespace Common.GameSpeed
{
    public interface IGameSpeedService
    {
        bool IsPaused { get; }
        void SetPaused(bool paused);
        void SetNormalSpeed();
        void SetDoubleSpeed();
        void SetTripleSpeed();
    }
}