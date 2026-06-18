using GoldenCrown.Database;
using GoldenCrown.DTOs.Finance;
using GoldenCrown.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GoldenCrown.Features.Finance.GetTransactionHistory
{
    public class GetTransactionHistoryQueryHandler : IRequestHandler<GetTransactionHistoryQuery, Result<List<TransactionHistoryResponse>>>
    {
        private readonly ApplicationDbContext _context;
        public GetTransactionHistoryQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<TransactionHistoryResponse>>> Handle(GetTransactionHistoryQuery request, CancellationToken cancellationToken)
        {
            if(request.DateFrom != null && request.DateTo != null && request.DateFrom > request.DateTo)
            {
                return Result<List<TransactionHistoryResponse>>.Failure("Некорректный диапазон дат");
            }
            var accountIds = await _context.Accounts.Where(
                x => x.UserId == request.UserId )
                .Select(x => x.Id)
                .ToListAsync();

            var transaction = _context.Transactions.Where(x =>
                accountIds.Contains(x.SenderAccountId) ||
                accountIds.Contains(x.ReceiverAccountId));

            if (request.DateFrom != null)
            {
                transaction = transaction.Where(x => x.CreatedAt >= request.DateFrom.Value);
            }
            if (request.DateTo != null)
            {
                transaction = transaction.Where(x => x.CreatedAt <= request.DateTo.Value);
            }
            transaction = transaction.OrderByDescending(x => x.CreatedAt);

            transaction = transaction.Skip(request.Skip).Take(request.Take);
            var dbTransaction = await transaction.ToListAsync(cancellationToken);

            var allAccountIds = dbTransaction.Select(x => x.SenderAccountId)
                .Concat(dbTransaction.Select(x => x.ReceiverAccountId))
                .ToHashSet();

            var names = await _context.Accounts
                .Where(x => allAccountIds.Contains(x.Id))
                .Join(_context.Users,
                acc => acc.UserId,
                u => u.Id,
                (acc, u) => new
                {
                    Name = u.Name,
                    AccId = acc.Id
                })
                .ToDictionaryAsync(x => x.AccId, cancellationToken);

            var result = dbTransaction.Select(t => new TransactionHistoryResponse
            {
                SenderName = names[t.SenderAccountId].Name,
                ReceiverName = names[t.ReceiverAccountId].Name,
                Amount = t.Amount,
                Date = t.CreatedAt,
                Type = t.Type
            }).ToList();
            
            return Result<List<TransactionHistoryResponse>>.Success(result);

        }

    }
}
