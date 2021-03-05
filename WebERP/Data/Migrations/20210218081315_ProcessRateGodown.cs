using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebERP.Data.Migrations
{
    public partial class ProcessRateGodown : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UDT_DATE",
                table: "UOM_MASTER",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "INS_DATE",
                table: "UOM_MASTER",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<string>(
                name: "ABV",
                table: "UOM_MASTER",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UDT_DATE",
                table: "Term_Master",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "INS_DATE",
                table: "Term_Master",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UDT_DATE",
                table: "Size_Master",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "INS_DATE",
                table: "Size_Master",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UDT_DATE",
                table: "Item_Master",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "INS_DATE",
                table: "Item_Master",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "LST_DATE",
                table: "Companies",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CST_DAT",
                table: "Companies",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<string>(
                name: "ABV",
                table: "Companies",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UDT_DATE",
                table: "Brand_Master",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "INS_DATE",
                table: "Brand_Master",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<string>(
                name: "ABV",
                table: "Brand_Master",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UDT_DATE",
                table: "Artical_Master",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "INS_DATE",
                table: "Artical_Master",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UDT_DATE",
                table: "Account_Masters",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "INS_DATE",
                table: "Account_Masters",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<string>(
                name: "GST_NO",
                table: "Account_Masters",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Godown_Master",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NAME = table.Column<string>(nullable: false),
                    ABV = table.Column<string>(nullable: true),
                    INS_DATE = table.Column<DateTime>(nullable: true),
                    INS_UID = table.Column<string>(nullable: true),
                    UDT_DATE = table.Column<DateTime>(nullable: true),
                    UDT_UID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Godown_Master", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Process_Master",
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
                    table.PrimaryKey("PK_Process_Master", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ProcessRate_Master",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Proc_Code = table.Column<string>(nullable: false),
                    UOM_Code = table.Column<string>(nullable: false),
                    From_DATE = table.Column<DateTime>(nullable: true),
                    To_DATE = table.Column<DateTime>(nullable: true),
                    INS_DATE = table.Column<DateTime>(nullable: true),
                    INS_UID = table.Column<string>(nullable: true),
                    UDT_DATE = table.Column<DateTime>(nullable: true),
                    UDT_UID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessRate_Master", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Godown_Master");

            migrationBuilder.DropTable(
                name: "Process_Master");

            migrationBuilder.DropTable(
                name: "ProcessRate_Master");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UDT_DATE",
                table: "UOM_MASTER",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "INS_DATE",
                table: "UOM_MASTER",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ABV",
                table: "UOM_MASTER",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UDT_DATE",
                table: "Term_Master",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "INS_DATE",
                table: "Term_Master",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UDT_DATE",
                table: "Size_Master",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "INS_DATE",
                table: "Size_Master",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UDT_DATE",
                table: "Item_Master",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "INS_DATE",
                table: "Item_Master",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LST_DATE",
                table: "Companies",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CST_DAT",
                table: "Companies",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ABV",
                table: "Companies",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UDT_DATE",
                table: "Brand_Master",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "INS_DATE",
                table: "Brand_Master",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ABV",
                table: "Brand_Master",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UDT_DATE",
                table: "Artical_Master",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "INS_DATE",
                table: "Artical_Master",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UDT_DATE",
                table: "Account_Masters",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "INS_DATE",
                table: "Account_Masters",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GST_NO",
                table: "Account_Masters",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
