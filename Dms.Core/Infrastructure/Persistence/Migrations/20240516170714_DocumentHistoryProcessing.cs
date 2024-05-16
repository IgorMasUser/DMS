using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dms.Core.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DocumentHistoryProcessing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DocumentHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Event = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PerformedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentHistory_FilesData_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "FilesData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

                      migrationBuilder.CreateIndex(
                name: "IX_DocumentHistory_DocumentId",
                table: "DocumentHistory",
                column: "DocumentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentHistory");
        }
    }
}
