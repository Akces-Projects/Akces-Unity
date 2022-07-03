using Microsoft.EntityFrameworkCore.Migrations;

namespace Akces.Unity.DataAccess.Migrations
{
    public partial class init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PositionsCount",
                table: "OperationReports",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PositionsCount",
                table: "OperationReports");
        }
    }
}
