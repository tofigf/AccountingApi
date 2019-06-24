using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AccountingApi.Migrations
{
    public partial class ContragentsAndAccounsPlan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountsPlans",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccPlanNumber = table.Column<string>(maxLength: 100, nullable: true),
                    Name = table.Column<string>(maxLength: 350, nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Level = table.Column<int>(nullable: true),
                    Obeysto = table.Column<string>(nullable: true),
                    ContraAccount = table.Column<bool>(nullable: true),
                    Debit = table.Column<double>(nullable: true),
                    Kredit = table.Column<double>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountsPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountsPlans_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Contragents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PhotoFile = table.Column<string>(nullable: true),
                    PhotoUrl = table.Column<string>(maxLength: 150, nullable: true),
                    CompanyName = table.Column<string>(maxLength: 100, nullable: true),
                    Fullname = table.Column<string>(maxLength: 100, nullable: true),
                    Position = table.Column<string>(maxLength: 75, nullable: true),
                    FieldOfActivity = table.Column<string>(maxLength: 75, nullable: true),
                    Phone = table.Column<string>(maxLength: 75, nullable: true),
                    Email = table.Column<string>(maxLength: 75, nullable: true),
                    VOEN = table.Column<string>(maxLength: 75, nullable: true),
                    CreetedAt = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsCostumer = table.Column<bool>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contragents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contragents_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Contragent_Details",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    City = table.Column<string>(maxLength: 75, nullable: true),
                    Country = table.Column<string>(maxLength: 75, nullable: true),
                    Adress = table.Column<string>(maxLength: 75, nullable: true),
                    WebSite = table.Column<string>(maxLength: 75, nullable: true),
                    Linkedin = table.Column<string>(maxLength: 75, nullable: true),
                    Instagram = table.Column<string>(maxLength: 75, nullable: true),
                    Facebook = table.Column<string>(maxLength: 75, nullable: true),
                    Behance = table.Column<string>(maxLength: 75, nullable: true),
                    ContragentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contragent_Details", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contragent_Details_Contragents_ContragentId",
                        column: x => x.ContragentId,
                        principalTable: "Contragents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountsPlans_CompanyId",
                table: "AccountsPlans",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Contragent_Details_ContragentId",
                table: "Contragent_Details",
                column: "ContragentId");

            migrationBuilder.CreateIndex(
                name: "IX_Contragents_CompanyId",
                table: "Contragents",
                column: "CompanyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountsPlans");

            migrationBuilder.DropTable(
                name: "Contragent_Details");

            migrationBuilder.DropTable(
                name: "Contragents");
        }
    }
}
