using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AccountingApi.Migrations
{
    public partial class Worker : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Workers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 75, nullable: true),
                    SurName = table.Column<string>(maxLength: 75, nullable: true),
                    Positon = table.Column<string>(maxLength: 75, nullable: true),
                    Salary = table.Column<double>(nullable: true),
                    Departament = table.Column<string>(maxLength: 75, nullable: true),
                    PartofDepartament = table.Column<string>(maxLength: 75, nullable: true),
                    Role = table.Column<string>(maxLength: 75, nullable: true),
                    RegisterDate = table.Column<DateTime>(nullable: false),
                    PhotoFile = table.Column<string>(nullable: true),
                    PhotoUrl = table.Column<string>(nullable: true),
                    IsState = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Workers_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Worker_Details",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FatherName = table.Column<string>(maxLength: 75, nullable: true),
                    Email = table.Column<string>(maxLength: 75, nullable: true),
                    Adress = table.Column<string>(maxLength: 75, nullable: true),
                    DSMF = table.Column<string>(maxLength: 75, nullable: true),
                    Voen = table.Column<string>(maxLength: 75, nullable: true),
                    Phone = table.Column<string>(maxLength: 75, nullable: true),
                    MobilePhone = table.Column<string>(maxLength: 75, nullable: true),
                    Education = table.Column<string>(maxLength: 75, nullable: true),
                    EducationLevel = table.Column<string>(maxLength: 75, nullable: true),
                    Gender = table.Column<string>(maxLength: 20, nullable: true),
                    Birthday = table.Column<DateTime>(nullable: true),
                    WorkerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Worker_Details", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Worker_Details_Workers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Worker_Details_WorkerId",
                table: "Worker_Details",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_Workers_CompanyId",
                table: "Workers",
                column: "CompanyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Worker_Details");

            migrationBuilder.DropTable(
                name: "Workers");
        }
    }
}
