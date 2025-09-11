using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS.Infractructure.Migrations
{
    /// <inheritdoc />
    public partial class connectionassignmentinactivity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Activities_ModuleActivityId",
                table: "Assignments");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModuleActivityId",
                table: "Assignments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Activities_ModuleActivityId",
                table: "Assignments",
                column: "ModuleActivityId",
                principalTable: "Activities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Activities_ModuleActivityId",
                table: "Assignments");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModuleActivityId",
                table: "Assignments",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Activities_ModuleActivityId",
                table: "Assignments",
                column: "ModuleActivityId",
                principalTable: "Activities",
                principalColumn: "Id");
        }
    }
}
