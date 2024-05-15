using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dms.Core.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DocumentSequenceNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DocumentNumber",
                table: "FilesData",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentNumber",
                table: "FilesData");
        }
    }
}
