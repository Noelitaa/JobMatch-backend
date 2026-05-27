using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobMatchBackend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "skills",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_skills", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AvatarUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    EmailVerified = table.Column<bool>(type: "bit", nullable: false),
                    University = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Career = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "availabilities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_availabilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_availabilities_users_StudentId",
                        column: x => x.StudentId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "fcm_tokens",
                columns: table => new
                {
                    IdToken = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeviceInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fcm_tokens", x => x.IdToken);
                    table.ForeignKey(
                        name: "FK_fcm_tokens_users_IdUser",
                        column: x => x.IdUser,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "jobs",
                columns: table => new
                {
                    IdJob = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_company = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    payment = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    payment_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    work_date = table.Column<DateOnly>(type: "date", nullable: false),
                    start_time = table.Column<TimeOnly>(type: "time", nullable: false),
                    end_time = table.Column<TimeOnly>(type: "time", nullable: false),
                    start_date = table.Column<DateOnly>(type: "date", nullable: true),
                    end_date = table.Column<DateOnly>(type: "date", nullable: true),
                    deliverables = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_jobs", x => x.IdJob);
                    table.ForeignKey(
                        name: "FK_jobs_users_id_company",
                        column: x => x.id_company,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "moderation_logs",
                columns: table => new
                {
                    IdLog = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdminUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TargetId = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_moderation_logs", x => x.IdLog);
                    table.ForeignKey(
                        name: "FK_moderation_logs_users_AdminUserId",
                        column: x => x.AdminUserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "notifications",
                columns: table => new
                {
                    IdNotification = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notifications", x => x.IdNotification);
                    table.ForeignKey(
                        name: "FK_notifications_users_IdUser",
                        column: x => x.IdUser,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "student_skills",
                columns: table => new
                {
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SkillId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_skills", x => new { x.StudentId, x.SkillId });
                    table.ForeignKey(
                        name: "FK_student_skills_skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_student_skills_users_StudentId",
                        column: x => x.StudentId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "applications",
                columns: table => new
                {
                    IdApplication = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdJob = table.Column<int>(type: "int", nullable: false),
                    IdStudent = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_applications", x => x.IdApplication);
                    table.ForeignKey(
                        name: "FK_applications_jobs_IdJob",
                        column: x => x.IdJob,
                        principalTable: "jobs",
                        principalColumn: "IdJob",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_applications_users_IdStudent",
                        column: x => x.IdStudent,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "contracts",
                columns: table => new
                {
                    IdContract = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdApplication = table.Column<int>(type: "int", nullable: false),
                    IdJob = table.Column<int>(type: "int", nullable: false),
                    IdStudent = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdCompany = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContractData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AcceptedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contracts", x => x.IdContract);
                    table.ForeignKey(
                        name: "FK_contracts_applications_IdApplication",
                        column: x => x.IdApplication,
                        principalTable: "applications",
                        principalColumn: "IdApplication",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_contracts_jobs_IdJob",
                        column: x => x.IdJob,
                        principalTable: "jobs",
                        principalColumn: "IdJob",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_contracts_users_IdCompany",
                        column: x => x.IdCompany,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_contracts_users_IdStudent",
                        column: x => x.IdStudent,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "payments",
                columns: table => new
                {
                    IdPayment = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdContract = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ReceiptUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payments", x => x.IdPayment);
                    table.ForeignKey(
                        name: "FK_payments_contracts_IdContract",
                        column: x => x.IdContract,
                        principalTable: "contracts",
                        principalColumn: "IdContract",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ratings",
                columns: table => new
                {
                    IdRating = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdContract = table.Column<int>(type: "int", nullable: false),
                    IdRater = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdRated = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Stars = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ratings", x => x.IdRating);
                    table.ForeignKey(
                        name: "FK_ratings_contracts_IdContract",
                        column: x => x.IdContract,
                        principalTable: "contracts",
                        principalColumn: "IdContract",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ratings_users_IdRated",
                        column: x => x.IdRated,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ratings_users_IdRater",
                        column: x => x.IdRater,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Application_Job_Student",
                table: "applications",
                columns: new[] { "IdJob", "IdStudent" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_applications_IdStudent",
                table: "applications",
                column: "IdStudent");

            migrationBuilder.CreateIndex(
                name: "IX_availabilities_StudentId",
                table: "availabilities",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_contracts_IdApplication",
                table: "contracts",
                column: "IdApplication",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_contracts_IdCompany",
                table: "contracts",
                column: "IdCompany");

            migrationBuilder.CreateIndex(
                name: "IX_contracts_IdJob",
                table: "contracts",
                column: "IdJob");

            migrationBuilder.CreateIndex(
                name: "IX_contracts_IdStudent",
                table: "contracts",
                column: "IdStudent");

            migrationBuilder.CreateIndex(
                name: "IX_fcm_tokens_IdUser",
                table: "fcm_tokens",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_jobs_id_company",
                table: "jobs",
                column: "id_company");

            migrationBuilder.CreateIndex(
                name: "IX_moderation_logs_AdminUserId",
                table: "moderation_logs",
                column: "AdminUserId");

            migrationBuilder.CreateIndex(
                name: "IX_notifications_IdUser",
                table: "notifications",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_payments_IdContract",
                table: "payments",
                column: "IdContract");

            migrationBuilder.CreateIndex(
                name: "IX_ratings_IdContract_IdRater",
                table: "ratings",
                columns: new[] { "IdContract", "IdRater" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ratings_IdRated",
                table: "ratings",
                column: "IdRated");

            migrationBuilder.CreateIndex(
                name: "IX_ratings_IdRater",
                table: "ratings",
                column: "IdRater");

            migrationBuilder.CreateIndex(
                name: "IX_student_skills_SkillId",
                table: "student_skills",
                column: "SkillId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "availabilities");

            migrationBuilder.DropTable(
                name: "fcm_tokens");

            migrationBuilder.DropTable(
                name: "moderation_logs");

            migrationBuilder.DropTable(
                name: "notifications");

            migrationBuilder.DropTable(
                name: "payments");

            migrationBuilder.DropTable(
                name: "ratings");

            migrationBuilder.DropTable(
                name: "student_skills");

            migrationBuilder.DropTable(
                name: "contracts");

            migrationBuilder.DropTable(
                name: "skills");

            migrationBuilder.DropTable(
                name: "applications");

            migrationBuilder.DropTable(
                name: "jobs");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
