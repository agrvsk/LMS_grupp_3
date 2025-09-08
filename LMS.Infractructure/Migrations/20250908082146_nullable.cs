using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS.Infractructure.Migrations
{
    /// <inheritdoc />
    public partial class nullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_ApplicationUser_UploaderId",
                table: "Documents");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_ApplicationUser_UploaderId",
                table: "Documents",
                column: "UploaderId",
                principalTable: "ApplicationUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_ApplicationUser_UploaderId",
                table: "Documents");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_ApplicationUser_UploaderId",
                table: "Documents",
                column: "UploaderId",
                principalTable: "ApplicationUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
