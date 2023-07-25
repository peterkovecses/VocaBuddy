using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VocaBuddy.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RenameNameToText : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "NativeWords",
                newName: "Text");

            migrationBuilder.RenameIndex(
                name: "IX_NativeWords_Name",
                table: "NativeWords",
                newName: "IX_NativeWords_Text");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "ForeignWords",
                newName: "Text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Text",
                table: "NativeWords",
                newName: "Name");

            migrationBuilder.RenameIndex(
                name: "IX_NativeWords_Text",
                table: "NativeWords",
                newName: "IX_NativeWords_Name");

            migrationBuilder.RenameColumn(
                name: "Text",
                table: "ForeignWords",
                newName: "Name");
        }
    }
}
