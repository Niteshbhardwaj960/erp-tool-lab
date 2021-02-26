using Microsoft.EntityFrameworkCore.Migrations;

namespace WebERP.Data.Migrations
{
    public partial class newcolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           migrationBuilder.AlterColumn<string>(
                name: "CITY_CODE",
                table: "Account_Masters",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "Country_Code",
                table: "Account_Masters",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "State_Code",
                table: "Account_Masters",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country_Code",
                table: "Account_Masters");

            migrationBuilder.DropColumn(
                name: "State_Code",
                table: "Account_Masters");

            migrationBuilder.AlterColumn<string>(
                name: "CITY_CODE",
                table: "Account_Masters",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
