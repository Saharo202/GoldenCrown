using GoldenCrown.Database;
using GoldenCrown.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GoldenCrown.Features.Finance.Transfer
{
    public class TransferCommandHandler : IRequestHandler<TransferCommand, Result>
    {
        private readonly ApplicationDbContext _context;
 private readonly GoldenCrown.Messaging.IRabbitMqPublisher _publisher;

        public TransferCommandHandler(ApplicationDbContext context, GoldenCrown.Messaging.IRabbitMqPublisher publisher){_context=context;_publisher=publisher;}

        public async Task<Result> Handle(TransferCommand request, CancellationToken cancellationToken)
        {
            await using var trx = await _context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                var fromAccount = await _context.Accounts
                    .FirstOrDefaultAsync(x =>
                        x.UserId == request.FromUserId &&
                        x.Currency == request.Currency);

                if (fromAccount == null)
                    return Result.Failure(
                    "Счет отправителя не найден");

                var receiver = await _context.Users.FirstOrDefaultAsync(
                x => x.Login == request.ToLogin);

                if (receiver == null)
                    return Result.Failure("Получатель не найден");

                if (receiver.Id == request.FromUserId)
                {
                    return Result.Failure("Нельзя переводить самому себе");
                }

                var toAccount = await _context.Accounts.FirstOrDefaultAsync(x =>
                x.UserId == receiver.Id &&
                x.Currency == request.Currency);

                if (toAccount == null)
                    return Result.Failure("Получатель не имеет счет данной валюты");

                if (fromAccount.Balance < request.Amount)
                    return Result.Failure(
                    "Недостаточно средств");

                fromAccount.Balance -= request.Amount;

                toAccount.Balance += request.Amount;

                _context.Transactions.Add(
                new Transaction
                {
                    SenderAccountId = fromAccount.Id,
                    ReceiverAccountId = toAccount.Id,
                    Currency = request.Currency,
                    Type = TransactionType.Transfer,
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

                await trx.CommitAsync(cancellationToken);

                await _publisher.PublishAsync(
                    new GoldenCrown.Contracts.TransactionCreatedEvent
                    {
                        SenderId = request.FromUserId,
                        ReceiverId = receiver.Id,
                        Amount = request.Amount,
                        Currency = request.Currency.ToString()
                    });

                return Result.Success();
            }
            catch
            {
                await trx.RollbackAsync();
                throw;
            }

        }

    }
}
