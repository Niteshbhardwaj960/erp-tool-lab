using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebERP.Data.Migrations
{
    public partial class EMPSALPCCONTDTL : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Emp_Sal_PC_Cont_Dtl",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EMP_SAL_FK = table.Column<int>(nullable: false),
                    ART_CODE = table.Column<int>(nullable: false),
                    PROC_CODE = table.Column<int>(nullable: false),
                    PRODUCT_QTY = table.Column<decimal>(nullable: false),
                    PRODUCT_RATE = table.Column<decimal>(nullable: false),
                    ART_AMOUNT = table.Column<decimal>(nullable: false),
                    INS_DATE = table.Column<DateTime>(nullable: true),
                    INS_UID = table.Column<string>(nullable: true),
                    UDT_DATE = table.Column<DateTime>(nullable: true),
                    UDT_UID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emp_Sal_PC_Cont_Dtl", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Emp_Sal_PC_Cont_Dtl");
        }
    }
}
