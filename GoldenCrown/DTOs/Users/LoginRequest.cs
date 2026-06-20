using System.ComponentModel.DataAnnotations;

namespace GoldenCrown.DTOs.Users
{
    public class LoginRequest
    {
        public string Login { get; set; }

        public string Password { get; set; }
    }
}
