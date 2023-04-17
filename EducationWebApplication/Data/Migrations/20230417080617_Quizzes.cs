using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducationWebApplication.Data.Migrations
{
    public partial class Quizzes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quiz_Course_CourseId",
                table: "Quiz");

            migrationBuilder.DropIndex(
                name: "IX_Quiz_CourseId",
                table: "Quiz");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Quiz");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "Quiz",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Quiz_CourseId",
                table: "Quiz",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Quiz_Course_CourseId",
                table: "Quiz",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
