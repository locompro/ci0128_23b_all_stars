using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Locompro.Migrations
{
    public partial class SubmissionUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_AspNetUsers_Username",
                table: "Submissions");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Submissions",
                newName: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_AspNetUsers_UserId",
                table: "Submissions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_AspNetUsers_UserId",
                table: "Submissions");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Submissions",
                newName: "Username");

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_AspNetUsers_Username",
                table: "Submissions",
                column: "Username",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
