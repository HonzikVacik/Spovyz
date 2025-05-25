using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spovyz.Migrations
{
    /// <inheritdoc />
    public partial class DvanactaMigrace : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Project_tags_ProjectId",
                table: "Project_tags");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Project_tags",
                table: "Project_tags",
                columns: new[] { "ProjectId", "TagId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Project_tags",
                table: "Project_tags");

            migrationBuilder.CreateIndex(
                name: "IX_Project_tags_ProjectId",
                table: "Project_tags",
                column: "ProjectId");
        }
    }
}
