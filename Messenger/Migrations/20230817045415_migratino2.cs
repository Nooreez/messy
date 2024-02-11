using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Messenger.Migrations
{
    /// <inheritdoc />
    public partial class migratino2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friends_Users_Friendid",
                table: "Friends");

            migrationBuilder.DropIndex(
                name: "IX_Friends_Friendid",
                table: "Friends");

            migrationBuilder.DropColumn(
                name: "Friendid",
                table: "Friends");

            migrationBuilder.AddColumn<int>(
                name: "Usersid",
                table: "Friends",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "friendUsername",
                table: "Friends",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "lastMessage",
                table: "Friends",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "FriendsRequest",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    from = table.Column<string>(type: "text", nullable: false),
                    to = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendsRequest", x => x.id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friends_Users_Usersid",
                table: "Friends");

            migrationBuilder.DropTable(
                name: "FriendsRequest");

            migrationBuilder.DropIndex(
                name: "IX_Friends_Usersid",
                table: "Friends");

            migrationBuilder.DropColumn(
                name: "Usersid",
                table: "Friends");

            migrationBuilder.DropColumn(
                name: "friendUsername",
                table: "Friends");

            migrationBuilder.DropColumn(
                name: "lastMessage",
                table: "Friends");

            migrationBuilder.AddColumn<int>(
                name: "Friendid",
                table: "Friends",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Friends_Friendid",
                table: "Friends",
                column: "Friendid");

            migrationBuilder.AddForeignKey(
                name: "FK_Friends_Users_Friendid",
                table: "Friends",
                column: "Friendid",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
