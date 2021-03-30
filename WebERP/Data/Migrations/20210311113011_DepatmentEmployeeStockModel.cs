using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebERP.Data.Migrations
{
    public partial class DepatmentEmployeeStockModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Department_Masters",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NAME = table.Column<string>(nullable: false),
                    INS_DATE = table.Column<DateTime>(nullable: true),
                    INS_UID = table.Column<string>(nullable: true),
                    UDT_DATE = table.Column<DateTime>(nullable: true),
                    UDT_UID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department_Masters", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Employee_Masters",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    COMP_CODE = table.Column<int>(nullable: false),
                    EMP_CODE = table.Column<string>(nullable: false),
                    EMP_NAME = table.Column<string>(nullable: false),
                    DEP_CODE = table.Column<int>(nullable: false),
                    EMP_TYPE = table.Column<string>(nullable: false),
                    Emp_Father_Name = table.Column<string>(nullable: false),
                    emp_mobile_no1 = table.Column<string>(nullable: false),
                    emp_mobile_no2 = table.Column<string>(nullable: true),
                    emp_doj = table.Column<DateTime>(nullable: false),
                    emp_salary = table.Column<int>(nullable: false),
                    active_tag = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    NAME = table.Column<string>(nullable: true),
                    INS_DATE = table.Column<DateTime>(nullable: true),
                    INS_UID = table.Column<string>(nullable: true),
                    UDT_DATE = table.Column<DateTime>(nullable: true),
                    UDT_UID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee_Masters", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "StockDTL_Models",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    COMP_CODE = table.Column<int>(nullable: false),
                    Tran_Table = table.Column<string>(nullable: true),
                    Tran_Table_PK = table.Column<int>(nullable: false),
                    GDW_CODE = table.Column<int>(nullable: false),
                    Item_Code = table.Column<int>(nullable: false),
                    Artical_CODE = table.Column<int>(nullable: false),
                    Size_Code = table.Column<int>(nullable: false),
                    Stk_Qty_IN = table.Column<int>(nullable: false),
                    Stk_Qty_OUT = table.Column<int>(nullable: false),
                    INS_DATE = table.Column<DateTime>(nullable: true),
                    INS_UID = table.Column<string>(nullable: true),
                    UDT_DATE = table.Column<DateTime>(nullable: true),
                    UDT_UID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockDTL_Models", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Department_Masters");

            migrationBuilder.DropTable(
                name: "Employee_Masters");

            migrationBuilder.DropTable(
                name: "StockDTL_Models");
        }
    }
}
