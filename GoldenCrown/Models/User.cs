namespace GoldenCrown.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; } = string.Empty; // или = null!;
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
