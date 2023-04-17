using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducationWebApplication.Data.Migrations
{
    public partial class AddCourseProfessor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CourseProf",
                table: "Course",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourseProf",
                table: "Course");
        }
    }
}
