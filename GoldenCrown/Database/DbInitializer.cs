using GoldenCrown.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GoldenCrown.Database
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            if (!context.Users.Any(u => u.Login == "admin" || u.Login == "user"))
            {
                var users = new List<User>
                {
                    new User { Login = "admin", Name = "Administrator", Password = "admin" },
                    new User { Login = "user", Name = "Regular User", Password = "user" }
                };
                context.Users.AddRange(users);
                context.SaveChanges();

                var adminId = users[0].Id;
                var userId = users[1].Id;

                if (!context.Accounts.Any(a => a.UserId == adminId || a.UserId == userId))
                {
                    context.Accounts.AddRange(
                        new Account { UserId = adminId, Currency = Currency.RUB, Balance = 0 },
                        new Account { UserId = userId, Currency = Currency.RUB, Balance = 0 }
                    );
                    context.SaveChanges();
                }
            }

            if (!context.ExchangeRates.Any())
            {
                context.ExchangeRates.AddRange(
                    new ExchangeRate { FromCurrency = Currency.RUB, ToCurrency = Currency.USD, Rate = 0.0125m, UpdatedAt = new DateTime(2026, 1, 1) },
                    new ExchangeRate { FromCurrency = Currency.USD, ToCurrency = Currency.RUB, Rate = 80m, UpdatedAt = new DateTime(2026, 1, 1) },
                    new ExchangeRate { FromCurrency = Currency.RUB, ToCurrency = Currency.EUR, Rate = 0.011m, UpdatedAt = new DateTime(2025, 1, 1) },
                    new ExchangeRate { FromCurrency = Currency.EUR, ToCurrency = Currency.RUB, Rate = 90m, UpdatedAt = new DateTime(2025, 1, 1) },
                    new ExchangeRate { FromCurrency = Currency.USD, ToCurrency = Currency.EUR, Rate = 0.91m, UpdatedAt = new DateTime(2025, 1, 1) },
                    new ExchangeRate { FromCurrency = Currency.EUR, ToCurrency = Currency.USD, Rate = 1.10m, UpdatedAt = new DateTime(2025, 1, 1) }
                );
                context.SaveChanges();
            }
        }
    }
}