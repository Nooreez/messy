using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Messenger.Migrations
{
    /// <inheritdoc />
    public partial class migration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_fromid",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_toid",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_fromid",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_toid",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "fromid",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "toid",
                table: "Messages");

            migrationBuilder.AddColumn<string>(
                name: "from",
                table: "Messages",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "to",
                table: "Messages",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "from",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "to",
                table: "Messages");

            migrationBuilder.AddColumn<int>(
                name: "fromid",
                table: "Messages",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "toid",
                table: "Messages",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_fromid",
                table: "Messages",
                column: "fromid");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_toid",
                table: "Messages",
                column: "toid");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_fromid",
                table: "Messages",
                column: "fromid",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_toid",
                table: "Messages",
                column: "toid",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
