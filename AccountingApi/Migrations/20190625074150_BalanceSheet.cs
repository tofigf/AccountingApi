using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AccountingApi.Migrations
{
    public partial class BalanceSheet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BalanceSheets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DebitMoney = table.Column<double>(nullable: true),
                    KreditMoney = table.Column<double>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false),
                    AccountsPlanId = table.Column<int>(nullable: true),
                    InvoiceId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BalanceSheets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BalanceSheets_AccountsPlans_AccountsPlanId",
                        column: x => x.AccountsPlanId,
                        principalTable: "AccountsPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_BalanceSheets_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_BalanceSheets_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BalanceSheets_AccountsPlanId",
                table: "BalanceSheets",
                column: "AccountsPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_BalanceSheets_CompanyId",
                table: "BalanceSheets",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_BalanceSheets_InvoiceId",
                table: "BalanceSheets",
                column: "InvoiceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BalanceSheets");
        }
    }
}
