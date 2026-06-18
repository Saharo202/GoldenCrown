using GoldenCrown.Models;
using MediatR;

namespace GoldenCrown.Features.Finance.Deposit
{
    public class DepositCommand : IRequest<Result>
    {
        public int UserId { get; }

        public Currency Currency { get; }

        public decimal Amount { get; }

        public DepositCommand(
            int userId,
            Currency currency,
            decimal amount)
        {
            UserId = userId;
            Currency = currency;
            Amount = amount;
        }
    }
}