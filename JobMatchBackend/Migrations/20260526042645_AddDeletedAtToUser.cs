using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobMatchBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddDeletedAtToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "users",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "users");
        }
    }
}
