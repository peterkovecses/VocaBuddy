using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VocaBuddy.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RecordMistakes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MistakeCount",
                table: "NativeWords",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddCheckConstraint(
                name: "CK_NativeWord_MistakeCount_NonNegative",
                table: "NativeWords",
                sql: "[MistakeCount] >= 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_NativeWord_MistakeCount_NonNegative",
                table: "NativeWords");

            migrationBuilder.DropColumn(
                name: "MistakeCount",
                table: "NativeWords");
        }
    }
}
