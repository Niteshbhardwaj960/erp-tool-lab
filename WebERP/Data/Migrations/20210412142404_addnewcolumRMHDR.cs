using Microsoft.EntityFrameworkCore.Migrations;

namespace WebERP.Data.Migrations
{
    public partial class addnewcolumRMHDR : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {    
            migrationBuilder.AddColumn<string>(
                name: "CUTTING_ORDER_NO",
                table: "RM_HDR",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CUTTING_ORDER_NO",
                table: "RM_HDR");            
        }
    }
}
