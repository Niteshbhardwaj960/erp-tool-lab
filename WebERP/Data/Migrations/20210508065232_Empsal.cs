using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebERP.Data.Migrations
{
    public partial class Empsal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EMP_SAL",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    COMP_CODE = table.Column<int>(nullable: true),
                    SAL_MONTH = table.Column<DateTime>(nullable: true),
                    SAL_TYPE = table.Column<int>(nullable: true),
                    EMP_CODE = table.Column<int>(nullable: true),
                    EMP_TYPE = table.Column<string>(nullable: true),
                    EMP_NAME = table.Column<string>(nullable: true),
                    SALARY = table.Column<decimal>(nullable: true),
                    SHIFT_HRS = table.Column<decimal>(nullable: true),
                    PAY_DAYS = table.Column<decimal>(nullable: true),
                    OT_HRS = table.Column<decimal>(nullable: true),
                    ERN_SAL = table.Column<decimal>(nullable: true),
                    ERN_OT = table.Column<decimal>(nullable: true),
                    ADVANCE_AMOUNT = table.Column<decimal>(nullable: true),
                    PAYABAL_SALARY = table.Column<decimal>(nullable: true),
                    RF_SAL = table.Column<decimal>(nullable: true),
                    NET_PAY_SAL = table.Column<decimal>(nullable: true),
                    PAID_SAL = table.Column<decimal>(nullable: true),
                    PAID_DATE = table.Column<DateTime>(nullable: true),
                    PAID_USER = table.Column<string>(nullable: true),
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
