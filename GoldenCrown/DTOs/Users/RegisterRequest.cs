using System.ComponentModel.DataAnnotations;

namespace GoldenCrown.DTOs.Users
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Поле login обязательно")]
        [MinLength(3,ErrorMessage = "Минимальная длина поля login - 3 символа")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Поле password обязательно")]
        [MinLength(6,ErrorMessage = "Минимальная длина пароля - 6 символов")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Поле name обязательно")]
        public string Name { get; set; }
    }
}
