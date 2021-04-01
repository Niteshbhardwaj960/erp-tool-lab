using Microsoft.EntityFrameworkCore.Migrations;

namespace WebERP.Data.Migrations
{
    public partial class modifyempAdvEmpAtten : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OT_HRS",
                table: "Employee_Advance");

            migrationBuilder.RenameColumn(
                name: "ADV_AMOUNT",
                table: "Employee_Attandance",
                newName: "PAY_DAYS");

            migrationBuilder.RenameColumn(
                name: "PAY_DAYS",
                table: "Employee_Advance",
                newName: "ADV_AMOUNT");

            migrationBuilder.AddColumn<decimal>(
                name: "OT_HRS",
                table: "Employee_Attandance",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OT_HRS",
                table: "Employee_Attandance");

            migrationBuilder.RenameColumn(
                name: "PAY_DAYS",
                table: "Employee_Attandance",
                newName: "ADV_AMOUNT");

            migrationBuilder.RenameColumn(
                name: "ADV_AMOUNT",
                table: "Employee_Advance",
                newName: "PAY_DAYS");

            migrationBuilder.AddColumn<decimal>(
                name: "OT_HRS",
                table: "Employee_Advance",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
