using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AccountingApi.Migrations
{
    public partial class Expense : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExpenseInvoiceId",
                table: "BalanceSheets",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExpenseItemId",
                table: "BalanceSheets",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ExpenseInvoices",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ExpenseInvoiceNumber = table.Column<string>(maxLength: 250, nullable: true),
                    PreparingDate = table.Column<DateTime>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    TotalPrice = table.Column<double>(nullable: true),
                    ResidueForCalc = table.Column<double>(nullable: true),
                    TotalTax = table.Column<double>(nullable: true),
                    Sum = table.Column<double>(nullable: true),
                    IsPaid = table.Column<byte>(nullable: false),
                    Desc = table.Column<string>(maxLength: 300, nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ContragentId = table.Column<int>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    TaxId = table.Column<int>(nullable: true),
                    AccountDebitId = table.Column<int>(nullable: true),
                    AccountKreditId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseInvoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpenseInvoices_AccountsPlans_AccountDebitId",
                        column: x => x.AccountDebitId,
                        principalTable: "AccountsPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ExpenseInvoices_AccountsPlans_AccountKreditId",
                        column: x => x.AccountKreditId,
                        principalTable: "AccountsPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ExpenseInvoices_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ExpenseInvoices_Contragents_ContragentId",
                        column: x => x.ContragentId,
                        principalTable: "Contragents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ExpenseInvoices_Taxes_TaxId",
                        column: x => x.TaxId,
                        principalTable: "Taxes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Expenses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TotalPrice = table.Column<double>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false),
                    ContragentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Expenses_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Expenses_Contragents_ContragentId",
                        column: x => x.ContragentId,
                        principalTable: "Contragents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ExpenseInvoiceItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Qty = table.Column<int>(nullable: true),
                    Price = table.Column<double>(nullable: true),
                    TotalOneProduct = table.Column<double>(nullable: true),
                    ProductId = table.Column<int>(nullable: true),
                    ExpenseInvoiceId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseInvoiceItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpenseInvoiceItems_ExpenseInvoices_ExpenseInvoiceId",
                        column: x => x.ExpenseInvoiceId,
                        principalTable: "ExpenseInvoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ExpenseInvoiceItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ExpenseItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Residue = table.Column<double>(nullable: true),
                    TotalOneInvoice = table.Column<double>(nullable: true),
                    PaidMoney = table.Column<double>(nullable: true),
                    IsBank = table.Column<bool>(nullable: false),
                    InvoiceNumber = table.Column<string>(nullable: true),
                    ExpenseInvoiceId = table.Column<int>(nullable: false),
                    ExpenseId = table.Column<int>(nullable: false),
                    AccountDebitId = table.Column<int>(nullable: true),
                    AccountKreditId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpenseItems_AccountsPlans_AccountDebitId",
                        column: x => x.AccountDebitId,
                        principalTable: "AccountsPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ExpenseItems_AccountsPlans_AccountKreditId",
                        column: x => x.AccountKreditId,
                        principalTable: "AccountsPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ExpenseItems_Expenses_ExpenseId",
                        column: x => x.ExpenseId,
                        principalTable: "Expenses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ExpenseItems_ExpenseInvoices_ExpenseInvoiceId",
                        column: x => x.ExpenseInvoiceId,
                        principalTable: "ExpenseInvoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BalanceSheets_ExpenseInvoiceId",
                table: "BalanceSheets",
                column: "ExpenseInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_BalanceSheets_ExpenseItemId",
                table: "BalanceSheets",
                column: "ExpenseItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseInvoiceItems_ExpenseInvoiceId",
                table: "ExpenseInvoiceItems",
                column: "ExpenseInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseInvoiceItems_ProductId",
                table: "ExpenseInvoiceItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseInvoices_AccountDebitId",
                table: "ExpenseInvoices",
                column: "AccountDebitId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseInvoices_AccountKreditId",
                table: "ExpenseInvoices",
                column: "AccountKreditId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseInvoices_CompanyId",
                table: "ExpenseInvoices",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseInvoices_ContragentId",
                table: "ExpenseInvoices",
                column: "ContragentId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseInvoices_TaxId",
                table: "ExpenseInvoices",
                column: "TaxId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseItems_AccountDebitId",
                table: "ExpenseItems",
                column: "AccountDebitId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseItems_AccountKreditId",
                table: "ExpenseItems",
                column: "AccountKreditId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseItems_ExpenseId",
                table: "ExpenseItems",
                column: "ExpenseId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseItems_ExpenseInvoiceId",
                table: "ExpenseItems",
                column: "ExpenseInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_CompanyId",
                table: "Expenses",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_ContragentId",
                table: "Expenses",
                column: "ContragentId");

            migrationBuilder.AddForeignKey(
                name: "FK_BalanceSheets_ExpenseInvoices_ExpenseInvoiceId",
                table: "BalanceSheets",
                column: "ExpenseInvoiceId",
                principalTable: "ExpenseInvoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_BalanceSheets_ExpenseItems_ExpenseItemId",
                table: "BalanceSheets",
                column: "ExpenseItemId",
                principalTable: "ExpenseItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BalanceSheets_ExpenseInvoices_ExpenseInvoiceId",
                table: "BalanceSheets");

            migrationBuilder.DropForeignKey(
                name: "FK_BalanceSheets_ExpenseItems_ExpenseItemId",
                table: "BalanceSheets");

            migrationBuilder.DropTable(
                name: "ExpenseInvoiceItems");

            migrationBuilder.DropTable(
                name: "ExpenseItems");

            migrationBuilder.DropTable(
                name: "Expenses");

            migrationBuilder.DropTable(
                name: "ExpenseInvoices");

            migrationBuilder.DropIndex(
                name: "IX_BalanceSheets_ExpenseInvoiceId",
                table: "BalanceSheets");

            migrationBuilder.DropIndex(
                name: "IX_BalanceSheets_ExpenseItemId",
                table: "BalanceSheets");

            migrationBuilder.DropColumn(
                name: "ExpenseInvoiceId",
                table: "BalanceSheets");

            migrationBuilder.DropColumn(
                name: "ExpenseItemId",
                table: "BalanceSheets");
        }
    }
}
