using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AccountingApi.Migrations
{
    public partial class JournalDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "ManualJournals",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "ManualJournals");
        }
    }
}
