using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebERP.Data.Migrations
{
    public partial class TAXMASTER : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "JW_RATE",
                table: "JobWorkIssue_Details",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "TAX_MASTER",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TAX_NAME = table.Column<string>(nullable: true),
                    IGST_PER = table.Column<decimal>(nullable: false),
                    CGST_PER = table.Column<decimal>(nullable: false),
                    SGST_PER = table.Column<decimal>(nullable: false),
                    ACTIVE_TAG = table.Column<string>(nullable: true),
                    INS_DATE = table.Column<DateTime>(nullable: true),
                    INS_UID = table.Column<string>(nullable: true),
                    UDT_DATE = table.Column<DateTime>(nullable: true),
                    UDT_UID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TAX_MASTER", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TAX_MASTER");

            migrationBuilder.DropColumn(
                name: "JW_RATE",
                table: "JobWorkIssue_Details");
        }
    }
}
