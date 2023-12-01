using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Locompro.Migrations
{
    public partial class CorrectCascadeBehaviorForSubmissionAndUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_AspNetUsers_UserId",
                table: "Submissions");

            migrationBuilder.CreateTable(
                name: "SubmissionApprover",
                columns: table => new
                {
                    ApproverId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SubmissionUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SubmissionEntryTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubmissionApprover", x => new { x.ApproverId, x.SubmissionUserId, x.SubmissionEntryTime });
                    table.ForeignKey(
                        name: "FK_SubmissionApprover_AspNetUsers_ApproverId",
                        column: x => x.ApproverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubmissionApprover_Submissions_SubmissionUserId_SubmissionEntryTime",
                        columns: x => new { x.SubmissionUserId, x.SubmissionEntryTime },
                        principalTable: "Submissions",
                        principalColumns: new[] { "UserId", "EntryTime" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubmissionRejecter",
                columns: table => new
                {
                    RejecterId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SubmissionUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SubmissionEntryTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubmissionRejecter", x => new { x.RejecterId, x.SubmissionUserId, x.SubmissionEntryTime });
                    table.ForeignKey(
                        name: "FK_SubmissionRejecter_AspNetUsers_RejecterId",
                        column: x => x.RejecterId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubmissionRejecter_Submissions_SubmissionUserId_SubmissionEntryTime",
                        columns: x => new { x.SubmissionUserId, x.SubmissionEntryTime },
                        principalTable: "Submissions",
                        principalColumns: new[] { "UserId", "EntryTime" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubmissionApprover_SubmissionUserId_SubmissionEntryTime",
                table: "SubmissionApprover",
                columns: new[] { "SubmissionUserId", "SubmissionEntryTime" });

            migrationBuilder.CreateIndex(
                name: "IX_SubmissionRejecter_SubmissionUserId_SubmissionEntryTime",
                table: "SubmissionRejecter",
                columns: new[] { "SubmissionUserId", "SubmissionEntryTime" });

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_AspNetUsers_UserId",
                table: "Submissions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_AspNetUsers_UserId",
                table: "Submissions");

            migrationBuilder.DropTable(
                name: "SubmissionApprover");

            migrationBuilder.DropTable(
                name: "SubmissionRejecter");

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_AspNetUsers_UserId",
                table: "Submissions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
