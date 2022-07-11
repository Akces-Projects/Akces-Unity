using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Akces.Unity.DataAccess.Migrations
{
    public partial class init3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "UnityUsers");

            migrationBuilder.AddColumn<string>(
                name: "Login",
                table: "UnityUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "UnityUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "WorkerStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    Enabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerStatuses", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkerStatuses");

            migrationBuilder.DropColumn(
                name: "Login",
                table: "UnityUsers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "UnityUsers");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "UnityUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
