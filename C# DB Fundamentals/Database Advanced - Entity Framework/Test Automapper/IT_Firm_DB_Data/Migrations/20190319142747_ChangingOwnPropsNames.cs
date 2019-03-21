using Microsoft.EntityFrameworkCore.Migrations;

namespace IT_Firm_DB_Data.Migrations
{
    public partial class ChangingOwnPropsNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name_LastName",
                table: "Employees",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "Name_FirstName",
                table: "Employees",
                newName: "FirstName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Employees",
                newName: "Name_LastName");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Employees",
                newName: "Name_FirstName");
        }
    }
}
