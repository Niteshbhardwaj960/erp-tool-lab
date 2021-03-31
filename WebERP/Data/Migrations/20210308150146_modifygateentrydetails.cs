using Microsoft.EntityFrameworkCore.Migrations;

namespace WebERP.Data.Migrations
{
    public partial class modifygateentrydetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ACC_NAME",
                table: "gateEntryDetails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Doc_No",
                table: "gateEntryDetails",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ACC_NAME",
                table: "gateEntryDetails");

            migrationBuilder.DropColumn(
                name: "Doc_No",
                table: "gateEntryDetails");
        }
    }
}
