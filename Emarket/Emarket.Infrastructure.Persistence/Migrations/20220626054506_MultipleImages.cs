using Microsoft.EntityFrameworkCore.Migrations;

namespace Emarket.Infrastructure.Persistence.Migrations
{
    public partial class MultipleImages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl2",
                table: "Advertisements",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl3",
                table: "Advertisements",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl4",
                table: "Advertisements",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl2",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "ImageUrl3",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "ImageUrl4",
                table: "Advertisements");
        }
    }
}
