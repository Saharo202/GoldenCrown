using GoldenCrown.Database;
using GoldenCrown.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GoldenCrown.Features.Finance.ConvertCurrency
{
    public class ConvertCurrencyCommandHandler : IRequestHandler<ConvertCurrencyCommand, Result>
    {
        private readonly ApplicationDbContext _context;

        public ConvertCurrencyCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle( ConvertCurrencyCommand request, CancellationToken cancellationToken)
        {
            if (request.FromCurrency == request.ToCurrency)
            {
                return Result.Failure("Валюты должны различаться");
            }

            await using var trx = await _context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                if (request.Amount <= 0)
                {
                    return Result.Failure("Сумма должна быть больше нуля");
                }

                var fromAccount =
                    await _context.Accounts.FirstOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.Currency == request.FromCurrency);

                if (fromAccount == null)
                {
                    return Result.Failure("Исходный счет не найден");
                }

                var toAccount =
                    await _context.Accounts.FirstOrDefaultAsync(x =>
                    x.UserId == request.UserId &&
                    x.Currency == request.ToCurrency);

                if (toAccount == null)
                {
                    return Result.Failure("Целевой счет не найден");
                }

                if (fromAccount.Balance < request.Amount)
                {
                    return Result.Failure("Недостаточно средств");
                }

                var rate =
                    await _context.ExchangeRates.FirstOrDefaultAsync(x =>
                    x.FromCurrency == request.FromCurrency &&
                    x.ToCurrency == request.ToCurrency);

                if (rate == null)
                {
                    return Result.Failure("Курс не найден");
                }

                var convertedAmount = decimal.Round(request.Amount * rate.Rate,2,MidpointRounding.ToEven);

                fromAccount.Balance -= request.Amount;

                toAccount.Balance += convertedAmount;

                _context.Transactions.Add(new Transaction
                    {
                        SenderAccountId = fromAccount.Id,
                        ReceiverAccountId = toAccount.Id,

                        Currency = request.ToCurrency,

                        Type = TransactionType.Conversion,

                        Amount = convertedAmount,

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

                await trx.CommitAsync(cancellationToken);

                return Result.Success();
            }
            catch
            {
                await trx.RollbackAsync(cancellationToken);

                throw;
            }
        }
    }
}