using GoldenCrown.Models;

namespace GoldenCrown.Features.Finance.ConvertCurrency
{
    public class ConvertCurrencyRequest
    {
        public Currency FromCurrency { get; set; }

        public Currency ToCurrency { get; set; }

        public decimal Amount { get; set; }
    }
}