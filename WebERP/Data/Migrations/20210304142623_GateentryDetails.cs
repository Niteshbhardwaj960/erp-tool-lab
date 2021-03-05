using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebERP.Data.Migrations
{
    public partial class GateentryDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "gateEntryDetails",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GH_FK = table.Column<int>(nullable: false),
                    POD_FK = table.Column<int>(nullable: false),
                    JW_FK = table.Column<int>(nullable: false),
                    Order_No = table.Column<int>(nullable: false),
                    Item_Name = table.Column<int>(nullable: false),
                    Item_UOM = table.Column<string>(nullable: true),
                    CHL_NO = table.Column<string>(nullable: true),
                    CHL_DATE = table.Column<DateTime>(nullable: true),
                    Bill_NO = table.Column<string>(nullable: true),
                    Bill_Date = table.Column<DateTime>(nullable: true),
                    Stk_Qty = table.Column<int>(nullable: false),
                    Stk_UOM = table.Column<int>(nullable: false),
                    Fin_Qty = table.Column<int>(nullable: false),
                    Fin_UOM = table.Column<int>(nullable: false),
                    INS_DATE = table.Column<DateTime>(nullable: true),
                    INS_UID = table.Column<string>(nullable: true),
                    UDT_DATE = table.Column<DateTime>(nullable: true),
                    UDT_UID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gateEntryDetails", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "gateEntryDetails");            
        }
    }
}
