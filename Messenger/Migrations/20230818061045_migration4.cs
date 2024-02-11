using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Messenger.Migrations
{
    /// <inheritdoc />
    public partial class migration4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friends_Users_Usersid",
                table: "Friends");

            migrationBuilder.DropIndex(
                name: "IX_Friends_Usersid",
                table: "Friends");

            migrationBuilder.DropColumn(
                name: "Usersid",
                table: "Friends");

            migrationBuilder.RenameColumn(
                name: "friendUsername",
                table: "Friends",
                newName: "username2");

            migrationBuilder.AddColumn<string>(
                name: "username1",
                table: "Friends",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "username1",
                table: "Friends");

            migrationBuilder.RenameColumn(
                name: "username2",
                table: "Friends",
                newName: "friendUsername");

            migrationBuilder.AddColumn<int>(
                name: "Usersid",
                table: "Friends",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Friends_Usersid",
                table: "Friends",
                column: "Usersid");

            migrationBuilder.AddForeignKey(
                name: "FK_Friends_Users_Usersid",
                table: "Friends",
                column: "Usersid",
                principalTable: "Users",
                principalColumn: "id");
        }
    }
}
