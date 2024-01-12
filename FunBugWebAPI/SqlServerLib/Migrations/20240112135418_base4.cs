using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SqlServerLib.Migrations
{
    public partial class base4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskItems_DiscordUserTaskLists_DiscordUserTaskListId",
                table: "TaskItems");

            migrationBuilder.RenameColumn(
                name: "DiscordUserTaskListId",
                table: "TaskItems",
                newName: "DiscordUserTaskListUserId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskItems_DiscordUserTaskListId",
                table: "TaskItems",
                newName: "IX_TaskItems_DiscordUserTaskListUserId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "DiscordUserTaskLists",
                newName: "UserId");

            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "TaskItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskItems_DiscordUserTaskLists_DiscordUserTaskListUserId",
                table: "TaskItems",
                column: "DiscordUserTaskListUserId",
                principalTable: "DiscordUserTaskLists",
                principalColumn: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskItems_DiscordUserTaskLists_DiscordUserTaskListUserId",
                table: "TaskItems");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "TaskItems");

            migrationBuilder.RenameColumn(
                name: "DiscordUserTaskListUserId",
                table: "TaskItems",
                newName: "DiscordUserTaskListId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskItems_DiscordUserTaskListUserId",
                table: "TaskItems",
                newName: "IX_TaskItems_DiscordUserTaskListId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "DiscordUserTaskLists",
                newName: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskItems_DiscordUserTaskLists_DiscordUserTaskListId",
                table: "TaskItems",
                column: "DiscordUserTaskListId",
                principalTable: "DiscordUserTaskLists",
                principalColumn: "Id");
        }
    }
}
