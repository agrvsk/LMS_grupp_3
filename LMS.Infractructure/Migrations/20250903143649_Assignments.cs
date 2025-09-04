using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS.Infractructure.Migrations
{
    /// <inheritdoc />
    public partial class Assignments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AssignmentId",
                table: "Submissions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "AssignmentId",
                table: "Documents",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Assignments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DocumentIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModuleActivityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assignments_Activities_ModuleActivityId",
                        column: x => x.ModuleActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Documents_AssignmentId",
                table: "Documents",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_ModuleActivityId",
                table: "Assignments",
                column: "ModuleActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Assignments_AssignmentId",
                table: "Documents",
                column: "AssignmentId",
                principalTable: "Assignments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Assignments_AssignmentId",
                table: "Documents");

            migrationBuilder.DropTable(
                name: "Assignments");

            migrationBuilder.DropIndex(
                name: "IX_Documents_AssignmentId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "AssignmentId",
                table: "Submissions");

            migrationBuilder.DropColumn(
                name: "AssignmentId",
                table: "Documents");
        }
    }
}
