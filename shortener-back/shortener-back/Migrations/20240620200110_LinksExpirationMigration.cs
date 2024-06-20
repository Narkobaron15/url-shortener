using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace shortener_back.Migrations
{
    /// <inheritdoc />
    public partial class LinksExpirationMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Clicks",
                table: "Shortens",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiresAt",
                table: "Shortens",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Clicks",
                table: "Shortens");

            migrationBuilder.DropColumn(
                name: "ExpiresAt",
                table: "Shortens");
        }
    }
}
