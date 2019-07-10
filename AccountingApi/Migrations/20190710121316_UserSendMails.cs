using Microsoft.EntityFrameworkCore.Migrations;

namespace AccountingApi.Migrations
{
    public partial class UserSendMails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSendMailChangePassword_Users_UserId",
                table: "UserSendMailChangePassword");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSendMailChangePassword",
                table: "UserSendMailChangePassword");

            migrationBuilder.RenameTable(
                name: "UserSendMailChangePassword",
                newName: "UserSendMailChangePasswords");

            migrationBuilder.RenameIndex(
                name: "IX_UserSendMailChangePassword_UserId",
                table: "UserSendMailChangePasswords",
                newName: "IX_UserSendMailChangePasswords_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSendMailChangePasswords",
                table: "UserSendMailChangePasswords",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSendMailChangePasswords_Users_UserId",
                table: "UserSendMailChangePasswords",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSendMailChangePasswords_Users_UserId",
                table: "UserSendMailChangePasswords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSendMailChangePasswords",
                table: "UserSendMailChangePasswords");

            migrationBuilder.RenameTable(
                name: "UserSendMailChangePasswords",
                newName: "UserSendMailChangePassword");

            migrationBuilder.RenameIndex(
                name: "IX_UserSendMailChangePasswords_UserId",
                table: "UserSendMailChangePassword",
                newName: "IX_UserSendMailChangePassword_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSendMailChangePassword",
                table: "UserSendMailChangePassword",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSendMailChangePassword_Users_UserId",
                table: "UserSendMailChangePassword",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
