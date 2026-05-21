using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobMatchBackend.Migrations
{
    /// <inheritdoc />
    public partial class RenameTablesLowercase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the old pre-existing student_skills table before renaming EF's StudentSkills
            migrationBuilder.Sql("DROP TABLE IF EXISTS student_skills;");

            // ---------------------------------------------------------------
            // Drop ALL FKs referencing tables that will be renamed
            // (both FKs ON the table and FKs FROM other tables pointing TO it)
            // ---------------------------------------------------------------

            // FKs on Jobs
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Jobs_User_CompanyId')
                    ALTER TABLE [Jobs] DROP CONSTRAINT [FK_Jobs_User_CompanyId];
            ");

            // FKs FROM Applications pointing to Jobs and User
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Applications_Jobs_JobIdJob')
                    ALTER TABLE [Applications] DROP CONSTRAINT [FK_Applications_Jobs_JobIdJob];
            ");
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Applications_User_StudentId')
                    ALTER TABLE [Applications] DROP CONSTRAINT [FK_Applications_User_StudentId];
            ");

            // FKs FROM Contracts pointing to Jobs, Applications, and User
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Contracts_Jobs_IdJob')
                    ALTER TABLE [Contracts] DROP CONSTRAINT [FK_Contracts_Jobs_IdJob];
            ");
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Contracts_Applications_IdApplication')
                    ALTER TABLE [Contracts] DROP CONSTRAINT [FK_Contracts_Applications_IdApplication];
            ");
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Contracts_User_IdCompany')
                    ALTER TABLE [Contracts] DROP CONSTRAINT [FK_Contracts_User_IdCompany];
            ");
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Contracts_User_IdStudent')
                    ALTER TABLE [Contracts] DROP CONSTRAINT [FK_Contracts_User_IdStudent];
            ");

            // FKs on Availabilities and StudentSkills
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Availabilities_User_StudentId')
                    ALTER TABLE [Availabilities] DROP CONSTRAINT [FK_Availabilities_User_StudentId];
            ");
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_StudentSkills_Skills_SkillId')
                    ALTER TABLE [StudentSkills] DROP CONSTRAINT [FK_StudentSkills_Skills_SkillId];
            ");
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_StudentSkills_User_StudentId')
                    ALTER TABLE [StudentSkills] DROP CONSTRAINT [FK_StudentSkills_User_StudentId];
            ");

            // ---------------------------------------------------------------
            // Drop PKs
            // ---------------------------------------------------------------
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = 'PK_Skills')
                    ALTER TABLE [Skills] DROP CONSTRAINT [PK_Skills];
            ");
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = 'PK_Jobs')
                    ALTER TABLE [Jobs] DROP CONSTRAINT [PK_Jobs];
            ");
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = 'PK_Applications')
                    ALTER TABLE [Applications] DROP CONSTRAINT [PK_Applications];
            ");
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = 'PK_Contracts')
                    ALTER TABLE [Contracts] DROP CONSTRAINT [PK_Contracts];
            ");
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = 'PK_Availabilities')
                    ALTER TABLE [Availabilities] DROP CONSTRAINT [PK_Availabilities];
            ");
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = 'PK_User')
                    ALTER TABLE [User] DROP CONSTRAINT [PK_User];
            ");
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = 'PK_StudentSkills')
                    ALTER TABLE [StudentSkills] DROP CONSTRAINT [PK_StudentSkills];
            ");

            // ---------------------------------------------------------------
            // Drop indexes
            // ---------------------------------------------------------------
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Availabilities_StudentId')
                    DROP INDEX [IX_Availabilities_StudentId] ON [Availabilities];
            ");
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_StudentSkills_SkillId')
                    DROP INDEX [IX_StudentSkills_SkillId] ON [StudentSkills];
            ");
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Applications_JobIdJob')
                    DROP INDEX [IX_Applications_JobIdJob] ON [Applications];
            ");
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Applications_StudentId')
                    DROP INDEX [IX_Applications_StudentId] ON [Applications];
            ");
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Application_Job_Student')
                    DROP INDEX [IX_Application_Job_Student] ON [Applications];
            ");
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Jobs_CompanyId')
                    DROP INDEX [IX_Jobs_CompanyId] ON [Jobs];
            ");
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Contracts_IdApplication')
                    DROP INDEX [IX_Contracts_IdApplication] ON [Contracts];
            ");
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Contracts_IdCompany')
                    DROP INDEX [IX_Contracts_IdCompany] ON [Contracts];
            ");
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Contracts_IdJob')
                    DROP INDEX [IX_Contracts_IdJob] ON [Contracts];
            ");
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Contracts_IdStudent')
                    DROP INDEX [IX_Contracts_IdStudent] ON [Contracts];
            ");

            // ---------------------------------------------------------------
            // Rename tables
            // ---------------------------------------------------------------
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Skills' AND schema_id = SCHEMA_ID('dbo'))
                    EXEC sp_rename N'dbo.Skills', N'skills', N'OBJECT';
            ");
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Jobs' AND schema_id = SCHEMA_ID('dbo'))
                    EXEC sp_rename N'dbo.Jobs', N'jobs', N'OBJECT';
            ");
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Applications' AND schema_id = SCHEMA_ID('dbo'))
                    EXEC sp_rename N'dbo.Applications', N'applications', N'OBJECT';
            ");
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Contracts' AND schema_id = SCHEMA_ID('dbo'))
                    EXEC sp_rename N'dbo.Contracts', N'contracts', N'OBJECT';
            ");
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Availabilities' AND schema_id = SCHEMA_ID('dbo'))
                    EXEC sp_rename N'dbo.Availabilities', N'availabilities', N'OBJECT';
            ");
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'User' AND schema_id = SCHEMA_ID('dbo'))
                    EXEC sp_rename N'dbo.User', N'users', N'OBJECT';
            ");
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'StudentSkills' AND schema_id = SCHEMA_ID('dbo'))
                    EXEC sp_rename N'dbo.StudentSkills', N'student_skills', N'OBJECT';
            ");

            // ---------------------------------------------------------------
            // Re-add PKs
            // ---------------------------------------------------------------
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = 'PK_skills')
                    ALTER TABLE [skills] ADD CONSTRAINT [PK_skills] PRIMARY KEY ([Id]);
            ");
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = 'PK_jobs')
                    ALTER TABLE [jobs] ADD CONSTRAINT [PK_jobs] PRIMARY KEY ([IdJob]);
            ");
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = 'PK_applications')
                    ALTER TABLE [applications] ADD CONSTRAINT [PK_applications] PRIMARY KEY ([IdApplication]);
            ");
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = 'PK_contracts')
                    ALTER TABLE [contracts] ADD CONSTRAINT [PK_contracts] PRIMARY KEY ([IdContract]);
            ");
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = 'PK_availabilities')
                    ALTER TABLE [availabilities] ADD CONSTRAINT [PK_availabilities] PRIMARY KEY ([Id]);
            ");
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = 'PK_users')
                    ALTER TABLE [users] ADD CONSTRAINT [PK_users] PRIMARY KEY ([Id]);
            ");
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = 'PK_student_skills')
                    ALTER TABLE [student_skills] ADD CONSTRAINT [PK_student_skills] PRIMARY KEY ([StudentId], [SkillId]);
            ");

            // ---------------------------------------------------------------
            // Re-add indexes
            // ---------------------------------------------------------------
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_availabilities_StudentId')
                    CREATE INDEX [IX_availabilities_StudentId] ON [availabilities] ([StudentId]);
            ");
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_student_skills_SkillId')
                    CREATE INDEX [IX_student_skills_SkillId] ON [student_skills] ([SkillId]);
            ");
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_applications_JobIdJob')
                    CREATE INDEX [IX_applications_JobIdJob] ON [applications] ([JobIdJob]);
            ");
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_applications_StudentId')
                    CREATE INDEX [IX_applications_StudentId] ON [applications] ([StudentId]);
            ");
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Application_Job_Student')
                    CREATE UNIQUE INDEX [IX_Application_Job_Student] ON [applications] ([IdJob], [IdStudent]);
            ");
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_jobs_CompanyId')
                    CREATE INDEX [IX_jobs_CompanyId] ON [jobs] ([CompanyId]);
            ");
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_contracts_IdApplication')
                    CREATE UNIQUE INDEX [IX_contracts_IdApplication] ON [contracts] ([IdApplication]);
            ");
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_contracts_IdCompany')
                    CREATE INDEX [IX_contracts_IdCompany] ON [contracts] ([IdCompany]);
            ");
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_contracts_IdJob')
                    CREATE INDEX [IX_contracts_IdJob] ON [contracts] ([IdJob]);
            ");
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_contracts_IdStudent')
                    CREATE INDEX [IX_contracts_IdStudent] ON [contracts] ([IdStudent]);
            ");

            // ---------------------------------------------------------------
            // Re-add FKs pointing to renamed tables
            // ---------------------------------------------------------------
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_jobs_users_CompanyId')
                AND EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('jobs') AND name = 'CompanyId')
                    ALTER TABLE [jobs]
                        ADD CONSTRAINT [FK_jobs_users_CompanyId]
                        FOREIGN KEY ([CompanyId]) REFERENCES [users] ([Id]);
            ");
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_applications_jobs_JobIdJob')
                    ALTER TABLE [applications]
                        ADD CONSTRAINT [FK_applications_jobs_JobIdJob]
                        FOREIGN KEY ([JobIdJob]) REFERENCES [jobs] ([IdJob]);
            ");
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_applications_users_StudentId')
                    ALTER TABLE [applications]
                        ADD CONSTRAINT [FK_applications_users_StudentId]
                        FOREIGN KEY ([StudentId]) REFERENCES [users] ([Id]);
            ");
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_contracts_applications_IdApplication')
                    ALTER TABLE [contracts]
                        ADD CONSTRAINT [FK_contracts_applications_IdApplication]
                        FOREIGN KEY ([IdApplication]) REFERENCES [applications] ([IdApplication]) ON DELETE CASCADE;
            ");
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_contracts_jobs_IdJob')
                    ALTER TABLE [contracts]
                        ADD CONSTRAINT [FK_contracts_jobs_IdJob]
                        FOREIGN KEY ([IdJob]) REFERENCES [jobs] ([IdJob]) ON DELETE NO ACTION;
            ");
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_contracts_users_IdCompany')
                    ALTER TABLE [contracts]
                        ADD CONSTRAINT [FK_contracts_users_IdCompany]
                        FOREIGN KEY ([IdCompany]) REFERENCES [users] ([Id]) ON DELETE NO ACTION;
            ");
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_contracts_users_IdStudent')
                    ALTER TABLE [contracts]
                        ADD CONSTRAINT [FK_contracts_users_IdStudent]
                        FOREIGN KEY ([IdStudent]) REFERENCES [users] ([Id]) ON DELETE NO ACTION;
            ");
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_availabilities_users_StudentId')
                    ALTER TABLE [availabilities]
                        ADD CONSTRAINT [FK_availabilities_users_StudentId]
                        FOREIGN KEY ([StudentId]) REFERENCES [users] ([Id]) ON DELETE CASCADE;
            ");
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_student_skills_skills_SkillId')
                    ALTER TABLE [student_skills]
                        ADD CONSTRAINT [FK_student_skills_skills_SkillId]
                        FOREIGN KEY ([SkillId]) REFERENCES [skills] ([Id]) ON DELETE CASCADE;
            ");
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_student_skills_users_StudentId')
                    ALTER TABLE [student_skills]
                        ADD CONSTRAINT [FK_student_skills_users_StudentId]
                        FOREIGN KEY ([StudentId]) REFERENCES [users] ([Id]) ON DELETE CASCADE;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop all FKs
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_student_skills_skills_SkillId')
                    ALTER TABLE [student_skills] DROP CONSTRAINT [FK_student_skills_skills_SkillId];
                IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_student_skills_users_StudentId')
                    ALTER TABLE [student_skills] DROP CONSTRAINT [FK_student_skills_users_StudentId];
                IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_availabilities_users_StudentId')
                    ALTER TABLE [availabilities] DROP CONSTRAINT [FK_availabilities_users_StudentId];
                IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_contracts_jobs_IdJob')
                    ALTER TABLE [contracts] DROP CONSTRAINT [FK_contracts_jobs_IdJob];
                IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_contracts_applications_IdApplication')
                    ALTER TABLE [contracts] DROP CONSTRAINT [FK_contracts_applications_IdApplication];
                IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_contracts_users_IdCompany')
                    ALTER TABLE [contracts] DROP CONSTRAINT [FK_contracts_users_IdCompany];
                IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_contracts_users_IdStudent')
                    ALTER TABLE [contracts] DROP CONSTRAINT [FK_contracts_users_IdStudent];
                IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_applications_jobs_JobIdJob')
                    ALTER TABLE [applications] DROP CONSTRAINT [FK_applications_jobs_JobIdJob];
                IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_applications_users_StudentId')
                    ALTER TABLE [applications] DROP CONSTRAINT [FK_applications_users_StudentId];
                IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_jobs_users_CompanyId')
                    ALTER TABLE [jobs] DROP CONSTRAINT [FK_jobs_users_CompanyId];
            ");

            // Drop PKs
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = 'PK_skills') ALTER TABLE [skills] DROP CONSTRAINT [PK_skills];
                IF EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = 'PK_jobs') ALTER TABLE [jobs] DROP CONSTRAINT [PK_jobs];
                IF EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = 'PK_applications') ALTER TABLE [applications] DROP CONSTRAINT [PK_applications];
                IF EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = 'PK_contracts') ALTER TABLE [contracts] DROP CONSTRAINT [PK_contracts];
                IF EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = 'PK_availabilities') ALTER TABLE [availabilities] DROP CONSTRAINT [PK_availabilities];
                IF EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = 'PK_users') ALTER TABLE [users] DROP CONSTRAINT [PK_users];
                IF EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = 'PK_student_skills') ALTER TABLE [student_skills] DROP CONSTRAINT [PK_student_skills];
            ");

            // Rename back
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'skills') EXEC sp_rename N'dbo.skills', N'Skills', N'OBJECT';");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'jobs') EXEC sp_rename N'dbo.jobs', N'Jobs', N'OBJECT';");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'applications') EXEC sp_rename N'dbo.applications', N'Applications', N'OBJECT';");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'contracts') EXEC sp_rename N'dbo.contracts', N'Contracts', N'OBJECT';");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'availabilities') EXEC sp_rename N'dbo.availabilities', N'Availabilities', N'OBJECT';");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'users') EXEC sp_rename N'dbo.users', N'User', N'OBJECT';");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'student_skills') EXEC sp_rename N'dbo.student_skills', N'StudentSkills', N'OBJECT';");

            // Re-add PKs
            migrationBuilder.Sql("ALTER TABLE [Skills] ADD CONSTRAINT [PK_Skills] PRIMARY KEY ([Id]);");
            migrationBuilder.Sql("ALTER TABLE [Jobs] ADD CONSTRAINT [PK_Jobs] PRIMARY KEY ([IdJob]);");
            migrationBuilder.Sql("ALTER TABLE [Applications] ADD CONSTRAINT [PK_Applications] PRIMARY KEY ([IdApplication]);");
            migrationBuilder.Sql("ALTER TABLE [Contracts] ADD CONSTRAINT [PK_Contracts] PRIMARY KEY ([IdContract]);");
            migrationBuilder.Sql("ALTER TABLE [Availabilities] ADD CONSTRAINT [PK_Availabilities] PRIMARY KEY ([Id]);");
            migrationBuilder.Sql("ALTER TABLE [User] ADD CONSTRAINT [PK_User] PRIMARY KEY ([Id]);");
            migrationBuilder.Sql("ALTER TABLE [StudentSkills] ADD CONSTRAINT [PK_StudentSkills] PRIMARY KEY ([StudentId], [SkillId]);");

            // Re-add FKs
            migrationBuilder.Sql("ALTER TABLE [Jobs] ADD CONSTRAINT [FK_Jobs_User_CompanyId] FOREIGN KEY ([CompanyId]) REFERENCES [User] ([Id]);");
            migrationBuilder.Sql("ALTER TABLE [Applications] ADD CONSTRAINT [FK_Applications_Jobs_JobIdJob] FOREIGN KEY ([JobIdJob]) REFERENCES [Jobs] ([IdJob]);");
            migrationBuilder.Sql("ALTER TABLE [Applications] ADD CONSTRAINT [FK_Applications_User_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [User] ([Id]);");
            migrationBuilder.Sql("ALTER TABLE [Contracts] ADD CONSTRAINT [FK_Contracts_Jobs_IdJob] FOREIGN KEY ([IdJob]) REFERENCES [Jobs] ([IdJob]) ON DELETE NO ACTION;");
            migrationBuilder.Sql("ALTER TABLE [Contracts] ADD CONSTRAINT [FK_Contracts_Applications_IdApplication] FOREIGN KEY ([IdApplication]) REFERENCES [Applications] ([IdApplication]) ON DELETE CASCADE;");
            migrationBuilder.Sql("ALTER TABLE [Contracts] ADD CONSTRAINT [FK_Contracts_User_IdCompany] FOREIGN KEY ([IdCompany]) REFERENCES [User] ([Id]) ON DELETE NO ACTION;");
            migrationBuilder.Sql("ALTER TABLE [Contracts] ADD CONSTRAINT [FK_Contracts_User_IdStudent] FOREIGN KEY ([IdStudent]) REFERENCES [User] ([Id]) ON DELETE NO ACTION;");
            migrationBuilder.Sql("ALTER TABLE [Availabilities] ADD CONSTRAINT [FK_Availabilities_User_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [User] ([Id]) ON DELETE CASCADE;");
            migrationBuilder.Sql("ALTER TABLE [StudentSkills] ADD CONSTRAINT [FK_StudentSkills_Skills_SkillId] FOREIGN KEY ([SkillId]) REFERENCES [Skills] ([Id]) ON DELETE CASCADE;");
            migrationBuilder.Sql("ALTER TABLE [StudentSkills] ADD CONSTRAINT [FK_StudentSkills_User_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [User] ([Id]) ON DELETE CASCADE;");
        }
    }
}