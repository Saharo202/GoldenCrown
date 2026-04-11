namespace GoldenCrown.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int RecieverId { get; set; }
        public DateTime CreatedAt  { get; set; }        
        public decimal Amount { get; set; }
    }
}
