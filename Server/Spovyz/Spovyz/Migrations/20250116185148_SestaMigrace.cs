using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spovyz.Migrations
{
    /// <inheritdoc />
    public partial class SestaMigrace : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "NeedResetPassword",
                table: "Employees",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NeedResetPassword",
                table: "Employees");
        }
    }
}
