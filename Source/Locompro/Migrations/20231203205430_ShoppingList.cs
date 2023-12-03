﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace Locompro.Migrations
{
    public partial class ShoppingList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_AspNetUsers_UserId",
                table: "Submissions");

            migrationBuilder.DropTable(
                name: "SubmissionUser");

            migrationBuilder.DropTable(
                name: "SubmissionUser1");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Stores");

            migrationBuilder.AddColumn<Point>(
                name: "Location",
                table: "Stores",
                type: "geography",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MostReportedUsersResult",
                columns: table => new
                {
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReportedSubmissionCount = table.Column<int>(type: "int", nullable: false),
                    TotalUserSubmissions = table.Column<int>(type: "int", nullable: false),
                    UserRating = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "ShoppingList",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingList", x => new { x.ProductId, x.UserId });
                    table.ForeignKey(
                        name: "FK_ShoppingList_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShoppingList_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "IX_ShoppingList_UserId",
                table: "ShoppingList",
                column: "UserId");

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
                name: "MostReportedUsersResult");

            migrationBuilder.DropTable(
                name: "ShoppingList");

            migrationBuilder.DropTable(
                name: "SubmissionApprover");

            migrationBuilder.DropTable(
                name: "SubmissionRejecter");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Stores");

            migrationBuilder.AddColumn<decimal>(
                name: "Latitude",
                table: "Stores",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Longitude",
                table: "Stores",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

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
