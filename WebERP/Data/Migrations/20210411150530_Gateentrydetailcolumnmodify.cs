using Microsoft.EntityFrameworkCore.Migrations;

namespace WebERP.Data.Migrations
{
    public partial class Gateentrydetailcolumnmodify : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Stk_Qty",
                table: "gateEntryDetails",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<decimal>(
                name: "Fin_Qty",
                table: "gateEntryDetails",
                nullable: false,
                oldClrType: typeof(int));
            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Stk_Qty",
                table: "gateEntryDetails",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<int>(
                name: "Fin_Qty",
                table: "gateEntryDetails",
                nullable: false,
                oldClrType: typeof(decimal));
        }
    }
}
