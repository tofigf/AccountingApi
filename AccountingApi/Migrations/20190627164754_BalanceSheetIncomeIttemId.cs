using Microsoft.EntityFrameworkCore.Migrations;

namespace AccountingApi.Migrations
{
    public partial class BalanceSheetIncomeIttemId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "InvoiceNumber",
                table: "IncomeItems",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IncomeItemId",
                table: "BalanceSheets",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BalanceSheets_IncomeItemId",
                table: "BalanceSheets",
                column: "IncomeItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_BalanceSheets_IncomeItems_IncomeItemId",
                table: "BalanceSheets",
                column: "IncomeItemId",
                principalTable: "IncomeItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BalanceSheets_IncomeItems_IncomeItemId",
                table: "BalanceSheets");

            migrationBuilder.DropIndex(
                name: "IX_BalanceSheets_IncomeItemId",
                table: "BalanceSheets");

            migrationBuilder.DropColumn(
                name: "IncomeItemId",
                table: "BalanceSheets");

            migrationBuilder.AlterColumn<string>(
                name: "InvoiceNumber",
                table: "IncomeItems",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 300,
                oldNullable: true);
        }
    }
}
