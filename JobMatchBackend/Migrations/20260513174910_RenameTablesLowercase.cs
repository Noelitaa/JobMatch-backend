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

            // Drop FKs that exist on tables being renamed
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Jobs_User_CompanyId')
                    ALTER TABLE [Jobs] DROP CONSTRAINT [FK_Jobs_User_CompanyId];
            ");
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

            // Drop PKs so tables can be renamed cleanly
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = 'PK_Skills')
                    ALTER TABLE [Skills] DROP CONSTRAINT [PK_Skills];
            ");
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = 'PK_Jobs')
                    ALTER TABLE [Jobs] DROP CONSTRAINT [PK_Jobs];
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

            // Drop indexes
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Availabilities_StudentId')
                    DROP INDEX [IX_Availabilities_StudentId] ON [Availabilities];
            ");
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_StudentSkills_SkillId')
                    DROP INDEX [IX_StudentSkills_SkillId] ON [StudentSkills];
            ");

            // Rename tables (guarded: only rename if the Pascal-case name still exists)
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Skills' AND schema_id = SCHEMA_ID('dbo'))
                    EXEC sp_rename N'dbo.Skills', N'skills', N'OBJECT';
            ");
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Jobs' AND schema_id = SCHEMA_ID('dbo'))
                    EXEC sp_rename N'dbo.Jobs', N'jobs', N'OBJECT';
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

            // Re-add PKs with new names
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = 'PK_skills')
                    ALTER TABLE [skills] ADD CONSTRAINT [PK_skills] PRIMARY KEY ([Id]);
            ");
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = 'PK_jobs')
                    ALTER TABLE [jobs] ADD CONSTRAINT [PK_jobs] PRIMARY KEY ([IdJob]);
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

            // Re-add indexes
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_availabilities_StudentId')
                    CREATE INDEX [IX_availabilities_StudentId] ON [availabilities] ([StudentId]);
            ");
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_student_skills_SkillId')
                    CREATE INDEX [IX_student_skills_SkillId] ON [student_skills] ([SkillId]);
            ");

            // Re-add FKs pointing to renamed tables
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_jobs_users_CompanyId')
                AND EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('jobs') AND name = 'CompanyId')
                    ALTER TABLE [jobs]
                        ADD CONSTRAINT [FK_jobs_users_CompanyId]
                        FOREIGN KEY ([CompanyId]) REFERENCES [users] ([Id]);
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
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_student_skills_skills_SkillId')
                    ALTER TABLE [student_skills] DROP CONSTRAINT [FK_student_skills_skills_SkillId];
                IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_student_skills_users_StudentId')
                    ALTER TABLE [student_skills] DROP CONSTRAINT [FK_student_skills_users_StudentId];
                IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_availabilities_users_StudentId')
                    ALTER TABLE [availabilities] DROP CONSTRAINT [FK_availabilities_users_StudentId];
            ");

            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = 'PK_skills') ALTER TABLE [skills] DROP CONSTRAINT [PK_skills];
                IF EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = 'PK_jobs') ALTER TABLE [jobs] DROP CONSTRAINT [PK_jobs];
                IF EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = 'PK_availabilities') ALTER TABLE [availabilities] DROP CONSTRAINT [PK_availabilities];
                IF EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = 'PK_users') ALTER TABLE [users] DROP CONSTRAINT [PK_users];
                IF EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = 'PK_student_skills') ALTER TABLE [student_skills] DROP CONSTRAINT [PK_student_skills];
            ");

            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'skills') EXEC sp_rename N'dbo.skills', N'Skills', N'OBJECT';");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'jobs') EXEC sp_rename N'dbo.jobs', N'Jobs', N'OBJECT';");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'availabilities') EXEC sp_rename N'dbo.availabilities', N'Availabilities', N'OBJECT';");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'users') EXEC sp_rename N'dbo.users', N'User', N'OBJECT';");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'student_skills') EXEC sp_rename N'dbo.student_skills', N'StudentSkills', N'OBJECT';");

            migrationBuilder.Sql("ALTER TABLE [Skills] ADD CONSTRAINT [PK_Skills] PRIMARY KEY ([Id]);");
            migrationBuilder.Sql("ALTER TABLE [Jobs] ADD CONSTRAINT [PK_Jobs] PRIMARY KEY ([IdJob]);");
            migrationBuilder.Sql("ALTER TABLE [Availabilities] ADD CONSTRAINT [PK_Availabilities] PRIMARY KEY ([Id]);");
            migrationBuilder.Sql("ALTER TABLE [User] ADD CONSTRAINT [PK_User] PRIMARY KEY ([Id]);");
            migrationBuilder.Sql("ALTER TABLE [StudentSkills] ADD CONSTRAINT [PK_StudentSkills] PRIMARY KEY ([StudentId], [SkillId]);");
            migrationBuilder.Sql("ALTER TABLE [Availabilities] ADD CONSTRAINT [FK_Availabilities_User_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [User] ([Id]) ON DELETE CASCADE;");
            migrationBuilder.Sql("ALTER TABLE [StudentSkills] ADD CONSTRAINT [FK_StudentSkills_Skills_SkillId] FOREIGN KEY ([SkillId]) REFERENCES [Skills] ([Id]) ON DELETE CASCADE;");
            migrationBuilder.Sql("ALTER TABLE [StudentSkills] ADD CONSTRAINT [FK_StudentSkills_User_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [User] ([Id]) ON DELETE CASCADE;");
        }
    }
}
