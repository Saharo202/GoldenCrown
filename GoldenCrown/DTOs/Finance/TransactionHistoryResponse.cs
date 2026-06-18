using GoldenCrown.Models;

namespace GoldenCrown.DTOs.Finance
{
    public class TransactionHistoryResponse
    {
        public string SenderName { get; set; }

        public string ReceiverName { get; set; }

        public Currency Currency { get; set; }

        public TransactionType Type { get; set; }

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }
    }
}
