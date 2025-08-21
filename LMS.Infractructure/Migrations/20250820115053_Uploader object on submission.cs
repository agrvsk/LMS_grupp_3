using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS.Infractructure.Migrations
{
    /// <inheritdoc />
    public partial class Uploaderobjectonsubmission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Submissions_DocumentId",
                table: "Submissions",
                column: "DocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_Documents_DocumentId",
                table: "Submissions",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_Documents_DocumentId",
                table: "Submissions");

            migrationBuilder.DropIndex(
                name: "IX_Submissions_DocumentId",
                table: "Submissions");
        }
    }
}
