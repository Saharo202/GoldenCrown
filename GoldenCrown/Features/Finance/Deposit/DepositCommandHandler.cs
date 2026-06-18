using GoldenCrown.Database;
using GoldenCrown.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GoldenCrown.Features.Finance.Deposit
{
    public class DepositCommandHandler : IRequestHandler<DepositCommand, Result>
    {
        private readonly ApplicationDbContext _context;

        public DepositCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(DepositCommand request, CancellationToken cancellationToken)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(x => x.UserId == request.UserId && x.Currency == request.Currency);
            if (account == null)
            {
                return Result.Failure("Счёт не найден");
            }

            account!.Balance += request.Amount;
            _context.Transactions.Add(
            new Transaction
            {
                SenderAccountId = account.Id,
                ReceiverAccountId = account.Id,
                Currency = request.Currency,
                Type = TransactionType.Deposit,
                Amount = request.Amount,
                CreatedAt = DateTime.UtcNow
            });
            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Result.Failure(
                    "Баланс был изменен другим запросом");
            }
            return Result.Success();
        }
    }
}