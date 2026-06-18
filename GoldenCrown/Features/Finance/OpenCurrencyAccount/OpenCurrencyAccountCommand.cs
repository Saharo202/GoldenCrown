using GoldenCrown.Models;
using MediatR;

namespace GoldenCrown.Features.Finance.OpenCurrencyAccount
{
    public class OpenCurrencyAccountCommand : IRequest<Result>
    {
        public int UserId { get; set; }

        public Currency Currency { get; set; }

        public OpenCurrencyAccountCommand(
            int userId,
            Currency currency)
        {
            UserId = userId;
            Currency = currency;
        }
    }
}