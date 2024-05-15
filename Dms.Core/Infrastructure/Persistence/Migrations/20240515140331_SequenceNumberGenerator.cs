using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dms.Core.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SequenceNumberGenerator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "DmsDocumentNumberSequence",
                startValue: 0L,
                maxValue: 2147483647L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropSequence(
                name: "DmsDocumentNumberSequence");
        }
    }
}
