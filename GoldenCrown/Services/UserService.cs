using GoldenCrown.Database;
using Microsoft.EntityFrameworkCore;
using GoldenCrown.Models;

namespace GoldenCrown.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IAccountService _accountService;

        public UserService(ApplicationDbContext context, IAccountService accountService)
        {
            _context = context;
            _accountService = accountService;
        }
        public async Task<Result> RegisterAsync(string login, string name, string password)
        {
            var existing = await _context.Users.FirstOrDefaultAsync(x => x.Login == login);
            if (existing != null)
            {
                return Result.Failure("User already exists");
            }

            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            {
                return Result.Failure("Invalid password");
            }

            var user = new User
            {
                Login = login,
                Name = name,
                Password = password
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            await _accountService.CreateAccountAsync(login);

            return Result.Success();

        }

    }
}
