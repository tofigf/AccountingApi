using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AccountingApi.Migrations
{
    public partial class UserAndcompany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 75, nullable: true),
                    SurName = table.Column<string>(maxLength: 100, nullable: true),
                    Email = table.Column<string>(maxLength: 50, nullable: true),
                    Token = table.Column<string>(maxLength: 150, nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    Password = table.Column<string>(maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PhotoFile = table.Column<string>(nullable: true),
                    PhotoUrl = table.Column<string>(maxLength: 150, nullable: true),
                    CompanyName = table.Column<string>(maxLength: 75, nullable: true),
                    Name = table.Column<string>(maxLength: 75, nullable: true),
                    Surname = table.Column<string>(maxLength: 75, nullable: true),
                    Postion = table.Column<string>(maxLength: 75, nullable: true),
                    FieldOfActivity = table.Column<string>(maxLength: 75, nullable: true),
                    VOEN = table.Column<string>(maxLength: 100, nullable: true),
                    Country = table.Column<string>(maxLength: 50, nullable: true),
                    Street = table.Column<string>(maxLength: 75, nullable: true),
                    Phone = table.Column<string>(maxLength: 20, nullable: true),
                    Mobile = table.Column<string>(maxLength: 20, nullable: true),
                    Website = table.Column<string>(maxLength: 30, nullable: true),
                    Linkedin = table.Column<string>(maxLength: 30, nullable: true),
                    Facebok = table.Column<string>(maxLength: 30, nullable: true),
                    Instagram = table.Column<string>(maxLength: 30, nullable: true),
                    Behance = table.Column<string>(maxLength: 30, nullable: true),
                    City = table.Column<string>(maxLength: 50, nullable: true),
                    Email = table.Column<string>(maxLength: 100, nullable: true),
                    IsCompany = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Companies_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Companies_UserId",
                table: "Companies",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
