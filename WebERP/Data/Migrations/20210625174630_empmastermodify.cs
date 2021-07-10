using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebERP.Data.Migrations
{
    public partial class empmastermodify : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "emp_salary",
                table: "Employee_Masters",
                nullable: false,
                oldClrType: typeof(int));
}

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.AlterColumn<int>(
                name: "emp_salary",
                table: "Employee_Masters",
                nullable: false,
                oldClrType: typeof(decimal));
        }
    }
}
