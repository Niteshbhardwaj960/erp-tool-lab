using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebERP.Data.Migrations
{
    public partial class EMPSAL : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EMP_SAL",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    COMP_CODE = table.Column<int>(nullable: false),
                    SAL_MONTH = table.Column<DateTime>(nullable: true),
                    SAL_TYPE = table.Column<int>(nullable: false),
                    EMP_CODE = table.Column<int>(nullable: false),
                    EMP_NAME = table.Column<string>(nullable: true),
                    SALARY = table.Column<decimal>(nullable: false),
                    SHIFT_HRS = table.Column<decimal>(nullable: false),
                    PAY_DAYS = table.Column<decimal>(nullable: false),
                    OT_HRS = table.Column<decimal>(nullable: false),
                    ERN_SAL = table.Column<decimal>(nullable: false),
                    ERN_OT = table.Column<decimal>(nullable: false),
                    ADVANCE_AMOUNT = table.Column<decimal>(nullable: false),
                    PAYABAL_SALARY = table.Column<decimal>(nullable: false),
                    RF_SAL = table.Column<decimal>(nullable: false),
                    NET_PAY_SAL = table.Column<decimal>(nullable: false),
                    PAID_SAL = table.Column<decimal>(nullable: false),
                    PAID_DATE = table.Column<decimal>(nullable: false),
                    PAID_USER = table.Column<decimal>(nullable: false),
                    INS_DATE = table.Column<DateTime>(nullable: true),
                    INS_UID = table.Column<string>(nullable: true),
                    UDT_DATE = table.Column<DateTime>(nullable: true),
                    UDT_UID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EMP_SAL", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EMP_SAL");
        }
    }
}
