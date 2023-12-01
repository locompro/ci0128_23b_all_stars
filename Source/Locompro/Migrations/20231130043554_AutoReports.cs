using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Locompro.Migrations
{
    public partial class AutoReports : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "AveragePrice",
                table: "Reports",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Confidence",
                table: "Reports",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Reports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "MaximumPrice",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinimumPrice",
                table: "Reports",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AveragePrice",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "Confidence",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "MaximumPrice",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "MinimumPrice",
                table: "Reports");
        }
    }
}
