using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobMatchBackend.Migrations
{
    /// <inheritdoc />
    public partial class FixApplicationsShadowColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Shadow FKs may have been dropped already by RenameTablesLowercase — guard them
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_applications_jobs_JobIdJob'
                           OR name = 'FK_Applications_Jobs_JobIdJob')
                    ALTER TABLE [applications] DROP CONSTRAINT [FK_Applications_Jobs_JobIdJob];
            ");
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_applications_users_StudentId'
                           OR name = 'FK_Applications_User_StudentId')
                    ALTER TABLE [applications] DROP CONSTRAINT [FK_Applications_User_StudentId];
            ");

            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_applications_IdApplication",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_jobs_IdJob",
                table: "Contracts");

            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Contracts_users_IdCompany' AND parent_object_id = OBJECT_ID('Contracts'))
                    ALTER TABLE [Contracts] DROP CONSTRAINT [FK_Contracts_users_IdCompany];
                IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Contracts_User_IdCompany' AND parent_object_id = OBJECT_ID('Contracts'))
                    ALTER TABLE [Contracts] DROP CONSTRAINT [FK_Contracts_User_IdCompany];
            ");
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Contracts_users_IdStudent' AND parent_object_id = OBJECT_ID('Contracts'))
                    ALTER TABLE [Contracts] DROP CONSTRAINT [FK_Contracts_users_IdStudent];
                IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Contracts_User_IdStudent' AND parent_object_id = OBJECT_ID('Contracts'))
                    ALTER TABLE [Contracts] DROP CONSTRAINT [FK_Contracts_User_IdStudent];
            ");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contracts",
                table: "Contracts");

            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_applications_JobIdJob'
                           AND object_id = OBJECT_ID('applications'))
                    DROP INDEX [IX_applications_JobIdJob] ON [applications];
            ");
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_applications_StudentId'
                           AND object_id = OBJECT_ID('applications'))
                    DROP INDEX [IX_applications_StudentId] ON [applications];
            ");

            migrationBuilder.DropColumn(
                name: "JobIdJob",
                table: "applications");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "applications");

            migrationBuilder.RenameTable(
                name: "Contracts",
                newName: "contracts");

            migrationBuilder.RenameIndex(
                name: "IX_Contracts_IdStudent",
                table: "contracts",
                newName: "IX_contracts_IdStudent");

            migrationBuilder.RenameIndex(
                name: "IX_Contracts_IdJob",
                table: "contracts",
                newName: "IX_contracts_IdJob");

            migrationBuilder.RenameIndex(
                name: "IX_Contracts_IdCompany",
                table: "contracts",
                newName: "IX_contracts_IdCompany");

            migrationBuilder.RenameIndex(
                name: "IX_Contracts_IdApplication",
                table: "contracts",
                newName: "IX_contracts_IdApplication");

            migrationBuilder.AddPrimaryKey(
                name: "PK_contracts",
                table: "contracts",
                column: "IdContract");

            migrationBuilder.CreateIndex(
                name: "IX_applications_IdStudent",
                table: "applications",
                column: "IdStudent");

            migrationBuilder.AddForeignKey(
                name: "FK_applications_jobs_IdJob",
                table: "applications",
                column: "IdJob",
                principalTable: "jobs",
                principalColumn: "IdJob",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_applications_users_IdStudent",
                table: "applications",
                column: "IdStudent",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_contracts_applications_IdApplication",
                table: "contracts",
                column: "IdApplication",
                principalTable: "applications",
                principalColumn: "IdApplication",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_contracts_jobs_IdJob",
                table: "contracts",
                column: "IdJob",
                principalTable: "jobs",
                principalColumn: "IdJob",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_contracts_users_IdCompany",
                table: "contracts",
                column: "IdCompany",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_contracts_users_IdStudent",
                table: "contracts",
                column: "IdStudent",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_applications_jobs_IdJob",
                table: "applications");

            migrationBuilder.DropForeignKey(
                name: "FK_applications_users_IdStudent",
                table: "applications");

            migrationBuilder.DropForeignKey(
                name: "FK_contracts_applications_IdApplication",
                table: "contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_contracts_jobs_IdJob",
                table: "contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_contracts_users_IdCompany",
                table: "contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_contracts_users_IdStudent",
                table: "contracts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_contracts",
                table: "contracts");

            migrationBuilder.DropIndex(
                name: "IX_applications_IdStudent",
                table: "applications");

            migrationBuilder.RenameTable(
                name: "contracts",
                newName: "Contracts");

            migrationBuilder.RenameIndex(
                name: "IX_contracts_IdStudent",
                table: "Contracts",
                newName: "IX_Contracts_IdStudent");

            migrationBuilder.RenameIndex(
                name: "IX_contracts_IdJob",
                table: "Contracts",
                newName: "IX_Contracts_IdJob");

            migrationBuilder.RenameIndex(
                name: "IX_contracts_IdCompany",
                table: "Contracts",
                newName: "IX_Contracts_IdCompany");

            migrationBuilder.RenameIndex(
                name: "IX_contracts_IdApplication",
                table: "Contracts",
                newName: "IX_Contracts_IdApplication");

            migrationBuilder.AddColumn<int>(
                name: "JobIdJob",
                table: "applications",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StudentId",
                table: "applications",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contracts",
                table: "Contracts",
                column: "IdContract");

            migrationBuilder.CreateIndex(
                name: "IX_applications_JobIdJob",
                table: "applications",
                column: "JobIdJob");

            migrationBuilder.CreateIndex(
                name: "IX_applications_StudentId",
                table: "applications",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_applications_jobs_JobIdJob",
                table: "applications",
                column: "JobIdJob",
                principalTable: "jobs",
                principalColumn: "IdJob");

            migrationBuilder.AddForeignKey(
                name: "FK_applications_users_StudentId",
                table: "applications",
                column: "StudentId",
                principalTable: "users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_applications_IdApplication",
                table: "Contracts",
                column: "IdApplication",
                principalTable: "applications",
                principalColumn: "IdApplication",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_jobs_IdJob",
                table: "Contracts",
                column: "IdJob",
                principalTable: "jobs",
                principalColumn: "IdJob",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_users_IdCompany",
                table: "Contracts",
                column: "IdCompany",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_users_IdStudent",
                table: "Contracts",
                column: "IdStudent",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
