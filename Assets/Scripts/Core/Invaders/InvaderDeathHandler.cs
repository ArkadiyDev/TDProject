using Common.UI.FloatingText;
using Economy.Rewards;

namespace Core.Invaders
{
    public class InvaderDeathHandler : IInvaderDeathHandler
    {
        private readonly IRewardProvider _rewardProvider;
        private readonly IFloatingTextService _floatingTextService;

        public InvaderDeathHandler(IRewardProvider rewardProvider, IFloatingTextService floatingTextService)
        {
            _rewardProvider = rewardProvider;
            _floatingTextService = floatingTextService;
        }

        public void InvaderDeathHandle(Invader invader)
        {
            _rewardProvider.ApplyReward(invader.Rewards);

            foreach (var currency in invader.Rewards.Currencies)
                _floatingTextService.ShowText(invader.BodyPoint.position, "+" + currency.Amount);
        }
    }
}