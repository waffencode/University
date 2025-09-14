using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace University.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCircular : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlannedClass_SubjectWorkPrograms_WorkProgramId",
                table: "PlannedClass");

            migrationBuilder.RenameColumn(
                name: "WorkProgramId",
                table: "PlannedClass",
                newName: "SubjectWorkProgramId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlannedClass_SubjectWorkPrograms_SubjectWorkProgramId",
                table: "PlannedClass",
                column: "SubjectWorkProgramId",
                principalTable: "SubjectWorkPrograms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlannedClass_SubjectWorkPrograms_SubjectWorkProgramId",
                table: "PlannedClass");

            migrationBuilder.RenameColumn(
                name: "SubjectWorkProgramId",
                table: "PlannedClass",
                newName: "WorkProgramId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlannedClass_SubjectWorkPrograms_WorkProgramId",
                table: "PlannedClass",
                column: "WorkProgramId",
                principalTable: "SubjectWorkPrograms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
