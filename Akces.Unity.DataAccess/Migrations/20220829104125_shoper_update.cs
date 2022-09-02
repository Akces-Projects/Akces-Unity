using Microsoft.EntityFrameworkCore.Migrations;

namespace Akces.Unity.DataAccess.Migrations
{
    public partial class shoper_update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BaseAddress",
                table: "ShoperConfiguration",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                table: "ShoperConfiguration",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientSecret",
                table: "ShoperConfiguration",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ImportOffset_Hours",
                table: "ShoperConfiguration",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "ImportOrders_OnlyCashOnDeliveryOrPaid",
                table: "ShoperConfiguration",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ImportOrders_OnlyConfirmed",
                table: "ShoperConfiguration",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "ShoperConfiguration",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BaseAddress",
                table: "ShoperConfiguration");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "ShoperConfiguration");

            migrationBuilder.DropColumn(
                name: "ClientSecret",
                table: "ShoperConfiguration");

            migrationBuilder.DropColumn(
                name: "ImportOffset_Hours",
                table: "ShoperConfiguration");

            migrationBuilder.DropColumn(
                name: "ImportOrders_OnlyCashOnDeliveryOrPaid",
                table: "ShoperConfiguration");

            migrationBuilder.DropColumn(
                name: "ImportOrders_OnlyConfirmed",
                table: "ShoperConfiguration");

            migrationBuilder.DropColumn(
                name: "Token",
                table: "ShoperConfiguration");
        }
    }
}
