using Microsoft.EntityFrameworkCore.Migrations;

namespace WebERP.Data.Migrations
{
    public partial class aaa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ART_NAME",
                table: "RM_HDR",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ITEM_NAME",
                table: "RM_HDR",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PROC_NAME",
                table: "RM_HDR",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SIZE_NAME",
                table: "RM_HDR",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ART_NAME",
                table: "RM_HDR");

            migrationBuilder.DropColumn(
                name: "ITEM_NAME",
                table: "RM_HDR");

            migrationBuilder.DropColumn(
                name: "PROC_NAME",
                table: "RM_HDR");

            migrationBuilder.DropColumn(
                name: "SIZE_NAME",
                table: "RM_HDR");
        }
    }
}
