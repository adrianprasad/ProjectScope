using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectScope.Entity.Migrations
{
    /// <inheritdoc />
    public partial class Firs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "Students",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Students");
        }
    }
}
