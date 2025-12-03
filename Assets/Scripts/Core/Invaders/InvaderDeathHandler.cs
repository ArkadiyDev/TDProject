using Economy.Rewards;

namespace Core.Invaders
{
    public class InvaderDeathHandler : IInvaderDeathHandler
    {
        private readonly IRewardProvider _rewardProvider;

        public InvaderDeathHandler(IRewardProvider rewardProvider)
        {
            _rewardProvider = rewardProvider;
        }

        public void InvaderDeathHandle(InvaderSettings settings)
        {
            _rewardProvider.ApplyReward(settings.Rewards);
        }
    }
}