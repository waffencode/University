using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace University.Migrations
{
    /// <inheritdoc />
    public partial class FieldOfStudy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PlannedClass",
                table: "PlannedClass");

            migrationBuilder.DropIndex(
                name: "IX_PlannedClass_WorkProgramId",
                table: "PlannedClass");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlannedClass",
                table: "PlannedClass",
                columns: new[] { "WorkProgramId", "Id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PlannedClass",
                table: "PlannedClass");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlannedClass",
                table: "PlannedClass",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_PlannedClass_WorkProgramId",
                table: "PlannedClass",
                column: "WorkProgramId");
        }
    }
}
