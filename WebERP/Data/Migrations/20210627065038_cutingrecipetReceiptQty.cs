using Microsoft.EntityFrameworkCore.Migrations;

namespace WebERP.Data.Migrations
{
    public partial class cutingrecipetReceiptQty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "RECEIPT_QTY",
                table: "Cutting_Receipt",
                nullable: false,
                oldClrType: typeof(decimal));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "RECEIPT_QTY",
                table: "Cutting_Receipt",
                nullable: false,
                oldClrType: typeof(int));
        }
    }
}
