using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobMatchBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddStudentSkills_Empty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='student_skills' AND xtype='U')
                BEGIN
                    CREATE TABLE student_skills (
                        id_skill uniqueidentifier NOT NULL,
                        id_student uniqueidentifier NOT NULL,
                        CONSTRAINT PK_student_skills PRIMARY KEY (id_skill, id_student),
                        CONSTRAINT FK_student_skills_Skills_id_skill FOREIGN KEY (id_skill) REFERENCES Skills(Id) ON DELETE CASCADE,
                        CONSTRAINT FK_student_skills_User_id_student FOREIGN KEY (id_student) REFERENCES [User](Id) ON DELETE CASCADE
                    );
                    
                    CREATE INDEX IX_student_skills_id_student ON student_skills(id_student);
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sysobjects WHERE name='student_skills' AND xtype='U')
                BEGIN
                    DROP TABLE student_skills;
                END
            ");
        }
    }
}