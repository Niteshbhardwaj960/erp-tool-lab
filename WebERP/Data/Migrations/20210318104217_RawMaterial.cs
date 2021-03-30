using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebERP.Data.Migrations
{
    public partial class RawMaterial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           migrationBuilder.CreateTable(
                name: "RM_DTL",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RM_HDR_FK = table.Column<int>(nullable: true),
                    GDW_Code = table.Column<int>(nullable: true),
                    ITEM_Code = table.Column<int>(nullable: true),
                    ARTICAL_Code = table.Column<int>(nullable: true),
                    BRAND_Code = table.Column<int>(nullable: true),
                    SIZE_Code = table.Column<int>(nullable: true),
                    ISSUE_QTY = table.Column<decimal>(nullable: true),
                    ORDER_QTY = table.Column<decimal>(nullable: true),
                    INS_DATE = table.Column<DateTime>(nullable: true),
                    INS_UID = table.Column<string>(nullable: true),
                    UDT_DATE = table.Column<DateTime>(nullable: true),
                    UDT_UID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RM_DTL", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "RM_HDR",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Comp_Code = table.Column<int>(nullable: true),
                    Doc_Date = table.Column<DateTime>(nullable: true),
                    Doc_FN_Year = table.Column<string>(nullable: true),
                    Doc_No = table.Column<int>(nullable: true),
                    Cutting_Order_FK = table.Column<int>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    INS_DATE = table.Column<DateTime>(nullable: true),
                    INS_UID = table.Column<string>(nullable: true),
                    UDT_DATE = table.Column<DateTime>(nullable: true),
                    UDT_UID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RM_HDR", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RM_DTL");

            migrationBuilder.DropTable(
                name: "RM_HDR");
            
        }
    }
}
