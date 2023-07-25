using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VocaBuddy.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NativeWords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    UserId = table.Column<string>(type: "varchar(36)", unicode: false, maxLength: 36, nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NativeWords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ForeignWords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    NativeWordId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForeignWords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ForeignWords_NativeWords_NativeWordId",
                        column: x => x.NativeWordId,
                        principalTable: "NativeWords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ForeignWords_NativeWordId",
                table: "ForeignWords",
                column: "NativeWordId");

            migrationBuilder.CreateIndex(
                name: "IX_NativeWords_CreatedUtc",
                table: "NativeWords",
                column: "CreatedUtc")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.CreateIndex(
                name: "IX_NativeWords_Name",
                table: "NativeWords",
                column: "Name",
                unique: true)
                .Annotation("SqlServer:Clustered", false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ForeignWords");

            migrationBuilder.DropTable(
                name: "NativeWords");
        }
    }
}
