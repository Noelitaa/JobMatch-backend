using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobMatchBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddContractsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF NOT EXISTS (
                    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_NAME = 'Jobs' AND COLUMN_NAME = 'Type'
                )
                BEGIN
                    ALTER TABLE [Jobs] ADD [Type] nvarchar(max) NULL;
                END
            ");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Applications')
                BEGIN
                    CREATE TABLE [Applications] (
                        [IdApplication] int IDENTITY(1,1) NOT NULL,
                        [IdJob] int NOT NULL,
                        [IdStudent] uniqueidentifier NOT NULL,
                        [Status] nvarchar(max) NULL,
                        [CreatedAt] datetime2 NOT NULL DEFAULT GETUTCDATE(),
                        [UpdatedAt] datetime2 NULL,
                        [JobIdJob] int NULL,
                        [StudentId] uniqueidentifier NULL,
                        CONSTRAINT [PK_Applications] PRIMARY KEY ([IdApplication])
                    );
                    CREATE UNIQUE INDEX [IX_Application_Job_Student] ON [Applications] ([IdJob], [IdStudent]);
                    CREATE INDEX [IX_applications_JobIdJob] ON [Applications] ([JobIdJob]);
                    CREATE INDEX [IX_applications_StudentId] ON [Applications] ([StudentId]);
                END
            ");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Contracts')
                BEGIN
                    CREATE TABLE [Contracts] (
                        [IdContract] int IDENTITY(1,1) NOT NULL,
                        [IdApplication] int NOT NULL,
                        [IdJob] int NOT NULL,
                        [IdStudent] uniqueidentifier NOT NULL,
                        [IdCompany] uniqueidentifier NOT NULL,
                        [Status] nvarchar(max) NULL,
                        [ContractData] nvarchar(max) NULL,
                        [CreatedAt] datetime2 NOT NULL DEFAULT GETUTCDATE(),
                        [UpdatedAt] datetime2 NULL,
                        [AcceptedAt] datetime2 NULL,
                        CONSTRAINT [PK_Contracts] PRIMARY KEY ([IdContract]),
                        CONSTRAINT [FK_Contracts_Applications_IdApplication] FOREIGN KEY ([IdApplication]) REFERENCES [Applications] ([IdApplication]) ON DELETE CASCADE,
                        CONSTRAINT [FK_Contracts_Jobs_IdJob] FOREIGN KEY ([IdJob]) REFERENCES [jobs] ([IdJob]),
                        CONSTRAINT [FK_Contracts_User_IdCompany] FOREIGN KEY ([IdCompany]) REFERENCES [users] ([Id]),
                        CONSTRAINT [FK_Contracts_User_IdStudent] FOREIGN KEY ([IdStudent]) REFERENCES [users] ([Id])
                    );
                    CREATE UNIQUE INDEX [IX_Contracts_IdApplication] ON [Contracts] ([IdApplication]);
                    CREATE INDEX [IX_Contracts_IdCompany] ON [Contracts] ([IdCompany]);
                    CREATE INDEX [IX_Contracts_IdJob] ON [Contracts] ([IdJob]);
                    CREATE INDEX [IX_Contracts_IdStudent] ON [Contracts] ([IdStudent]);
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contracts");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Jobs");
        }
    }
}
