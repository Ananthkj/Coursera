using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coursera.Migrations
{
    /// <inheritdoc />
    public partial class CoursesTableUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "courses");

            migrationBuilder.AddColumn<int>(
                name: "ApprovalStatus",
                table: "courses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovalStatus",
                table: "courses");

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "courses",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
