using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebERP.Data.Migrations
{
    public partial class EmpAttendanceEmpAdvance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employee_Advance",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    COMP_CODE = table.Column<int>(nullable: true),
                    DOC_DATE = table.Column<DateTime>(nullable: true),
                    EMP_TYPE = table.Column<int>(nullable: true),
                    EMP_CODE = table.Column<int>(nullable: true),
                    SAL_YYYYMM = table.Column<DateTime>(nullable: true),
                    SAL_YYYYMM_BRK = table.Column<int>(nullable: true),
                    PAY_DAYS = table.Column<decimal>(nullable: true),
                    OT_HRS = table.Column<decimal>(nullable: true),
                    INS_DATE = table.Column<DateTime>(nullable: true),
                    INS_UID = table.Column<string>(nullable: true),
                    UDT_DATE = table.Column<DateTime>(nullable: true),
                    UDT_UID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee_Advance", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Employee_Attandance",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    COMP_CODE = table.Column<int>(nullable: true),
                    DOC_DATE = table.Column<DateTime>(nullable: true),
                    EMP_TYPE = table.Column<int>(nullable: true),
                    EMP_CODE = table.Column<int>(nullable: true),
                    SAL_YYYYMM = table.Column<DateTime>(nullable: true),
                    SAL_YYYYMM_BRK = table.Column<int>(nullable: true),
                    ADV_AMOUNT = table.Column<decimal>(nullable: true),
                    INS_DATE = table.Column<DateTime>(nullable: true),
                    INS_UID = table.Column<string>(nullable: true),
                    UDT_DATE = table.Column<DateTime>(nullable: true),
                    UDT_UID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee_Attandance", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employee_Advance");

            migrationBuilder.DropTable(
                name: "Employee_Attandance");
        }
    }
}
