using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebERP.Data.Migrations
{
    public partial class AddJobWorkTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PODetailModel");

            migrationBuilder.DropTable(
                name: "POHeaderModel");

            migrationBuilder.AlterColumn<string>(
                name: "StateCode",
                table: "States",
                maxLength: 3,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "QTY_UOM",
                table: "PODetail_Master",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "CountryCode",
                table: "Countries",
                maxLength: 3,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "CityCode",
                table: "Cities",
                maxLength: 3,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "JobWorkIssue_Header",
                columns: table => new
                {
                    JWH_PK = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    COMP_CODE = table.Column<int>(nullable: false),
                    ACC_CODE = table.Column<int>(nullable: false),
                    DOC_DATE = table.Column<DateTime>(nullable: false),
                    DOC_FINYEAR = table.Column<string>(nullable: true),
                    DOC_NO = table.Column<int>(nullable: false),
                    REMARKS = table.Column<string>(maxLength: 100, nullable: true),
                    INS_DATE = table.Column<DateTime>(nullable: true),
                    INS_UID = table.Column<string>(nullable: true),
                    UDT_DATE = table.Column<DateTime>(nullable: true),
                    UDT_UID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobWorkIssue_Header", x => x.JWH_PK);
                });

            
            migrationBuilder.CreateTable(
                name: "JobWorkIssue_Details",
                columns: table => new
                {
                    JWD_PK = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    JWH_FK = table.Column<int>(nullable: false),
                    PROC_CODE = table.Column<int>(nullable: false),
                    HSN_CODE = table.Column<int>(nullable: false),
                    GODOWN_CODE = table.Column<int>(nullable: false),
                    ITEM_CODE = table.Column<int>(nullable: false),
                    ARTICAL_CODE = table.Column<int>(nullable: false),
                    SIZE_CODE = table.Column<int>(nullable: false),
                    QTY = table.Column<decimal>(nullable: false),
                    QTY_UOM = table.Column<string>(nullable: true),
                    REMARKS = table.Column<string>(maxLength: 100, nullable: true),
                    INS_DATE = table.Column<DateTime>(nullable: true),
                    INS_UID = table.Column<string>(nullable: true),
                    UDT_DATE = table.Column<DateTime>(nullable: true),
                    UDT_UID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobWorkIssue_Details", x => x.JWD_PK);
                    table.ForeignKey(
                        name: "FK_JobWorkIssue_Details_JobWorkIssue_Header_JWH_FK",
                        column: x => x.JWH_FK,
                        principalTable: "JobWorkIssue_Header",
                        principalColumn: "JWH_PK",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobWorkIssue_Details_JWH_FK",
                table: "JobWorkIssue_Details",
                column: "JWH_FK");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobWorkIssue_Details");

           
            migrationBuilder.DropTable(
                name: "JobWorkIssue_Header");

            migrationBuilder.AlterColumn<string>(
                name: "StateCode",
                table: "States",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 3);

            migrationBuilder.AlterColumn<int>(
                name: "QTY_UOM",
                table: "PODetail_Master",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CountryCode",
                table: "Countries",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 3);

            migrationBuilder.AlterColumn<string>(
                name: "CityCode",
                table: "Cities",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 3,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "PODetailModel",
                columns: table => new
                {
                    POD_PK = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    APPROVED_DATE = table.Column<DateTime>(nullable: true),
                    APPROVED_UID = table.Column<string>(nullable: true),
                    DELV_DATE = table.Column<DateTime>(nullable: false),
                    DISC_PER = table.Column<decimal>(nullable: false),
                    DISC_RATE = table.Column<decimal>(nullable: false),
                    INS_DATE = table.Column<DateTime>(nullable: true),
                    INS_UID = table.Column<string>(nullable: true),
                    ITEM_CODE = table.Column<int>(nullable: false),
                    NET_RATE = table.Column<decimal>(nullable: false),
                    POD_PK_STATUS = table.Column<string>(maxLength: 3, nullable: true),
                    POH_FK = table.Column<int>(nullable: false),
                    QTY = table.Column<decimal>(nullable: false),
                    QTY_UOM = table.Column<int>(nullable: false),
                    RATE = table.Column<decimal>(nullable: false),
                    RATE_UOM = table.Column<int>(nullable: false),
                    REMARKS = table.Column<string>(maxLength: 100, nullable: true),
                    UDT_DATE = table.Column<DateTime>(nullable: true),
                    UDT_UID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PODetailModel", x => x.POD_PK);
                });

            migrationBuilder.CreateTable(
                name: "POHeaderModel",
                columns: table => new
                {
                    POH_PK = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ACC_CODE = table.Column<int>(nullable: false),
                    COMP_CODE = table.Column<int>(nullable: false),
                    INS_DATE = table.Column<DateTime>(nullable: true),
                    INS_UID = table.Column<string>(nullable: true),
                    ORDER_DATE = table.Column<DateTime>(nullable: false),
                    ORDER_FINYEAR = table.Column<string>(nullable: true),
                    ORDER_NO = table.Column<int>(nullable: false),
                    REMARKS = table.Column<string>(maxLength: 100, nullable: true),
                    UDT_DATE = table.Column<DateTime>(nullable: true),
                    UDT_UID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_POHeaderModel", x => x.POH_PK);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PODetailModel_POH_FK",
                table: "PODetailModel",
                column: "POH_FK");
        }
    }
}
