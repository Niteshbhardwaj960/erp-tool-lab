using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebERP.Data.Migrations
{
    public partial class gateentrydetailchanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.AlterColumn<int>(
                name: "Item_UOM",
                table: "gateEntryDetails",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DOC_DATE",
                table: "gateEntryDetails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FIN_YEAR",
                table: "gateEntryDetails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "gateEntryDetails",
                nullable: true);

            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "DOC_DATE",
                table: "gateEntryDetails");

            migrationBuilder.DropColumn(
                name: "FIN_YEAR",
                table: "gateEntryDetails");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "gateEntryDetails");

            migrationBuilder.AlterColumn<string>(
                name: "Item_UOM",
                table: "gateEntryDetails",
                nullable: true,
                oldClrType: typeof(int));
        }

    }
}
