using Microsoft.EntityFrameworkCore.Migrations;

namespace WebERP.Data.Migrations
{
    public partial class newcolumncompany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Country_Code",
                table: "Companies",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State_Code",
                table: "Companies",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country_Code",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "State_Code",
                table: "Companies");
        }
    }
}
