using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobMatchBackend.Migrations
{
    public partial class ConvertContractDataToView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TABLE IF EXISTS [ContractData];");

            migrationBuilder.Sql(@"
                CREATE VIEW [vw_ContractData] AS
                SELECT
                    c.IdContract,
                    a.IdApplication,
                    j.IdJob,
                    u_student.Id   AS IdStudent,
                    j.Title        AS JobTitle,
                    j.Type         AS JobType,
                    u_company.CompanyName,
                    u_company.Email         AS CompanyEmail,
                    u_company.FullName      AS CompanyOwnerName,
                    u_student.FullName      AS StudentName,
                    u_student.Email         AS StudentEmail,
                    u_student.University    AS StudentUniversity,
                    u_student.Career        AS StudentCareer
                FROM [contracts] c
                JOIN [applications] a  ON c.IdApplication = a.IdApplication
                JOIN [jobs] j          ON c.IdJob = j.IdJob
                JOIN [users] u_student ON c.IdStudent = u_student.Id
                JOIN [users] u_company ON c.IdCompany  = u_company.Id;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS [vw_ContractData];");
        }
    }
}
