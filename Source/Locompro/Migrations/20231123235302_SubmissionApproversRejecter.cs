using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Locompro.Migrations
{
    public partial class SubmissionApproversRejecter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SubmissionUser",
                columns: table => new
                {
                    ApproversId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApprovedSubmissionsUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApprovedSubmissionsEntryTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubmissionUser", x => new { x.ApproversId, x.ApprovedSubmissionsUserId, x.ApprovedSubmissionsEntryTime });
                    table.ForeignKey(
                        name: "FK_SubmissionUser_AspNetUsers_ApproversId",
                        column: x => x.ApproversId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubmissionUser_Submissions_ApprovedSubmissionsUserId_ApprovedSubmissionsEntryTime",
                        columns: x => new { x.ApprovedSubmissionsUserId, x.ApprovedSubmissionsEntryTime },
                        principalTable: "Submissions",
                        principalColumns: new[] { "UserId", "EntryTime" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubmissionUser1",
                columns: table => new
                {
                    RejectersId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RejectedSubmissionsUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RejectedSubmissionsEntryTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubmissionUser1", x => new { x.RejectersId, x.RejectedSubmissionsUserId, x.RejectedSubmissionsEntryTime });
                    table.ForeignKey(
                        name: "FK_SubmissionUser1_AspNetUsers_RejectersId",
                        column: x => x.RejectersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubmissionUser1_Submissions_RejectedSubmissionsUserId_RejectedSubmissionsEntryTime",
                        columns: x => new { x.RejectedSubmissionsUserId, x.RejectedSubmissionsEntryTime },
                        principalTable: "Submissions",
                        principalColumns: new[] { "UserId", "EntryTime" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubmissionUser_ApprovedSubmissionsUserId_ApprovedSubmissionsEntryTime",
                table: "SubmissionUser",
                columns: new[] { "ApprovedSubmissionsUserId", "ApprovedSubmissionsEntryTime" });

            migrationBuilder.CreateIndex(
                name: "IX_SubmissionUser1_RejectedSubmissionsUserId_RejectedSubmissionsEntryTime",
                table: "SubmissionUser1",
                columns: new[] { "RejectedSubmissionsUserId", "RejectedSubmissionsEntryTime" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubmissionUser");

            migrationBuilder.DropTable(
                name: "SubmissionUser1");
        }
    }
}
