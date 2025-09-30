using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DawamApp.Migrations
{
    /// <inheritdoc />
    public partial class SeedSampleRecords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "EmployeeStatuses",
                keyColumn: "Id",
                keyValue: 2,
                column: "StartDate",
                value: new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "EmployeeStatuses",
                keyColumn: "Id",
                keyValue: 2,
                column: "StartDate",
                value: new DateTime(2025, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
