using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebERP.Data.Migrations
{
    public partial class POdetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "POHeaderModel",
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
                    table.PrimaryKey("PK_POHeaderModel", x => x.POH_PK);
                });

            migrationBuilder.CreateTable(
                name: "PODetailModel",
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
                    table.PrimaryKey("PK_PODetailModel", x => x.POD_PK);
                    table.ForeignKey(
                        name: "FK_PODetailModel_POHeaderModel_POH_FK",
                        column: x => x.POH_FK,
                        principalTable: "POHeaderModel",
                        principalColumn: "POH_PK",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PODetailModel_POH_FK",
                table: "PODetailModel",
                column: "POH_FK");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PODetailModel");

            migrationBuilder.DropTable(
                name: "POHeaderModel");
        }
    }
}
