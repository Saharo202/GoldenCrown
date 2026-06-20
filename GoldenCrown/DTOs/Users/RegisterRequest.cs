using System.ComponentModel.DataAnnotations;

namespace GoldenCrown.DTOs.Users
{
    public class RegisterRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
    }
}
