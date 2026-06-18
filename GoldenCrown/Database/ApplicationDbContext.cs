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

        }
    }
}
