using Microsoft.EntityFrameworkCore.Migrations;

namespace WebERP.Data.Migrations
{
    public partial class GoDownDatatypechange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Stk_Qty_OUT",
                table: "StockDTL_Models",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<decimal>(
                name: "Stk_Qty_IN",
                table: "StockDTL_Models",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Stk_Qty_OUT",
                table: "StockDTL_Models",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<int>(
                name: "Stk_Qty_IN",
                table: "StockDTL_Models",
                nullable: false,
                oldClrType: typeof(decimal));
        }
    }
}
