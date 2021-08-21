using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Data.Migrations
{
    public partial class CorrectDateOfBirthColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateOfBirf",
                table: "Users",
                newName: "DateOfBirth");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateOfBirth",
                table: "Users",
                newName: "DateOfBirf");
        }
    }
}
