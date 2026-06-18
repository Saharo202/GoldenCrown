using GoldenCrown.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoldenCrown.Database
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<ExchangeRate> ExchangeRates { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var userEntity = modelBuilder.Entity<User>().ToTable("users");
            userEntity.HasKey(x => x.Id);
            userEntity.Property(x => x.Id).HasColumnName("id").UseIdentityColumn();
            userEntity.Property(x => x.Login).HasColumnName("login").IsRequired();
            userEntity.Property(x => x.Name).HasColumnName("name").IsRequired();
            userEntity.Property(x => x.Password).HasColumnName("password").IsRequired();

            SeedUserData(userEntity);

            var accountEntity = modelBuilder.Entity<Account>().ToTable("accounts");
            accountEntity.HasKey(x => x.Id);
            accountEntity.Property(x => x.Id).HasColumnName("id").UseIdentityColumn();
            accountEntity.Property(x => x.UserId).HasColumnName("user_id").IsRequired();
            accountEntity.Property(x => x.Currency).HasColumnName("currency").IsRequired();
            accountEntity.Property(x => x.Balance).HasColumnName("balance").HasPrecision(18, 4).IsRequired();
            accountEntity.Property(x => x.RowVersion).IsRowVersion();
            accountEntity.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
            accountEntity.HasIndex(x => new
            {
                x.UserId,
                x.Currency
            }).IsUnique();

            SeedAccountData(accountEntity);

            var sessionEntity = modelBuilder.Entity<Session>().ToTable("sessions");
            sessionEntity.HasKey(x => x.UserId);
            sessionEntity.Property(x => x.UserId).HasColumnName("user_id").IsRequired();
            sessionEntity.Property(x => x.Token).HasColumnName("token").IsRequired();
            sessionEntity.Property(x => x.ExpiresAt).HasColumnName("expires_at").IsRequired();
            sessionEntity.HasOne<User>().WithOne().HasForeignKey<Session>(x => x.UserId).OnDelete(DeleteBehavior.Cascade);

            var transactionEntity = modelBuilder.Entity<Transaction>().ToTable("transaction");
            transactionEntity.HasKey(x => x.Id);
            transactionEntity.Property(x => x.Id).HasColumnName("id").UseIdentityColumn();
            transactionEntity.Property(x => x.SenderAccountId).HasColumnName("sender_account_id").IsRequired();
            transactionEntity.Property(x => x.ReceiverAccountId).HasColumnName("receiver_account_id").IsRequired();
            transactionEntity.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
            transactionEntity.Property(x => x.Amount).HasColumnName("amount").HasPrecision(18, 4).IsRequired();
            transactionEntity.Property(x => x.Currency).HasColumnName("currency");
            transactionEntity
                .Property(x => x.Type)
                .HasColumnName("type");
            transactionEntity.HasOne<Account>().WithMany().HasForeignKey(x => x.SenderAccountId).OnDelete(DeleteBehavior.NoAction);
            transactionEntity.HasOne<Account>().WithMany().HasForeignKey(x => x.ReceiverAccountId).OnDelete(DeleteBehavior.NoAction);

            var rateEntity = modelBuilder.Entity<ExchangeRate>()
                .ToTable("exchange_rates");
            rateEntity.HasKey(x => x.Id);
            rateEntity.Property(x => x.Id)
                .HasColumnName("id")
                .UseIdentityColumn();
            rateEntity.Property(x => x.FromCurrency)
                .HasColumnName("from_currency")
                .IsRequired();
            rateEntity.Property(x => x.ToCurrency)
                .HasColumnName("to_currency")
                .IsRequired();
            rateEntity.Property(x => x.Rate)
                .HasColumnName("rate")
                .HasPrecision(18, 6)
                .IsRequired();
            rateEntity.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired();

            SeedExchangeRates(rateEntity);



        }

        private void SeedUserData(EntityTypeBuilder<User> userEntity)
        {
            userEntity.HasData
                (
                new User
                {
                    Id = 1,
                    Login = "admin",
                    Name = "Administrator",
                    Password = "admin"
                },
                new User
                {
                    Id = 2,
                    Login = "user",
                    Name = "Regular User",
                    Password = "user"
                }
                );

        }

        private void SeedAccountData(EntityTypeBuilder<Account> accountEntity) 
        {
            accountEntity.HasData(
                new Account
                {
                    Id = 1,
                    UserId = 1,
                    Currency = Currency.RUB,
                    Balance = 0
                },
                new Account
                {
                    Id = 2,
                    UserId = 2,
                    Currency = Currency.RUB,
                    Balance = 0
                });
        }

        private void SeedExchangeRates(EntityTypeBuilder<ExchangeRate> exchangeRateEntity)
        {
            exchangeRateEntity.HasData(
                new ExchangeRate
                {
                    Id = 1,
                    FromCurrency = Currency.RUB,
                    ToCurrency = Currency.USD,
                    Rate = 0.0125m,
                    UpdatedAt = new DateTime(2026, 1, 1)
                },
                new ExchangeRate
                {
                    Id = 2,
                    FromCurrency = Currency.USD,
                    ToCurrency = Currency.RUB,
                    Rate = 80m,
                    UpdatedAt = new DateTime(2026, 1, 1)
                },
                new ExchangeRate
                {
                    Id = 3,
                    FromCurrency = Currency.RUB,
                    ToCurrency = Currency.EUR,
                    Rate = 0.011m,
                    UpdatedAt = new DateTime(2025, 1, 1)
                },
                new ExchangeRate
                {
                    Id = 4,
                    FromCurrency = Currency.EUR,
                    ToCurrency = Currency.RUB,
                    Rate = 90m,
                    UpdatedAt = new DateTime(2025, 1, 1)
                },
                new ExchangeRate
                {
                    Id = 5,
                    FromCurrency = Currency.USD,
                    ToCurrency = Currency.EUR,
                    Rate = 0.91m,
                    UpdatedAt = new DateTime(2025, 1, 1)
                },
                new ExchangeRate
                {
                    Id = 6,
                    FromCurrency = Currency.EUR,
                    ToCurrency = Currency.USD,
                    Rate = 1.10m,
                    UpdatedAt = new DateTime(2025, 1, 1)
                }
            );
        }

    }
}
