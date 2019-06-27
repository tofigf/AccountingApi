using Microsoft.EntityFrameworkCore.Migrations;

namespace AccountingApi.Migrations
{
    public partial class IncomeAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccountDebitId",
                table: "IncomeItems",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AccountKreditId",
                table: "IncomeItems",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IncomeItems_AccountDebitId",
                table: "IncomeItems",
                column: "AccountDebitId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeItems_AccountKreditId",
                table: "IncomeItems",
                column: "AccountKreditId");

            migrationBuilder.AddForeignKey(
                name: "FK_IncomeItems_AccountsPlans_AccountDebitId",
                table: "IncomeItems",
                column: "AccountDebitId",
                principalTable: "AccountsPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_IncomeItems_AccountsPlans_AccountKreditId",
                table: "IncomeItems",
                column: "AccountKreditId",
                principalTable: "AccountsPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IncomeItems_AccountsPlans_AccountDebitId",
                table: "IncomeItems");

            migrationBuilder.DropForeignKey(
                name: "FK_IncomeItems_AccountsPlans_AccountKreditId",
                table: "IncomeItems");

            migrationBuilder.DropIndex(
                name: "IX_IncomeItems_AccountDebitId",
                table: "IncomeItems");

            migrationBuilder.DropIndex(
                name: "IX_IncomeItems_AccountKreditId",
                table: "IncomeItems");

            migrationBuilder.DropColumn(
                name: "AccountDebitId",
                table: "IncomeItems");

            migrationBuilder.DropColumn(
                name: "AccountKreditId",
                table: "IncomeItems");
        }
    }
}
