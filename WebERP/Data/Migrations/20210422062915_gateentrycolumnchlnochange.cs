using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebERP.Data.Migrations
{
    public partial class gateentrycolumnchlnochange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TAX_NAME",
                table: "TAX_MASTER",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CHL_NO",
                table: "gateEntryDetails",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {           
            migrationBuilder.AlterColumn<string>(
                name: "TAX_NAME",
                table: "TAX_MASTER",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "CHL_NO",
                table: "gateEntryDetails",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
