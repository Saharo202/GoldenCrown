using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GoldenCrown.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "accounts",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "accounts",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "exchange_rates",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "exchange_rates",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "exchange_rates",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "exchange_rates",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "exchange_rates",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "exchange_rates",
                keyColumn: "id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: 2);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "exchange_rates",
                columns: new[] { "id", "from_currency", "rate", "to_currency", "updated_at" },
                values: new object[,]
                {
                    { 1, 1, 0.0125m, 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 2, 80m, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 1, 0.011m, 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, 3, 90m, 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, 2, 0.91m, 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, 3, 1.10m, 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "login", "name", "password" },
                values: new object[,]
                {
                    { 1, "admin", "Administrator", "admin" },
                    { 2, "user", "Regular User", "user" }
                });

            migrationBuilder.InsertData(
                table: "accounts",
                columns: new[] { "id", "balance", "currency", "user_id" },
                values: new object[,]
                {
                    { 1, 0m, 1, 1 },
                    { 2, 0m, 1, 2 }
                });
        }
    }
}
