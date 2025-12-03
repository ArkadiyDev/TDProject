using Economy.Wallets;

namespace Economy.Rewards
{
    public class ArenaRewardProvider : IRewardProvider
    {
        private readonly IWalletService _walletService;
        
        public ArenaRewardProvider(IWalletService walletService)
        {
            _walletService = walletService;
        }
        
        public void ApplyReward(RewardData rewardData)
        {
            if (rewardData?.Currencies == null)
                return;

            _walletService.IncreaseCurrencies(rewardData.Currencies);
        }
    }
}