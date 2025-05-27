using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spovyz.Migrations
{
    /// <inheritdoc />
    public partial class CtrnacaMigrace : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Task_tags_TaskId",
                table: "Task_tags");

            migrationBuilder.DropIndex(
                name: "IX_Task_employees_TaskId",
                table: "Task_employees");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Task_tags",
                table: "Task_tags",
                columns: new[] { "TaskId", "TagId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Task_employees",
                table: "Task_employees",
                columns: new[] { "TaskId", "EmployeeId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Task_tags",
                table: "Task_tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Task_employees",
                table: "Task_employees");

            migrationBuilder.CreateIndex(
                name: "IX_Task_tags_TaskId",
                table: "Task_tags",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Task_employees_TaskId",
                table: "Task_employees",
                column: "TaskId");
        }
    }
}
