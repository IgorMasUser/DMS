using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dms.Core.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FileAccounter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FileAccounterId",
                table: "FilesData",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FileAccounters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileAccounters", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FilesData_FileAccounterId",
                table: "FilesData",
                column: "FileAccounterId");

            migrationBuilder.AddForeignKey(
                name: "FK_FilesData_FileAccounters_FileAccounterId",
                table: "FilesData",
                column: "FileAccounterId",
                principalTable: "FileAccounters",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FilesData_FileAccounters_FileAccounterId",
                table: "FilesData");

            migrationBuilder.DropTable(
                name: "FileAccounters");

            migrationBuilder.DropIndex(
                name: "IX_FilesData_FileAccounterId",
                table: "FilesData");

            migrationBuilder.DropColumn(
                name: "FileAccounterId",
                table: "FilesData");
        }
    }
}
