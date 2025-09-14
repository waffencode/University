using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace University.Migrations
{
    /// <inheritdoc />
    public partial class Classrooms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "StudyGroupId",
                table: "Users",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "StudyGroup",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Classrooms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Designation = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classrooms", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_StudyGroupId",
                table: "Users",
                column: "StudyGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_StudyGroup_StudyGroupId",
                table: "Users",
                column: "StudyGroupId",
                principalTable: "StudyGroup",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_StudyGroup_StudyGroupId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Classrooms");

            migrationBuilder.DropIndex(
                name: "IX_Users_StudyGroupId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "StudyGroupId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "StudyGroup");
        }
    }
}
