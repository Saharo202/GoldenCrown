namespace GoldenCrown.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        public int SenderAccountId { get; set; }

        public int ReceiverAccountId { get; set; }

        public Currency Currency { get; set; }

        public TransactionType Type { get; set; }

        public decimal Amount { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public enum TransactionType
    {
        Deposit,
        Transfer,
        Conversion
    }
}
