using Microsoft.EntityFrameworkCore.Migrations;

namespace Akces.Unity.DataAccess.Migrations
{
    public partial class nexo_config_update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalNumberTemplate",
                table: "WarehouseConfigurationMember");

            migrationBuilder.AddColumn<bool>(
                name: "DefaultAddress",
                table: "NexoConfiguration",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ExternalNumberTemplate",
                table: "NexoConfiguration",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultAddress",
                table: "NexoConfiguration");

            migrationBuilder.DropColumn(
                name: "ExternalNumberTemplate",
                table: "NexoConfiguration");

            migrationBuilder.AddColumn<string>(
                name: "ExternalNumberTemplate",
                table: "WarehouseConfigurationMember",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
