using Microsoft.EntityFrameworkCore.Migrations;

namespace WebERP.Data.Migrations
{
    public partial class EmpAdvEmpAttand : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EMP_DEP",
                table: "Employee_Attandance",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EMP_FATHER",
                table: "Employee_Attandance",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EMP_MOB_NO",
                table: "Employee_Attandance",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EMP_DEP",
                table: "Employee_Advance",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EMP_FATHER",
                table: "Employee_Advance",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EMP_MOB_NO",
                table: "Employee_Advance",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EMP_DEP",
                table: "Employee_Attandance");

            migrationBuilder.DropColumn(
                name: "EMP_FATHER",
                table: "Employee_Attandance");

            migrationBuilder.DropColumn(
                name: "EMP_MOB_NO",
                table: "Employee_Attandance");

            migrationBuilder.DropColumn(
                name: "EMP_DEP",
                table: "Employee_Advance");

            migrationBuilder.DropColumn(
                name: "EMP_FATHER",
                table: "Employee_Advance");

            migrationBuilder.DropColumn(
                name: "EMP_MOB_NO",
                table: "Employee_Advance");
        }
    }
}
