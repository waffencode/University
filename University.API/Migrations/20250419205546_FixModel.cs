using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace University.Migrations
{
    /// <inheritdoc />
    public partial class FixModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudyGroups_FieldsOfStudy_FieldOfStudyId",
                table: "StudyGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_StudyGroups_Messages_MessageId",
                table: "StudyGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_StudyGroups_ScheduleClasses_ScheduleClassId",
                table: "StudyGroups");

            migrationBuilder.DropIndex(
                name: "IX_StudyGroups_MessageId",
                table: "StudyGroups");

            migrationBuilder.DropIndex(
                name: "IX_StudyGroups_ScheduleClassId",
                table: "StudyGroups");

            migrationBuilder.DropColumn(
                name: "MessageId",
                table: "StudyGroups");

            migrationBuilder.DropColumn(
                name: "ScheduleClassId",
                table: "StudyGroups");

            migrationBuilder.AlterColumn<Guid>(
                name: "FieldOfStudyId",
                table: "StudyGroups",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "MessageStudyGroup",
                columns: table => new
                {
                    MessageId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReceiversStudyGroupsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageStudyGroup", x => new { x.MessageId, x.ReceiversStudyGroupsId });
                    table.ForeignKey(
                        name: "FK_MessageStudyGroup_Messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MessageStudyGroup_StudyGroups_ReceiversStudyGroupsId",
                        column: x => x.ReceiversStudyGroupsId,
                        principalTable: "StudyGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleClassStudyGroup",
                columns: table => new
                {
                    GroupsId = table.Column<Guid>(type: "uuid", nullable: false),
                    ScheduleClassId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleClassStudyGroup", x => new { x.GroupsId, x.ScheduleClassId });
                    table.ForeignKey(
                        name: "FK_ScheduleClassStudyGroup_ScheduleClasses_ScheduleClassId",
                        column: x => x.ScheduleClassId,
                        principalTable: "ScheduleClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScheduleClassStudyGroup_StudyGroups_GroupsId",
                        column: x => x.GroupsId,
                        principalTable: "StudyGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MessageStudyGroup_ReceiversStudyGroupsId",
                table: "MessageStudyGroup",
                column: "ReceiversStudyGroupsId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleClassStudyGroup_ScheduleClassId",
                table: "ScheduleClassStudyGroup",
                column: "ScheduleClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudyGroups_FieldsOfStudy_FieldOfStudyId",
                table: "StudyGroups",
                column: "FieldOfStudyId",
                principalTable: "FieldsOfStudy",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudyGroups_FieldsOfStudy_FieldOfStudyId",
                table: "StudyGroups");

            migrationBuilder.DropTable(
                name: "MessageStudyGroup");

            migrationBuilder.DropTable(
                name: "ScheduleClassStudyGroup");

            migrationBuilder.AlterColumn<Guid>(
                name: "FieldOfStudyId",
                table: "StudyGroups",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "MessageId",
                table: "StudyGroups",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ScheduleClassId",
                table: "StudyGroups",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudyGroups_MessageId",
                table: "StudyGroups",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_StudyGroups_ScheduleClassId",
                table: "StudyGroups",
                column: "ScheduleClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudyGroups_FieldsOfStudy_FieldOfStudyId",
                table: "StudyGroups",
                column: "FieldOfStudyId",
                principalTable: "FieldsOfStudy",
                principalColumn: "Id");

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
        }
    }
}
