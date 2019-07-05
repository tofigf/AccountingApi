using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AccountingApi.Migrations
{
    public partial class Journal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ManualJournalId",
                table: "BalanceSheets",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OperationCategories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 250, nullable: true),
                    ShortName = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ManualJournals",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    JurnalNumber = table.Column<string>(maxLength: 250, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Desc = table.Column<string>(maxLength: 300, nullable: true),
                    Price = table.Column<double>(nullable: true),
                    ContragentId = table.Column<int>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    AccountDebitId = table.Column<int>(nullable: true),
                    AccountKreditId = table.Column<int>(nullable: true),
                    OperationCategoryId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManualJournals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ManualJournals_AccountsPlans_AccountDebitId",
                        column: x => x.AccountDebitId,
                        principalTable: "AccountsPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ManualJournals_AccountsPlans_AccountKreditId",
                        column: x => x.AccountKreditId,
                        principalTable: "AccountsPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ManualJournals_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ManualJournals_Contragents_ContragentId",
                        column: x => x.ContragentId,
                        principalTable: "Contragents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ManualJournals_OperationCategories_OperationCategoryId",
                        column: x => x.OperationCategoryId,
                        principalTable: "OperationCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BalanceSheets_ManualJournalId",
                table: "BalanceSheets",
                column: "ManualJournalId");

            migrationBuilder.CreateIndex(
                name: "IX_ManualJournals_AccountDebitId",
                table: "ManualJournals",
                column: "AccountDebitId");

            migrationBuilder.CreateIndex(
                name: "IX_ManualJournals_AccountKreditId",
                table: "ManualJournals",
                column: "AccountKreditId");

            migrationBuilder.CreateIndex(
                name: "IX_ManualJournals_CompanyId",
                table: "ManualJournals",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ManualJournals_ContragentId",
                table: "ManualJournals",
                column: "ContragentId");

            migrationBuilder.CreateIndex(
                name: "IX_ManualJournals_OperationCategoryId",
                table: "ManualJournals",
                column: "OperationCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_BalanceSheets_ManualJournals_ManualJournalId",
                table: "BalanceSheets",
                column: "ManualJournalId",
                principalTable: "ManualJournals",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BalanceSheets_ManualJournals_ManualJournalId",
                table: "BalanceSheets");

            migrationBuilder.DropTable(
                name: "ManualJournals");

            migrationBuilder.DropTable(
                name: "OperationCategories");

            migrationBuilder.DropIndex(
                name: "IX_BalanceSheets_ManualJournalId",
                table: "BalanceSheets");

            migrationBuilder.DropColumn(
                name: "ManualJournalId",
                table: "BalanceSheets");
        }
    }
}
