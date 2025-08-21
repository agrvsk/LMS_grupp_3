using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS.Infractructure.Migrations
{
    /// <inheritdoc />
    public partial class ApplicationUserguidtostringfix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_ApplicationUser_ApplicationUserId1",
                table: "Submissions");

            migrationBuilder.DropIndex(
                name: "IX_Submissions_ApplicationUserId1",
                table: "Submissions");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId1",
                table: "Submissions");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Submissions",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_ApplicationUser_ApplicationUserId",
                table: "Submissions");

            migrationBuilder.DropIndex(
                name: "IX_Submissions_ApplicationUserId",
                table: "Submissions");

            migrationBuilder.AlterColumn<Guid>(
                name: "ApplicationUserId",
                table: "Submissions",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId1",
                table: "Submissions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_ApplicationUserId1",
                table: "Submissions",
                column: "ApplicationUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_ApplicationUser_ApplicationUserId1",
                table: "Submissions",
                column: "ApplicationUserId1",
                principalTable: "ApplicationUser",
                principalColumn: "Id");
        }
    }
}
