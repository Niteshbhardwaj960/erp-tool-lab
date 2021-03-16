using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebERP.Data.Migrations
{
    public partial class Cuttingorder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cutting_Orders",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    COMP_CODE = table.Column<int>(nullable: true),
                    DOC_DATE = table.Column<DateTime>(nullable: true),
                    DOC_FINYEAR = table.Column<int>(nullable: true),
                    DOC_NO = table.Column<int>(nullable: true),
                    EMP_CODE = table.Column<int>(nullable: true),
                    CONT_EMP_CODE = table.Column<int>(nullable: true),
                    ITEM_CODE = table.Column<int>(nullable: true),
                    ARTICAL_CODE = table.Column<int>(nullable: true),
                    SIZE_CODE = table.Column<int>(nullable: true),
                    PROC_CODE = table.Column<int>(nullable: true),
                    ORDER_QTY = table.Column<int>(nullable: true),
                    AVG_PC_WEIGHT = table.Column<int>(nullable: true),
                    WASTAGE_PER = table.Column<int>(nullable: true),
                    ORDER_STATUS = table.Column<string>(nullable: true),
                    INS_DATE = table.Column<DateTime>(nullable: true),
                    INS_UID = table.Column<string>(nullable: true),
                    UDT_DATE = table.Column<DateTime>(nullable: true),
                    UDT_UID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cutting_Orders", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cutting_Orders");
        }
    }
}
