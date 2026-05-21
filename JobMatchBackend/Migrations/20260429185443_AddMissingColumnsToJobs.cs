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
            migrationBuilder.Sql(@"
                IF NOT EXISTS (
                    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_NAME = 'Jobs' AND COLUMN_NAME = 'Deliverables'
                )
                BEGIN
                    ALTER TABLE [Jobs] ADD [Deliverables] nvarchar(max) NULL;
                END
            ");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (
                    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_NAME = 'Jobs' AND COLUMN_NAME = 'EndDate'
                )
                BEGIN
                    ALTER TABLE [Jobs] ADD [EndDate] date NULL;
                END
            ");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (
                    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_NAME = 'Jobs' AND COLUMN_NAME = 'EndTime'
                )
                BEGIN
                    ALTER TABLE [Jobs] ADD [EndTime] time NOT NULL DEFAULT '00:00:00';
                END
            ");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (
                    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_NAME = 'Jobs' AND COLUMN_NAME = 'Payment'
                )
                BEGIN
                    ALTER TABLE [Jobs] ADD [Payment] decimal(18,2) NOT NULL DEFAULT 0;
                END
            ");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (
                    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_NAME = 'Jobs' AND COLUMN_NAME = 'PaymentType'
                )
                BEGIN
                    ALTER TABLE [Jobs] ADD [PaymentType] nvarchar(max) NOT NULL DEFAULT '';
                END
            ");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (
                    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_NAME = 'Jobs' AND COLUMN_NAME = 'StartDate'
                )
                BEGIN
                    ALTER TABLE [Jobs] ADD [StartDate] date NULL;
                END
            ");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (
                    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_NAME = 'Jobs' AND COLUMN_NAME = 'StartTime'
                )
                BEGIN
                    ALTER TABLE [Jobs] ADD [StartTime] time NOT NULL DEFAULT '00:00:00';
                END
            ");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (
                    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_NAME = 'Jobs' AND COLUMN_NAME = 'WorkDate'
                )
                BEGIN
                    ALTER TABLE [Jobs] ADD [WorkDate] date NOT NULL DEFAULT '0001-01-01';
                END
            ");
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
