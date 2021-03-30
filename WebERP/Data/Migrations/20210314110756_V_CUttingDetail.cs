using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebERP.Data.Migrations
{
    public partial class V_CUttingDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "V_CuttingDetails",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    COMP_CODE = table.Column<int>(nullable: false),
                    DOC_DATE = table.Column<DateTime>(nullable: true),
                    DOC_FINYEAR = table.Column<int>(nullable: false),
                    DOC_NO = table.Column<int>(nullable: false),
                    EMP_CODE = table.Column<int>(nullable: false),
                    CONT_EMP_CODE = table.Column<int>(nullable: false),
                    ITEM_CODE = table.Column<int>(nullable: false),
                    ARTICAL_CODE = table.Column<int>(nullable: false),
                    SIZE_CODE = table.Column<int>(nullable: false),
                    PROC_CODE = table.Column<int>(nullable: false),
                    ORDER_QTY = table.Column<int>(nullable: false),
                    AVG_PC_WEIGHT = table.Column<int>(nullable: false),
                    WASTAGE_PER = table.Column<int>(nullable: false),
                    ORDER_STATUS = table.Column<string>(nullable: true),
                    INS_DATE = table.Column<DateTime>(nullable: true),
                    INS_UID = table.Column<string>(nullable: true),
                    UDT_DATE = table.Column<DateTime>(nullable: true),
                    UDT_UID = table.Column<string>(nullable: true),
                    EMP_NAME = table.Column<int>(nullable: false),
                    CONT_EMP_NAME = table.Column<int>(nullable: false),
                    ITEM_NAME = table.Column<int>(nullable: false),
                    ARTICAL_NAME = table.Column<int>(nullable: false),
                    SIZE_NAME = table.Column<int>(nullable: false),
                    PROC_NAME = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_V_CuttingDetails", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "V_CuttingDetails");
        }
    }
}
