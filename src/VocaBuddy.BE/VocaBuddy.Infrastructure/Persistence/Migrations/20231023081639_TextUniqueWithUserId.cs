using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VocaBuddy.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class TextUniqueWithUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_NativeWords_Text",
                table: "NativeWords");

            migrationBuilder.CreateIndex(
                name: "IX_NativeWords_UserId_Text",
                table: "NativeWords",
                columns: new[] { "UserId", "Text" },
                unique: true)
                .Annotation("SqlServer:Clustered", false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_NativeWords_UserId_Text",
                table: "NativeWords");

            migrationBuilder.CreateIndex(
                name: "IX_NativeWords_Text",
                table: "NativeWords",
                column: "Text",
                unique: true)
                .Annotation("SqlServer:Clustered", false);
        }
    }
}
