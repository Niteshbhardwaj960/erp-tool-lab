using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebERP.Data.Migrations
{
    public partial class Locationnew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Cities_States_StateId",
            //    table: "Cities");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_States_Countries_CountryId",
            //    table: "States");

            //migrationBuilder.DropIndex(
            //    name: "IX_States_CountryId",
            //    table: "States");

            //migrationBuilder.DropIndex(
            //    name: "IX_Cities_StateId",
            //    table: "Cities");

            //migrationBuilder.RenameColumn(
            //    name: "StateCode",
            //    table: "Cities",
            //    newName: "CityCode");

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "Upd_Date",
            //    table: "States",
            //    nullable: true,
            //    oldClrType: typeof(DateTime));

            //migrationBuilder.AlterColumn<string>(
            //    name: "StateCode",
            //    table: "States",
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<string>(
            //    name: "Name",
            //    table: "States",
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "Ins_Date",
            //    table: "States",
            //    nullable: true,
            //    oldClrType: typeof(DateTime));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "Upd_Date",
            //    table: "Countries",
            //    nullable: true,
            //    oldClrType: typeof(DateTime));

            //migrationBuilder.AlterColumn<string>(
            //    name: "Name",
            //    table: "Countries",
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "Ins_Date",
            //    table: "Countries",
            //    nullable: true,
            //    oldClrType: typeof(DateTime));

            //migrationBuilder.AlterColumn<string>(
            //    name: "CountryCode",
            //    table: "Countries",
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "Upd_Date",
            //    table: "Cities",
            //    nullable: true,
            //    oldClrType: typeof(DateTime));

            //migrationBuilder.AlterColumn<string>(
            //    name: "Name",
            //    table: "Cities",
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "Ins_Date",
            //    table: "Cities",
            //    nullable: true,
            //    oldClrType: typeof(DateTime));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.RenameColumn(
            //    name: "CityCode",
            //    table: "Cities",
            //    newName: "StateCode");

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "Upd_Date",
            //    table: "States",
            //    nullable: false,
            //    oldClrType: typeof(DateTime),
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<string>(
            //    name: "StateCode",
            //    table: "States",
            //    nullable: true,
            //    oldClrType: typeof(string));

            //migrationBuilder.AlterColumn<string>(
            //    name: "Name",
            //    table: "States",
            //    nullable: true,
            //    oldClrType: typeof(string));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "Ins_Date",
            //    table: "States",
            //    nullable: false,
            //    oldClrType: typeof(DateTime),
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "Upd_Date",
            //    table: "Countries",
            //    nullable: false,
            //    oldClrType: typeof(DateTime),
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<string>(
            //    name: "Name",
            //    table: "Countries",
            //    nullable: true,
            //    oldClrType: typeof(string));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "Ins_Date",
            //    table: "Countries",
            //    nullable: false,
            //    oldClrType: typeof(DateTime),
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<string>(
            //    name: "CountryCode",
            //    table: "Countries",
            //    nullable: true,
            //    oldClrType: typeof(string));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "Upd_Date",
            //    table: "Cities",
            //    nullable: false,
            //    oldClrType: typeof(DateTime),
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<string>(
            //    name: "Name",
            //    table: "Cities",
            //    nullable: true,
            //    oldClrType: typeof(string));

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "Ins_Date",
            //    table: "Cities",
            //    nullable: false,
            //    oldClrType: typeof(DateTime),
            //    oldNullable: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_States_CountryId",
            //    table: "States",
            //    column: "CountryId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Cities_StateId",
            //    table: "Cities",
            //    column: "StateId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Cities_States_StateId",
            //    table: "Cities",
            //    column: "StateId",
            //    principalTable: "States",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_States_Countries_CountryId",
            //    table: "States",
            //    column: "CountryId",
            //    principalTable: "Countries",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }
    }
}
