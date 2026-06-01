using GoldenCrown.Database;
using Microsoft.EntityFrameworkCore;

namespace GoldenCrown.Services
{
    public class FinanceService : IFinanceService
    {
        private readonly ApplicationDbContext _dbContext;
        
        public FinanceService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<decimal>> GetBalanceAsync(string token)
        {
            var session = await _dbContext.Sessions.FirstOrDefaultAsync(s => s.Token == token);
            if (session == null)
            {
                return Result<decimal>.Failure("Пользователь не авторизован");
            }

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == session.UserId);
            var account = await _dbContext.Accounts.FirstOrDefaultAsync(a => a.UserId == user!.Id);
            return Result<decimal>.Success(account!.Balance);
        }

        public async Task<Result> DepositAsync(string token, decimal amount)
        {
            var session = await _dbContext.Sessions.FirstOrDefaultAsync(s => s.Token == token);
            if(session == null)
            {
                return Result.Failure("Пользователь не авторизован");
            }
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == session.UserId);
            var account = await _dbContext.Accounts.FirstOrDefaultAsync(a => a.UserId == user!.Id);

            account!.Balance += amount;
            await _dbContext.SaveChangesAsync();
            return Result.Success();
        }
    }
}
