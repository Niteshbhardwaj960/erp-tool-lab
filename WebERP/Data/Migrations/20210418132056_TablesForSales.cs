using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebERP.Data.Migrations
{
    public partial class TablesForSales : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SalesHeader",
                columns: table => new
                {
                    SALE_PK = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    COMP_CODE = table.Column<int>(nullable: false),
                    ACC_CODE = table.Column<int>(nullable: false),
                    DOC_DATE = table.Column<DateTime>(nullable: false),
                    DOC_FINYEAR = table.Column<string>(nullable: true),
                    DOC_NO = table.Column<int>(nullable: false),
                    AGENTACC_CODE = table.Column<int>(nullable: false),
                    GATEOUT_DATE = table.Column<DateTime>(nullable: true),
                    GATEOUT_UID = table.Column<string>(nullable: true),
                    GROSS_AMT = table.Column<decimal>(nullable: false),
                    TAX_AMT = table.Column<decimal>(nullable: false),
                    OTH_AMTNAME1 = table.Column<string>(maxLength: 50, nullable: true),
                    OTH_AMT1 = table.Column<decimal>(nullable: false),
                    OTH_AMTNAME2 = table.Column<string>(maxLength: 50, nullable: true),
                    OTH_AMT2 = table.Column<decimal>(nullable: false),
                    RF_AMT = table.Column<decimal>(nullable: false),
                    NET_AMT = table.Column<decimal>(nullable: false),
                    TAX_CODE = table.Column<int>(nullable: false),
                    IGST_PER = table.Column<decimal>(nullable: false),
                    IGST_AMOUNT = table.Column<decimal>(nullable: false),
                    CGST_PER = table.Column<decimal>(nullable: false),
                    CGST_AMOUNT = table.Column<decimal>(nullable: false),
                    SGST_PER = table.Column<decimal>(nullable: false),
                    SGST_AMOUNT = table.Column<decimal>(nullable: false),
                    REMARKS = table.Column<string>(maxLength: 100, nullable: true),
                    INS_DATE = table.Column<DateTime>(nullable: true),
                    INS_UID = table.Column<string>(nullable: true),
                    UDT_DATE = table.Column<DateTime>(nullable: true),
                    UDT_UID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesHeader", x => x.SALE_PK);
                });

            migrationBuilder.CreateTable(
                name: "SalesDetails",
                columns: table => new
                {
                    SALEDTL_PK = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SALE_FK = table.Column<int>(nullable: false),
                    GODOWN_CODE = table.Column<int>(nullable: false),
                    ITEM_CODE = table.Column<int>(nullable: false),
                    ARTICAL_CODE = table.Column<int>(nullable: false),
                    SIZE_CODE = table.Column<int>(nullable: false),
                    HSN_CODE = table.Column<int>(nullable: false),
                    SALE_QTY = table.Column<decimal>(nullable: false),
                    SALEQTY_UOM = table.Column<string>(nullable: true),
                    RATE = table.Column<decimal>(nullable: false),
                    DISCPER_TAG = table.Column<string>(maxLength: 3, nullable: true),
                    DISC_PER = table.Column<decimal>(nullable: false),
                    DISC_RATE = table.Column<decimal>(nullable: false),
                    NET_RATE = table.Column<decimal>(nullable: false),
                    ITEM_AMOUNT = table.Column<decimal>(nullable: false),
                    REMARKS = table.Column<string>(maxLength: 100, nullable: true),
                    INS_DATE = table.Column<DateTime>(nullable: true),
                    INS_UID = table.Column<string>(nullable: true),
                    UDT_DATE = table.Column<DateTime>(nullable: true),
                    UDT_UID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesDetails", x => x.SALEDTL_PK);
                    table.ForeignKey(
                        name: "FK_SalesDetails_SalesHeader_SALE_FK",
                        column: x => x.SALE_FK,
                        principalTable: "SalesHeader",
                        principalColumn: "SALE_PK",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalesDetails_SALE_FK",
                table: "SalesDetails",
                column: "SALE_FK");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SalesDetails");

            migrationBuilder.DropTable(
                name: "SalesHeader");
        }
    }
}
