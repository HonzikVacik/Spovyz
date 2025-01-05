using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spovyz.Migrations
{
    /// <inheritdoc />
    public partial class TretiMigrace : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Salt",
                table: "Employees");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Salt",
                table: "Employees",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
