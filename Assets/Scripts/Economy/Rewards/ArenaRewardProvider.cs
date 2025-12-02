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
            if (rewardData == null) return;

            foreach (var data in rewardData.Currencies)
            {
                if (!data.Settings)
                    continue;
                
                _walletService.IncreaseCurrency(data.Settings.Id, data.Amount);
            }
        }
    }
}