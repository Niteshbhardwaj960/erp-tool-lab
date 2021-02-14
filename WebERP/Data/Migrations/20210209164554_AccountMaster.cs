using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebERP.Data.Migrations
{
    public partial class AccountMaster : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account_Masters",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NAME = table.Column<string>(nullable: false),
                    ADD1 = table.Column<string>(nullable: false),
                    ADD2 = table.Column<string>(nullable: true),
                    CITY_CODE = table.Column<string>(nullable: true),
                    PIN_CODE = table.Column<string>(nullable: false),
                    GST_REGD_TAG = table.Column<string>(nullable: true),
                    GST_NO = table.Column<string>(nullable: true),
                    MOBILE_NO = table.Column<string>(nullable: false),
                    EMAIL_ID = table.Column<string>(nullable: false),
                    PH_NO = table.Column<string>(nullable: true),
                    OP_BAL = table.Column<string>(nullable: true),
                    OP_BAL_TAG = table.Column<string>(nullable: true),
                    ACC_TYPE = table.Column<string>(nullable: true),
                    CR_LIMIT = table.Column<string>(nullable: true),
                    CR_DAYS = table.Column<string>(nullable: true),
                    ACTIVE_TAG = table.Column<string>(nullable: true),
                    REMARKS = table.Column<string>(nullable: true),
                    INS_DATE = table.Column<DateTime>(nullable: true),
                    INS_UID = table.Column<string>(nullable: true),
                    UDT_DATE = table.Column<DateTime>(nullable: true),
                    UDT_UID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account_Masters", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Account_Masters");            
        }
    }
}
