using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebERP.Data.Migrations
{
    public partial class Company : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NAME = table.Column<string>(nullable: true),
                    ABV = table.Column<string>(nullable: true),
                    ADD1 = table.Column<string>(nullable: true),
                    ADD2 = table.Column<string>(nullable: true),
                    CITY_CODE = table.Column<string>(nullable: true),
                    PIN_CODE = table.Column<string>(nullable: true),
                    MOBILE_NO = table.Column<string>(nullable: true),
                    URL = table.Column<string>(nullable: true),
                    EMAIL_ID = table.Column<string>(nullable: true),
                    PH_NO = table.Column<string>(nullable: true),
                    FAX_NO = table.Column<string>(nullable: true),
                    LST_NO = table.Column<string>(nullable: true),
                    LST_DATE = table.Column<DateTime>(nullable: true),
                    CST_NO = table.Column<string>(nullable: true),
                    CST_DAT = table.Column<DateTime>(nullable: true),
                    TIN_NO = table.Column<string>(nullable: true),
                    GST_NO = table.Column<string>(nullable: true),
                    ECC_NO = table.Column<string>(nullable: true),
                    SERVICE_TAX_NO = table.Column<string>(nullable: true),
                    PAN_NO = table.Column<string>(nullable: true),
                    IFSC_CODE = table.Column<string>(nullable: true),
                    TDS_NO = table.Column<string>(nullable: true),
                    ESI_NO = table.Column<string>(nullable: true),
                    PF_NO = table.Column<string>(nullable: true),
                    MSME_NO = table.Column<string>(nullable: true),
                    LOGO_NAME = table.Column<string>(nullable: true),
                    BANK_NAME = table.Column<string>(nullable: true),
                    BANK_ACC_NO = table.Column<string>(nullable: true),
                    BANK_BRANCH = table.Column<string>(nullable: true),
                    ACTIVE_TAG = table.Column<string>(nullable: true),
                    REMARKS = table.Column<string>(nullable: true),
                    INS_DATE = table.Column<DateTime>(nullable: true),
                    INS_UID = table.Column<string>(nullable: true),
                    UDT_DATE = table.Column<DateTime>(nullable: true),
                    UDT_UID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}
