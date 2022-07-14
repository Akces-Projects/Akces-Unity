using Microsoft.EntityFrameworkCore.Migrations;

namespace Akces.Unity.DataAccess.Migrations
{
    public partial class init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CalculateOrderPositionQuantityScript",
                table: "Accounts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConcludeProductSymbolScript",
                table: "Accounts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MatchAssormentScript",
                table: "Accounts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CalculateOrderPositionQuantityScript",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "ConcludeProductSymbolScript",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "MatchAssormentScript",
                table: "Accounts");
        }
    }
}
