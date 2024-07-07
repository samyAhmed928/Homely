using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Homely_modified_api.Migrations
{
    public partial class rentperiod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Rent_period",
                table: "Properties",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rent_period",
                table: "Properties");
        }
    }
}
