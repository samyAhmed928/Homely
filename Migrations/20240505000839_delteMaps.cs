using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Homely_modified_api.Migrations
{
    public partial class delteMaps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MapLatitude",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "MapLongitude",
                table: "Properties");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "MapLatitude",
                table: "Properties",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MapLongitude",
                table: "Properties",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
