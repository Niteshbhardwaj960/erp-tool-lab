using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebERP.Data.Migrations
{
    public partial class newcolumnGateEntry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Art_Name",
                table: "gateEntryDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Proc_Name",
                table: "gateEntryDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Size_Name",
                table: "gateEntryDetails",
                nullable: false,
                defaultValue: 0);
          
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Art_Name",
                table: "gateEntryDetails");

            migrationBuilder.DropColumn(
                name: "Proc_Name",
                table: "gateEntryDetails");

            migrationBuilder.DropColumn(
                name: "Size_Name",
                table: "gateEntryDetails");
        }
    }
}
