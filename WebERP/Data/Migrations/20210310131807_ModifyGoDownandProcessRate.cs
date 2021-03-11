using Microsoft.EntityFrameworkCore.Migrations;

namespace WebERP.Data.Migrations
{
    public partial class ModifyGoDownandProcessRate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Artical_Code",
                table: "ProcessRate_Master",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Comm_Rate",
                table: "ProcessRate_Master",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GO_DOWN_TYPE",
                table: "Godown_Master",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SALE_TAG",
                table: "Godown_Master",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Artical_Code",
                table: "ProcessRate_Master");

            migrationBuilder.DropColumn(
                name: "Comm_Rate",
                table: "ProcessRate_Master");

            migrationBuilder.DropColumn(
                name: "GO_DOWN_TYPE",
                table: "Godown_Master");

            migrationBuilder.DropColumn(
                name: "SALE_TAG",
                table: "Godown_Master");
        }
    }
}
