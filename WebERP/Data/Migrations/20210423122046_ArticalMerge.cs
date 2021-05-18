using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebERP.Data.Migrations
{
    public partial class ArticalMerge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Artical_Merge_DTL",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HDR_FK = table.Column<int>(nullable: true),
                    GDW_CODE = table.Column<int>(nullable: true),
                    ITEM_CODE = table.Column<int>(nullable: true),
                    ARTICAL_CODE = table.Column<int>(nullable: true),
                    SIZE_CODE = table.Column<int>(nullable: true),
                    STK_QTY_OUT = table.Column<decimal>(nullable: true),
                    INS_DATE = table.Column<DateTime>(nullable: true),
                    INS_UID = table.Column<string>(nullable: true),
                    UDT_DATE = table.Column<DateTime>(nullable: true),
                    UDT_UID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artical_Merge_DTL", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Artical_Merge_HDR",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    COMP_CODE = table.Column<int>(nullable: true),
                    DOC_DATE = table.Column<DateTime>(nullable: true),
                    DOC_FN_YEAR = table.Column<string>(nullable: true),
                    DOC_NO = table.Column<int>(nullable: true),
                    GDW_CODE = table.Column<int>(nullable: true),
                    ITEM_CODE = table.Column<int>(nullable: true),
                    ARTICAL_CODE = table.Column<int>(nullable: true),
                    SIZE_CODE = table.Column<int>(nullable: true),
                    STK_QTY_IN = table.Column<decimal>(nullable: true),
                    INS_DATE = table.Column<DateTime>(nullable: true),
                    INS_UID = table.Column<string>(nullable: true),
                    UDT_DATE = table.Column<DateTime>(nullable: true),
                    UDT_UID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artical_Merge_HDR", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Artical_Merge_DTL");

            migrationBuilder.DropTable(
                name: "Artical_Merge_HDR");
        }
    }
}
