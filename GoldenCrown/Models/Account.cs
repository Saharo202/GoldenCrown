namespace GoldenCrown.Models
{
    public class Account
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public Currency Currency { get; set; }
        public decimal Balance {  get; set; }
        public byte[] RowVersion { get; set; }
    }
}
