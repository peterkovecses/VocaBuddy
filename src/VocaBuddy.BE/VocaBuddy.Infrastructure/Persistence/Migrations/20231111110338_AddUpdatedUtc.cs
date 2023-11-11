using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VocaBuddy.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUpdatedUtc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedUtc",
                table: "NativeWords",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_NativeWords_UpdatedUtc",
                table: "NativeWords",
                column: "UpdatedUtc")
                .Annotation("SqlServer:Clustered", false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_NativeWords_UpdatedUtc",
                table: "NativeWords");

            migrationBuilder.DropColumn(
                name: "UpdatedUtc",
                table: "NativeWords");
        }
    }
}
