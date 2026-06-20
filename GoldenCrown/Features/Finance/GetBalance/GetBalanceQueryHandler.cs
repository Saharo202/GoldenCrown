using GoldenCrown.Database;
using GoldenCrown.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GoldenCrown.Features.Finance.GetBalance
{
    public class GetBalanceQueryHandler :
        IRequestHandler<GetBalanceQuery, Result<List<BalanceItem>>>
    {
        private readonly ApplicationDbContext _context;

        public GetBalanceQueryHandler(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<BalanceItem>>> Handle(
            GetBalanceQuery request,
            CancellationToken cancellationToken)
        {
            var accounts = await _context.Accounts
                .Where(x => x.UserId == request.UserId)
                .Select(x => new BalanceItem
                {
                    Currency = x.Currency,
                    Balance = x.Balance
                })
                .ToListAsync(cancellationToken);

            return Result<List<BalanceItem>>
                .Success(accounts);
        }
    }

    public class BalanceItem
    {
        public Currency Currency { get; set; }

        public decimal Balance { get; set; }
    }
}