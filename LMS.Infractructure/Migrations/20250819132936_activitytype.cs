using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Companies.Infractructure.Migrations
{
    /// <inheritdoc />
    public partial class activitytype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Activities",
                newName: "TypeId");

            migrationBuilder.CreateTable(
                name: "ActivityType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_TypeId",
                table: "Activities",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_ActivityType_TypeId",
                table: "Activities",
                column: "TypeId",
                principalTable: "ActivityType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_ActivityType_TypeId",
                table: "Activities");

            migrationBuilder.DropTable(
                name: "ActivityType");

            migrationBuilder.DropIndex(
                name: "IX_Activities_TypeId",
                table: "Activities");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "Activities",
                newName: "Type");
        }
    }
}
