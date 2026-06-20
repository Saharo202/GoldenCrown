using GoldenCrown.Features.Finance.GetBalance;

namespace GoldenCrown.DTOs.Finance
{
    public class BalanceResponce
    {
        public List<BalanceItem> Accounts { get; set; }
    }
}