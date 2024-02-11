using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Messenger.Migrations
{
    /// <inheritdoc />
    public partial class migration5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "lastMessage",
                table: "Friends",
                newName: "lastMessageDate");

            migrationBuilder.AddColumn<string>(
                name: "lastMessageText",
                table: "Friends",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "lastMessageText",
                table: "Friends");

            migrationBuilder.RenameColumn(
                name: "lastMessageDate",
                table: "Friends",
                newName: "lastMessage");
        }
    }
}
