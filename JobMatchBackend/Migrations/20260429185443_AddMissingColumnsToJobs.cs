using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobMatchBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingColumnsToJobs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Deliverables",
                table: "Jobs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "EndDate",
                table: "Jobs",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "EndTime",
                table: "Jobs",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<decimal>(
                name: "Payment",
                table: "Jobs",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "PaymentType",
                table: "Jobs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateOnly>(
                name: "StartDate",
                table: "Jobs",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "StartTime",
                table: "Jobs",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<DateOnly>(
                name: "WorkDate",
                table: "Jobs",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deliverables",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "Payment",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "PaymentType",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "WorkDate",
                table: "Jobs");
        }
    }
}
