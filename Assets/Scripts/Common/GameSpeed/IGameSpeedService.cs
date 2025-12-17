namespace Common.GameSpeed
{
    public interface IGameSpeedService
    {
        void SwitchPause();
        void SetPaused();
        void SetUnpaused();
        void SetSpeed(GameSpeed speed);
    }
}