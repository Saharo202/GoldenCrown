using GoldenCrown.Database;
using GoldenCrown.Models;

using Microsoft.EntityFrameworkCore;

namespace GoldenCrown.Services
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _context;
        private readonly IAccountService _accountService;

        public AccountService(ApplicationDbContext context, IAccountService accountService)
        {
            _context = context;
            _accountService = accountService;
        }

        public async Task CreateAccountAsync(string login)
        {            
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Login == login);
            if (user == null)
            {
                throw new InvalidOperationException($"Unable to find a user with login: {login}");
            }

            var account = new Account 
            { 
                UserId = user.Id,
                Balance = 0,
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            await _accountService.CreateAccountAsync(login);

        }
    }
}
