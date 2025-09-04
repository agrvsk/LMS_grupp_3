using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS.Infractructure.Migrations
{
    /// <inheritdoc />
    public partial class refacturingasssubdocuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_ApplicationUser_ApplicationUserId",
                table: "Submissions");

            migrationBuilder.DropIndex(
                name: "IX_Submissions_ApplicationUserId",
                table: "Submissions");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Submissions");

            migrationBuilder.CreateTable(
                name: "ApplicationUserSubmission",
                columns: table => new
                {
                    SubmissionsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubmittersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserSubmission", x => new { x.SubmissionsId, x.SubmittersId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserSubmission_ApplicationUser_SubmittersId",
                        column: x => x.SubmittersId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserSubmission_Submissions_SubmissionsId",
                        column: x => x.SubmissionsId,
                        principalTable: "Submissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_AssignmentId",
                table: "Submissions",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserSubmission_SubmittersId",
                table: "ApplicationUserSubmission",
                column: "SubmittersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_Assignments_AssignmentId",
                table: "Submissions",
                column: "AssignmentId",
                principalTable: "Assignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_Assignments_AssignmentId",
                table: "Submissions");

            migrationBuilder.DropTable(
                name: "ApplicationUserSubmission");

            migrationBuilder.DropIndex(
                name: "IX_Submissions_AssignmentId",
                table: "Submissions");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Submissions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_ApplicationUserId",
                table: "Submissions",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_ApplicationUser_ApplicationUserId",
                table: "Submissions",
                column: "ApplicationUserId",
                principalTable: "ApplicationUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
