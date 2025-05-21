using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spovyz.Migrations
{
    /// <inheritdoc />
    public partial class JedenactaMigrace : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Customer_CustomerId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Task_employees_Employees_EmlployeeId",
                table: "Task_employees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Customer",
                table: "Customer");

            migrationBuilder.RenameTable(
                name: "Customer",
                newName: "Customers");

            migrationBuilder.RenameColumn(
                name: "EmlployeeId",
                table: "Task_employees",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Task_employees_EmlployeeId",
                table: "Task_employees",
                newName: "IX_Task_employees_EmployeeId");

            migrationBuilder.RenameColumn(
                name: "Pay",
                table: "Accountings",
                newName: "Salary");

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "Accountings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customers",
                table: "Customers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Customers_CustomerId",
                table: "Projects",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Task_employees_Employees_EmployeeId",
                table: "Task_employees",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Customers_CustomerId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Task_employees_Employees_EmployeeId",
                table: "Task_employees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Customers",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "Accountings");

            migrationBuilder.RenameTable(
                name: "Customers",
                newName: "Customer");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "Task_employees",
                newName: "EmlployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Task_employees_EmployeeId",
                table: "Task_employees",
                newName: "IX_Task_employees_EmlployeeId");

            migrationBuilder.RenameColumn(
                name: "Salary",
                table: "Accountings",
                newName: "Pay");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customer",
                table: "Customer",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Customer_CustomerId",
                table: "Projects",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Task_employees_Employees_EmlployeeId",
                table: "Task_employees",
                column: "EmlployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
