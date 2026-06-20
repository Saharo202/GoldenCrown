using GoldenCrown.Models;
using MediatR;

namespace GoldenCrown.Features.Finance.ConvertCurrency
{
    public class ConvertCurrencyCommand
        : IRequest<Result>
    {
        public int UserId { get; }

        public Currency FromCurrency { get; }

        public Currency ToCurrency { get; }

        public decimal Amount { get; }

        public ConvertCurrencyCommand(
            int userId,
            Currency fromCurrency,
            Currency toCurrency,
            decimal amount)
        {
            UserId = userId;
            FromCurrency = fromCurrency;
            ToCurrency = toCurrency;
            Amount = amount;
        }
    }
}