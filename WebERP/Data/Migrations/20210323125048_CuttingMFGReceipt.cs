using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebERP.Data.Migrations
{
    public partial class CuttingMFGReceipt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cutting_Receipt",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    COMP_CODE = table.Column<int>(nullable: false),
                    DOC_DATE = table.Column<DateTime>(nullable: true),
                    DOC_FINYEAR = table.Column<int>(nullable: false),
                    DOC_NO = table.Column<int>(nullable: false),
                    CUTTING_ORDER_FK = table.Column<int>(nullable: false),
                    RECEIPT_QTY = table.Column<decimal>(nullable: false),
                    EMP_NAME = table.Column<string>(nullable: true),
                    ITEM_NAME = table.Column<string>(nullable: true),
                    ART_NAME = table.Column<string>(nullable: true),
                    SIZE_NAME = table.Column<string>(nullable: true),
                    PROC_NAME = table.Column<string>(nullable: true),
                    INS_DATE = table.Column<DateTime>(nullable: true),
                    INS_UID = table.Column<string>(nullable: true),
                    UDT_DATE = table.Column<DateTime>(nullable: true),
                    UDT_UID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cutting_Receipt", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MGF_RECEIPT",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    COMP_CODE = table.Column<int>(nullable: false),
                    DOC_DATE = table.Column<DateTime>(nullable: true),
                    DOC_FINYEAR = table.Column<int>(nullable: false),
                    DOC_NO = table.Column<int>(nullable: false),
                    PROC_CODE = table.Column<int>(nullable: false),
                    EMP_CODE = table.Column<int>(nullable: false),
                    CONT_EMP_CODE = table.Column<int>(nullable: false),
                    CUTTING_ORDER_FK = table.Column<int>(nullable: false),
                    RECEIPT_QTY = table.Column<decimal>(nullable: false),
                    EMP_NAME = table.Column<string>(nullable: true),
                    ITEM_NAME = table.Column<string>(nullable: true),
                    ART_NAME = table.Column<string>(nullable: true),
                    SIZE_NAME = table.Column<string>(nullable: true),
                    PROC_NAME = table.Column<string>(nullable: true),
                    INS_DATE = table.Column<DateTime>(nullable: true),
                    INS_UID = table.Column<string>(nullable: true),
                    UDT_DATE = table.Column<DateTime>(nullable: true),
                    UDT_UID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MGF_RECEIPT", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cutting_Receipt");

            migrationBuilder.DropTable(
                name: "MGF_RECEIPT");
        }
    }
}
