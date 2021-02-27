using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebERP.Data.Migrations
{
    public partial class AddingPOTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_States_StateId",
                table: "Cities");

            migrationBuilder.DropForeignKey(
                name: "FK_States_Countries_CountryId",
                table: "States");

            migrationBuilder.DropIndex(
                name: "IX_States_CountryId",
                table: "States");

            migrationBuilder.DropIndex(
                name: "IX_Cities_StateId",
                table: "Cities");

            migrationBuilder.RenameColumn(
                name: "StateCode",
                table: "Cities",
                newName: "CityCode");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Upd_Date",
                table: "States",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<string>(
                name: "StateCode",
                table: "States",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "States",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Ins_Date",
                table: "States",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Upd_Date",
                table: "Countries",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Countries",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Ins_Date",
                table: "Countries",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<string>(
                name: "CountryCode",
                table: "Countries",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Upd_Date",
                table: "Cities",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Cities",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Ins_Date",
                table: "Cities",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.CreateTable(
                name: "POHeader_Master",
                columns: table => new
                {
                    POH_PK = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    COMP_CODE = table.Column<int>(nullable: false),
                    ORDER_DATE = table.Column<DateTime>(nullable: false),
                    ORDER_FINYEAR = table.Column<string>(nullable: true),
                    ORDER_NO = table.Column<int>(nullable: false),
                    ACC_CODE = table.Column<int>(nullable: false),
                    REMARKS = table.Column<string>(maxLength: 100, nullable: true),
                    INS_DATE = table.Column<DateTime>(nullable: true),
                    INS_UID = table.Column<string>(nullable: true),
                    UDT_DATE = table.Column<DateTime>(nullable: true),
                    UDT_UID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_POHeader_Master", x => x.POH_PK);
                });

            migrationBuilder.CreateTable(
                name: "PODetail_Master",
                columns: table => new
                {
                    POD_PK = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    POH_FK = table.Column<int>(nullable: false),
                    ITEM_CODE = table.Column<int>(nullable: false),
                    QTY = table.Column<decimal>(nullable: false),
                    QTY_UOM = table.Column<int>(nullable: false),
                    RATE = table.Column<decimal>(nullable: false),
                    DISC_PER = table.Column<decimal>(nullable: false),
                    DISC_RATE = table.Column<decimal>(nullable: false),
                    NET_RATE = table.Column<decimal>(nullable: false),
                    RATE_UOM = table.Column<int>(nullable: false),
                    DELV_DATE = table.Column<DateTime>(nullable: false),
                    POD_PK_STATUS = table.Column<string>(maxLength: 3, nullable: true),
                    REMARKS = table.Column<string>(maxLength: 100, nullable: true),
                    APPROVED_UID = table.Column<string>(nullable: true),
                    APPROVED_DATE = table.Column<DateTime>(nullable: true),
                    INS_DATE = table.Column<DateTime>(nullable: true),
                    INS_UID = table.Column<string>(nullable: true),
                    UDT_DATE = table.Column<DateTime>(nullable: true),
                    UDT_UID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PODetail_Master", x => x.POD_PK);
                    table.ForeignKey(
                        name: "FK_PODetail_Master_POHeader_Master_POH_FK",
                        column: x => x.POH_FK,
                        principalTable: "POHeader_Master",
                        principalColumn: "POH_PK",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "POTerm_Master",
                columns: table => new
                {
                    POT_PK = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    POH_FK = table.Column<int>(nullable: false),
                    TERMS_CODE = table.Column<int>(nullable: false),
                    REMARKS = table.Column<string>(maxLength: 100, nullable: true),
                    INS_DATE = table.Column<DateTime>(nullable: true),
                    INS_UID = table.Column<string>(nullable: true),
                    UDT_DATE = table.Column<DateTime>(nullable: true),
                    UDT_UID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_POTerm_Master", x => x.POT_PK);
                    table.ForeignKey(
                        name: "FK_POTerm_Master_POHeader_Master_POH_FK",
                        column: x => x.POH_FK,
                        principalTable: "POHeader_Master",
                        principalColumn: "POH_PK",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PODetail_Master_POH_FK",
                table: "PODetail_Master",
                column: "POH_FK");

            migrationBuilder.CreateIndex(
                name: "IX_POTerm_Master_POH_FK",
                table: "POTerm_Master",
                column: "POH_FK");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PODetail_Master");

            migrationBuilder.DropTable(
                name: "POTerm_Master");

            migrationBuilder.DropTable(
                name: "POHeader_Master");

            migrationBuilder.RenameColumn(
                name: "CityCode",
                table: "Cities",
                newName: "StateCode");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Upd_Date",
                table: "States",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StateCode",
                table: "States",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "States",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Ins_Date",
                table: "States",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Upd_Date",
                table: "Countries",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Countries",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Ins_Date",
                table: "Countries",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CountryCode",
                table: "Countries",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Upd_Date",
                table: "Cities",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Cities",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Ins_Date",
                table: "Cities",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_States_CountryId",
                table: "States",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_StateId",
                table: "Cities",
                column: "StateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_States_StateId",
                table: "Cities",
                column: "StateId",
                principalTable: "States",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_States_Countries_CountryId",
                table: "States",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
