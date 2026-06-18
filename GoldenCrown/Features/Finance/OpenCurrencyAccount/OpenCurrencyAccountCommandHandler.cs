using GoldenCrown.Database;
using GoldenCrown.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GoldenCrown.Features.Finance.OpenCurrencyAccount
{
    public class OpenCurrencyAccountCommandHandler : IRequestHandler<OpenCurrencyAccountCommand, Result>
    {
        private readonly ApplicationDbContext _context;

        public OpenCurrencyAccountCommandHandler(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(
            OpenCurrencyAccountCommand request,
            CancellationToken cancellationToken)
        {
            var accountExists =
                await _context.Accounts.AnyAsync(
                    a =>
                        a.UserId == request.UserId &&
                        a.Currency == request.Currency,
                    cancellationToken);

            if (accountExists)
            {
                return Result.Failure(
                    "Счет в данной валюте уже существует");
            }

            var account = new Account
            {
                UserId = request.UserId,
                Currency = request.Currency,
                Balance = 0m
            };

            _context.Accounts.Add(account);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}