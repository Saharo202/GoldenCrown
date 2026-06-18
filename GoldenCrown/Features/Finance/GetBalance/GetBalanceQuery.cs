using MediatR;
using static GoldenCrown.Features.Finance.GetBalance.GetBalanceQueryHandler;

namespace GoldenCrown.Features.Finance.GetBalance
{
    public class GetBalanceQuery : IRequest<Result<List<BalanceItem>>>
    {
        public int UserId { get; set; }
        public GetBalanceQuery(int userId)
        {
            UserId = userId;
        }
    }
}