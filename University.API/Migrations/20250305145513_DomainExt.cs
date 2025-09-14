using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace University.Migrations
{
    /// <inheritdoc />
    public partial class DomainExt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudyGroup_Messages_MessageId",
                table: "StudyGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_StudyGroup_StudyGroupId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudyGroup",
                table: "StudyGroup");

            migrationBuilder.RenameTable(
                name: "StudyGroup",
                newName: "StudyGroups");

            migrationBuilder.RenameIndex(
                name: "IX_StudyGroup_MessageId",
                table: "StudyGroups",
                newName: "IX_StudyGroups_MessageId");

            migrationBuilder.AddColumn<Guid>(
                name: "ScheduleClassId",
                table: "StudyGroups",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudyGroups",
                table: "StudyGroups",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ClassTimeSlots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassTimeSlots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleClasses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    TeacherId = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    TimeSlotId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClassroomId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClassType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleClasses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduleClasses_ClassTimeSlots_TimeSlotId",
                        column: x => x.TimeSlotId,
                        principalTable: "ClassTimeSlots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScheduleClasses_Classrooms_ClassroomId",
                        column: x => x.ClassroomId,
                        principalTable: "Classrooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScheduleClasses_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScheduleClasses_Users_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubjectWorkPrograms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectWorkPrograms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubjectWorkPrograms_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlannedClass",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Theme = table.Column<string>(type: "text", nullable: false),
                    Hours = table.Column<int>(type: "integer", nullable: false),
                    ClassType = table.Column<int>(type: "integer", nullable: false),
                    WorkProgramId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlannedClass", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlannedClass_SubjectWorkPrograms_WorkProgramId",
                        column: x => x.WorkProgramId,
                        principalTable: "SubjectWorkPrograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudyGroups_ScheduleClassId",
                table: "StudyGroups",
                column: "ScheduleClassId");

            migrationBuilder.CreateIndex(
                name: "IX_PlannedClass_WorkProgramId",
                table: "PlannedClass",
                column: "WorkProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleClasses_ClassroomId",
                table: "ScheduleClasses",
                column: "ClassroomId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleClasses_SubjectId",
                table: "ScheduleClasses",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleClasses_TeacherId",
                table: "ScheduleClasses",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleClasses_TimeSlotId",
                table: "ScheduleClasses",
                column: "TimeSlotId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectWorkPrograms_SubjectId",
                table: "SubjectWorkPrograms",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudyGroups_Messages_MessageId",
                table: "StudyGroups",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudyGroups_ScheduleClasses_ScheduleClassId",
                table: "StudyGroups",
                column: "ScheduleClassId",
                principalTable: "ScheduleClasses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_StudyGroups_StudyGroupId",
                table: "Users",
                column: "StudyGroupId",
                principalTable: "StudyGroups",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudyGroups_Messages_MessageId",
                table: "StudyGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_StudyGroups_ScheduleClasses_ScheduleClassId",
                table: "StudyGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_StudyGroups_StudyGroupId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "PlannedClass");

            migrationBuilder.DropTable(
                name: "ScheduleClasses");

            migrationBuilder.DropTable(
                name: "SubjectWorkPrograms");

            migrationBuilder.DropTable(
                name: "ClassTimeSlots");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudyGroups",
                table: "StudyGroups");

            migrationBuilder.DropIndex(
                name: "IX_StudyGroups_ScheduleClassId",
                table: "StudyGroups");

            migrationBuilder.DropColumn(
                name: "ScheduleClassId",
                table: "StudyGroups");

            migrationBuilder.RenameTable(
                name: "StudyGroups",
                newName: "StudyGroup");

            migrationBuilder.RenameIndex(
                name: "IX_StudyGroups_MessageId",
                table: "StudyGroup",
                newName: "IX_StudyGroup_MessageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudyGroup",
                table: "StudyGroup",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudyGroup_Messages_MessageId",
                table: "StudyGroup",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_StudyGroup_StudyGroupId",
                table: "Users",
                column: "StudyGroupId",
                principalTable: "StudyGroup",
                principalColumn: "Id");
        }
    }
}
