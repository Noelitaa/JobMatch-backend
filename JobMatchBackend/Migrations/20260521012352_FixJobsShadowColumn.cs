using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobMatchBackend.Migrations
{
    /// <inheritdoc />
    public partial class FixJobsShadowColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_jobs_users_CompanyId",
                table: "jobs");

            migrationBuilder.DropIndex(
                name: "IX_jobs_CompanyId",
                table: "jobs");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "jobs");

            migrationBuilder.CreateIndex(
                name: "IX_jobs_IdCompany",
                table: "jobs",
                column: "IdCompany");

            // Remove orphaned jobs whose company no longer exists in users
            migrationBuilder.Sql(@"
                DELETE j FROM [jobs] j
                LEFT JOIN [users] u ON j.[IdCompany] = u.[Id]
                WHERE u.[Id] IS NULL;
            ");

            migrationBuilder.AddForeignKey(
                name: "FK_jobs_users_IdCompany",
                table: "jobs",
                column: "IdCompany",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_jobs_users_IdCompany",
                table: "jobs");

            migrationBuilder.DropIndex(
                name: "IX_jobs_IdCompany",
                table: "jobs");

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId",
                table: "jobs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_jobs_CompanyId",
                table: "jobs",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_jobs_users_CompanyId",
                table: "jobs",
                column: "CompanyId",
                principalTable: "users",
                principalColumn: "Id");
        }
    }
}
