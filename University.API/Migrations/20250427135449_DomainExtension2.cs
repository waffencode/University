using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace University.Migrations
{
    /// <inheritdoc />
    public partial class DomainExtension2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentDetails_ScheduleClassDetails_ScheduleClassDetailsId",
                table: "StudentDetails");

            migrationBuilder.DropTable(
                name: "ScheduleClassDetails");

            migrationBuilder.RenameColumn(
                name: "ScheduleClassDetailsId",
                table: "StudentDetails",
                newName: "ScheduleClassDetailsScheduleClassId");

            migrationBuilder.AddColumn<Guid>(
                name: "Details_Id",
                table: "ScheduleClasses",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string[]>(
                name: "Attachments",
                table: "Messages",
                type: "text[]",
                nullable: false,
                defaultValue: new string[0]);

            migrationBuilder.AddColumn<Guid>(
                name: "RelatedClassId",
                table: "Messages",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Classrooms",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Classrooms",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_RelatedClassId",
                table: "Messages",
                column: "RelatedClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_ScheduleClasses_RelatedClassId",
                table: "Messages",
                column: "RelatedClassId",
                principalTable: "ScheduleClasses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentDetails_ScheduleClasses_ScheduleClassDetailsSchedule~",
                table: "StudentDetails",
                column: "ScheduleClassDetailsScheduleClassId",
                principalTable: "ScheduleClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_ScheduleClasses_RelatedClassId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentDetails_ScheduleClasses_ScheduleClassDetailsSchedule~",
                table: "StudentDetails");

            migrationBuilder.DropIndex(
                name: "IX_Messages_RelatedClassId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "Details_Id",
                table: "ScheduleClasses");

            migrationBuilder.DropColumn(
                name: "Attachments",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "RelatedClassId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Classrooms");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Classrooms");

            migrationBuilder.RenameColumn(
                name: "ScheduleClassDetailsScheduleClassId",
                table: "StudentDetails",
                newName: "ScheduleClassDetailsId");

            migrationBuilder.CreateTable(
                name: "ScheduleClassDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClassId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleClassDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduleClassDetails_ScheduleClasses_ClassId",
                        column: x => x.ClassId,
                        principalTable: "ScheduleClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleClassDetails_ClassId",
                table: "ScheduleClassDetails",
                column: "ClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentDetails_ScheduleClassDetails_ScheduleClassDetailsId",
                table: "StudentDetails",
                column: "ScheduleClassDetailsId",
                principalTable: "ScheduleClassDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
