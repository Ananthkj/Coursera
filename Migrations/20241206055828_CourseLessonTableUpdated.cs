using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coursera.Migrations
{
    /// <inheritdoc />
    public partial class CourseLessonTableUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_courseLessons_courseSections_SectionId",
                table: "courseLessons");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "courseLessons");

            migrationBuilder.AlterColumn<int>(
                name: "SectionId",
                table: "courseLessons",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_courseLessons_courseSections_SectionId",
                table: "courseLessons",
                column: "SectionId",
                principalTable: "courseSections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_courseLessons_courseSections_SectionId",
                table: "courseLessons");

            migrationBuilder.AlterColumn<int>(
                name: "SectionId",
                table: "courseLessons",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "courseLessons",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_courseLessons_courseSections_SectionId",
                table: "courseLessons",
                column: "SectionId",
                principalTable: "courseSections",
                principalColumn: "Id");
        }
    }
}
