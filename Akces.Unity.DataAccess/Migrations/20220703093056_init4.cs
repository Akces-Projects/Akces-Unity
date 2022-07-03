using Microsoft.EntityFrameworkCore.Migrations;

namespace Akces.Unity.DataAccess.Migrations
{
    public partial class init4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ObjectName",
                table: "OperationReportPosition",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ObjectName",
                table: "OperationReportPosition");
        }
    }
}
