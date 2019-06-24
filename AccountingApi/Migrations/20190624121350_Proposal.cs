using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AccountingApi.Migrations
{
    public partial class Proposal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Proposals",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProposalNumber = table.Column<string>(maxLength: 500, nullable: true),
                    PreparingDate = table.Column<DateTime>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    TotalPrice = table.Column<double>(nullable: true),
                    TotalTax = table.Column<double>(nullable: true),
                    Sum = table.Column<double>(nullable: true),
                    Desc = table.Column<string>(maxLength: 300, nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ContragentId = table.Column<int>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    TaxId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proposals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Proposals_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Proposals_Contragents_ContragentId",
                        column: x => x.ContragentId,
                        principalTable: "Contragents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Proposals_Taxes_TaxId",
                        column: x => x.TaxId,
                        principalTable: "Taxes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ProposalItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Qty = table.Column<int>(nullable: true),
                    Price = table.Column<double>(nullable: true),
                    SellPrice = table.Column<double>(nullable: true),
                    TotalOneProduct = table.Column<double>(nullable: true),
                    ProductId = table.Column<int>(nullable: true),
                    ProposalId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProposalItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProposalItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProposalItems_Proposals_ProposalId",
                        column: x => x.ProposalId,
                        principalTable: "Proposals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ProposalSentMails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(maxLength: 50, nullable: true),
                    Token = table.Column<string>(maxLength: 100, nullable: true),
                    ProposalId = table.Column<int>(nullable: false),
                    IsPaid = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProposalSentMails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProposalSentMails_Proposals_ProposalId",
                        column: x => x.ProposalId,
                        principalTable: "Proposals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProposalItems_ProductId",
                table: "ProposalItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProposalItems_ProposalId",
                table: "ProposalItems",
                column: "ProposalId");

            migrationBuilder.CreateIndex(
                name: "IX_Proposals_CompanyId",
                table: "Proposals",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Proposals_ContragentId",
                table: "Proposals",
                column: "ContragentId");

            migrationBuilder.CreateIndex(
                name: "IX_Proposals_TaxId",
                table: "Proposals",
                column: "TaxId");

            migrationBuilder.CreateIndex(
                name: "IX_ProposalSentMails_ProposalId",
                table: "ProposalSentMails",
                column: "ProposalId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProposalItems");

            migrationBuilder.DropTable(
                name: "ProposalSentMails");

            migrationBuilder.DropTable(
                name: "Proposals");
        }
    }
}
