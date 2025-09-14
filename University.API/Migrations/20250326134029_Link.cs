using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace University.Migrations
{
    /// <inheritdoc />
    public partial class Link : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FieldOfStudyId",
                table: "StudyGroups",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudyGroups_FieldOfStudyId",
                table: "StudyGroups",
                column: "FieldOfStudyId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudyGroups_FieldsOfStudy_FieldOfStudyId",
                table: "StudyGroups",
                column: "FieldOfStudyId",
                principalTable: "FieldsOfStudy",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudyGroups_FieldsOfStudy_FieldOfStudyId",
                table: "StudyGroups");

            migrationBuilder.DropIndex(
                name: "IX_StudyGroups_FieldOfStudyId",
                table: "StudyGroups");

            migrationBuilder.DropColumn(
                name: "FieldOfStudyId",
                table: "StudyGroups");
        }
    }
}
