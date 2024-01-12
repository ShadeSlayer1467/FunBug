using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SqlServerLib.Migrations
{
    public partial class base3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserTask");

            migrationBuilder.CreateTable(
                name: "TaskItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    DiscordUserTaskListId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskItems_DiscordUserTaskLists_DiscordUserTaskListId",
                        column: x => x.DiscordUserTaskListId,
                        principalTable: "DiscordUserTaskLists",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskItems_DiscordUserTaskListId",
                table: "TaskItems",
                column: "DiscordUserTaskListId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskItems");

            migrationBuilder.CreateTable(
                name: "UserTask",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiscordUserTaskListId = table.Column<int>(type: "int", nullable: true),
                    Task = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTask", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTask_DiscordUserTaskLists_DiscordUserTaskListId",
                        column: x => x.DiscordUserTaskListId,
                        principalTable: "DiscordUserTaskLists",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserTask_DiscordUserTaskListId",
                table: "UserTask",
                column: "DiscordUserTaskListId");
        }
    }
}
