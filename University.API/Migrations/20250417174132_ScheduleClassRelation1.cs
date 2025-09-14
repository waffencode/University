using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace University.Migrations
{
    /// <inheritdoc />
    public partial class ScheduleClassRelation1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleClasses_Subjects_SubjectId",
                table: "ScheduleClasses");

            migrationBuilder.RenameColumn(
                name: "SubjectId",
                table: "ScheduleClasses",
                newName: "SubjectWorkProgramId");

            migrationBuilder.RenameIndex(
                name: "IX_ScheduleClasses_SubjectId",
                table: "ScheduleClasses",
                newName: "IX_ScheduleClasses_SubjectWorkProgramId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleClasses_SubjectWorkPrograms_SubjectWorkProgramId",
                table: "ScheduleClasses",
                column: "SubjectWorkProgramId",
                principalTable: "SubjectWorkPrograms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleClasses_SubjectWorkPrograms_SubjectWorkProgramId",
                table: "ScheduleClasses");

            migrationBuilder.RenameColumn(
                name: "SubjectWorkProgramId",
                table: "ScheduleClasses",
                newName: "SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_ScheduleClasses_SubjectWorkProgramId",
                table: "ScheduleClasses",
                newName: "IX_ScheduleClasses_SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleClasses_Subjects_SubjectId",
                table: "ScheduleClasses",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
