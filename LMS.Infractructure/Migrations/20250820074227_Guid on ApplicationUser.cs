using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS.Infractructure.Migrations
{
    /// <inheritdoc />
    public partial class GuidonApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUser_Courses_CourseId1",
                table: "ApplicationUser");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationUser_CourseId1",
                table: "ApplicationUser");

            migrationBuilder.DropColumn(
                name: "CourseId1",
                table: "ApplicationUser");

            migrationBuilder.AlterColumn<Guid>(
                name: "CourseId",
                table: "ApplicationUser",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_CourseId",
                table: "ApplicationUser",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUser_Courses_CourseId",
                table: "ApplicationUser",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUser_Courses_CourseId",
                table: "ApplicationUser");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationUser_CourseId",
                table: "ApplicationUser");

            migrationBuilder.AlterColumn<string>(
                name: "CourseId",
                table: "ApplicationUser",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CourseId1",
                table: "ApplicationUser",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_CourseId1",
                table: "ApplicationUser",
                column: "CourseId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUser_Courses_CourseId1",
                table: "ApplicationUser",
                column: "CourseId1",
                principalTable: "Courses",
                principalColumn: "Id");
        }
    }
}
