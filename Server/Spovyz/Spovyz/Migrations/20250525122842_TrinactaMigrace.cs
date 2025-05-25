using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spovyz.Migrations
{
    /// <inheritdoc />
    public partial class TrinactaMigrace : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Project_employees_ProjectId",
                table: "Project_employees");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Project_employees",
                table: "Project_employees",
                columns: new[] { "ProjectId", "EmployeeId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Project_employees",
                table: "Project_employees");

            migrationBuilder.CreateIndex(
                name: "IX_Project_employees_ProjectId",
                table: "Project_employees",
                column: "ProjectId");
        }
    }
}
