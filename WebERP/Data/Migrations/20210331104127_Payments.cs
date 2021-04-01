using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebERP.Data.Migrations
{
    public partial class Payments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    COMP_CODE = table.Column<int>(nullable: true),
                    DOC_DATE = table.Column<DateTime>(nullable: true),
                    DOC_FN_YEAR = table.Column<string>(nullable: true),
                    DOC_NO = table.Column<int>(nullable: true),
                    ACC_CODE = table.Column<int>(nullable: true),
                    CB_ACC_CODE = table.Column<int>(nullable: true),
                    PAYMENT_TAG = table.Column<int>(nullable: true),
                    PAYMENT_MODE = table.Column<int>(nullable: true),
                    PAY_DOC_NO = table.Column<int>(nullable: true),
                    PAY_DOC_DATE = table.Column<DateTime>(nullable: true),
                    AMOUNT = table.Column<decimal>(nullable: true),
                    REMARKS = table.Column<string>(nullable: true),
                    INS_DATE = table.Column<DateTime>(nullable: true),
                    INS_UID = table.Column<string>(nullable: true),
                    UDT_DATE = table.Column<DateTime>(nullable: true),
                    UDT_UID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payments");
        }
    }
}
