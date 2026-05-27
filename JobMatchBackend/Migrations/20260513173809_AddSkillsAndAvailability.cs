using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobMatchBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddSkillsAndAvailability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Skills')
                BEGIN
                    CREATE TABLE [Skills] (
                        [Id] uniqueidentifier NOT NULL,
                        [Name] nvarchar(max) NOT NULL,
                        CONSTRAINT [PK_Skills] PRIMARY KEY ([Id])
                    );
                END
            ");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Availabilities')
                BEGIN
                    CREATE TABLE [Availabilities] (
                        [Id] uniqueidentifier NOT NULL,
                        [StudentId] uniqueidentifier NOT NULL,
                        [DayOfWeek] int NOT NULL,
                        [StartTime] time NOT NULL,
                        [EndTime] time NOT NULL,
                        CONSTRAINT [PK_Availabilities] PRIMARY KEY ([Id]),
                        CONSTRAINT [FK_Availabilities_User_StudentId] FOREIGN KEY ([StudentId])
                            REFERENCES [User] ([Id]) ON DELETE CASCADE
                    );
                    CREATE INDEX [IX_Availabilities_StudentId] ON [Availabilities] ([StudentId]);
                END
            ");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'StudentSkills')
                BEGIN
                    CREATE TABLE [StudentSkills] (
                        [StudentId] uniqueidentifier NOT NULL,
                        [SkillId] uniqueidentifier NOT NULL,
                        CONSTRAINT [PK_StudentSkills] PRIMARY KEY ([StudentId], [SkillId]),
                        CONSTRAINT [FK_StudentSkills_Skills_SkillId] FOREIGN KEY ([SkillId])
                            REFERENCES [Skills] ([Id]) ON DELETE CASCADE,
                        CONSTRAINT [FK_StudentSkills_User_StudentId] FOREIGN KEY ([StudentId])
                            REFERENCES [User] ([Id]) ON DELETE CASCADE
                    );
                    CREATE INDEX [IX_StudentSkills_SkillId] ON [StudentSkills] ([SkillId]);
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'StudentSkills')
                    DROP TABLE [StudentSkills];
            ");

            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Availabilities')
                    DROP TABLE [Availabilities];
            ");

            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Skills')
                    DROP TABLE [Skills];
            ");
        }
    }
}
