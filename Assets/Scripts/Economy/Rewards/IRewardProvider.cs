namespace Economy.Rewards
{
    public interface IRewardProvider
    {
        void ApplyReward(RewardData rewardData);
    }
}