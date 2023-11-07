using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Locompro.Migrations
{
    public partial class Pictures_Reports_Profile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GetPicturesResult",
                columns: table => new
                {
                    PictureTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PictureData = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "GetQualifiedUserIDsResult",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GetQualifiedUserIDsResult", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    SubmissionUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SubmissionEntryTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => new { x.SubmissionUserId, x.SubmissionEntryTime, x.UserId });
                    table.ForeignKey(
                        name: "FK_Reports_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reports_Submissions_SubmissionUserId_SubmissionEntryTime",
                        columns: x => new { x.SubmissionUserId, x.SubmissionEntryTime },
                        principalTable: "Submissions",
                        principalColumns: new[] { "UserId", "EntryTime" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reports_UserId",
                table: "Reports",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GetPicturesResult");

            migrationBuilder.DropTable(
                name: "GetQualifiedUserIDsResult");

            migrationBuilder.DropTable(
                name: "Reports");
        }
    }
}
