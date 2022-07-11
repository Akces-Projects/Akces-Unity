using Microsoft.EntityFrameworkCore.Migrations;

namespace Akces.Unity.DataAccess.Migrations
{
    public partial class init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authorisation_UnityUsers_UnityUserId",
                table: "Authorisation");

            migrationBuilder.AlterColumn<int>(
                name: "UnityUserId",
                table: "Authorisation",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Authorisation_UnityUsers_UnityUserId",
                table: "Authorisation",
                column: "UnityUserId",
                principalTable: "UnityUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authorisation_UnityUsers_UnityUserId",
                table: "Authorisation");

            migrationBuilder.AlterColumn<int>(
                name: "UnityUserId",
                table: "Authorisation",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Authorisation_UnityUsers_UnityUserId",
                table: "Authorisation",
                column: "UnityUserId",
                principalTable: "UnityUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
