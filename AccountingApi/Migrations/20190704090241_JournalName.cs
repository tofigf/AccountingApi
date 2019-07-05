using Microsoft.EntityFrameworkCore.Migrations;

namespace AccountingApi.Migrations
{
    public partial class JournalName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "JurnalNumber",
                table: "ManualJournals",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JurnalName",
                table: "ManualJournals",
                maxLength: 300,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JurnalName",
                table: "ManualJournals");

            migrationBuilder.AlterColumn<string>(
                name: "JurnalNumber",
                table: "ManualJournals",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 300,
                oldNullable: true);
        }
    }
}
