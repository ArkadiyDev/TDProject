using UnityEngine;

namespace Common.GameSpeed
{
    public class GameSpeedService : IGameSpeedService
    {
        public bool IsPaused => Mathf.Approximately(Time.timeScale, 0);

        public void SetPaused(bool paused)
        {
            SetSpeed(paused ? 0 : 1);
        }

        public void SetNormalSpeed()
        {
            SetSpeed(1);
        }
        
        public void SetDoubleSpeed()
        {
            SetSpeed(2);
        }
        
        public void SetTripleSpeed()
        {
            SetSpeed(3);
        }
        
        private void SetSpeed(float speed)
        {
            Time.timeScale = speed;
            Debug.Log("Game speed changed to: " + speed);
        }
    }
}