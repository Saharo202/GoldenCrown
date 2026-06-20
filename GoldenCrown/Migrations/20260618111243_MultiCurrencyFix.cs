using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GoldenCrown.Migrations
{
    /// <inheritdoc />
    public partial class MultiCurrencyFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_sessions_users_userd_id",
                table: "sessions");

            migrationBuilder.DropIndex(
                name: "IX_accounts_user_id",
                table: "accounts");

            migrationBuilder.RenameColumn(
                name: "userd_id",
                table: "sessions",
                newName: "user_id");

            migrationBuilder.AlterColumn<decimal>(
                name: "amount",
                table: "transaction",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<int>(
                name: "currency",
                table: "transaction",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "type",
                table: "transaction",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "balance",
                table: "accounts",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "accounts",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<int>(
                name: "currency",
                table: "accounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "exchange_rates",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    from_currency = table.Column<int>(type: "int", nullable: false),
                    to_currency = table.Column<int>(type: "int", nullable: false),
                    rate = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exchange_rates", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "accounts",
                columns: new[] { "id", "balance", "currency", "user_id" },
                values: new object[,]
                {
                    { 1, 0m, 1, 1 },
                    { 2, 0m, 1, 2 }
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_accounts_user_id_currency",
                table: "accounts",
                columns: new[] { "user_id", "currency" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_sessions_users_user_id",
                table: "sessions",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_sessions_users_user_id",
                table: "sessions");

            migrationBuilder.DropTable(
                name: "exchange_rates");

            migrationBuilder.DropIndex(
                name: "IX_accounts_user_id_currency",
                table: "accounts");

            migrationBuilder.DeleteData(
                table: "accounts",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "accounts",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "currency",
                table: "transaction");

            migrationBuilder.DropColumn(
                name: "type",
                table: "transaction");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "accounts");

            migrationBuilder.DropColumn(
                name: "currency",
                table: "accounts");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "sessions",
                newName: "userd_id");

            migrationBuilder.AlterColumn<decimal>(
                name: "amount",
                table: "transaction",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldPrecision: 18,
                oldScale: 4);

            migrationBuilder.AlterColumn<decimal>(
                name: "balance",
                table: "accounts",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldPrecision: 18,
                oldScale: 4);

            migrationBuilder.CreateIndex(
                name: "IX_accounts_user_id",
                table: "accounts",
                column: "user_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_sessions_users_userd_id",
                table: "sessions",
                column: "userd_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
