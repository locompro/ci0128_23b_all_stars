using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Locompro.Migrations
{
    public partial class Store : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Province",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    CountryName = table.Column<string>(type: "nvarchar(60)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Province", x => x.Name);
                    table.ForeignKey(
                        name: "FK_Province_Country_CountryName",
                        column: x => x.CountryName,
                        principalTable: "Country",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Canton",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    ProvinceName = table.Column<string>(type: "nvarchar(60)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Canton", x => x.Name);
                    table.ForeignKey(
                        name: "FK_Canton_Province_ProvinceName",
                        column: x => x.ProvinceName,
                        principalTable: "Province",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Store",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    CantonName = table.Column<string>(type: "nvarchar(60)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: false),
                    Telephone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Store", x => x.Name);
                    table.ForeignKey(
                        name: "FK_Store_Canton_CantonName",
                        column: x => x.CantonName,
                        principalTable: "Canton",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Canton_ProvinceName",
                table: "Canton",
                column: "ProvinceName");

            migrationBuilder.CreateIndex(
                name: "IX_Province_CountryName",
                table: "Province",
                column: "CountryName");

            migrationBuilder.CreateIndex(
                name: "IX_Store_CantonName",
                table: "Store",
                column: "CantonName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Store");

            migrationBuilder.DropTable(
                name: "Canton");

            migrationBuilder.DropTable(
                name: "Province");

            migrationBuilder.DropTable(
                name: "Country");
        }
    }
}
