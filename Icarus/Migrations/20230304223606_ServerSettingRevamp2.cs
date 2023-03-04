using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Icarus.Migrations
{
    public partial class ServerSettingRevamp2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServerSetting_Member_SetByMemberId",
                table: "ServerSetting");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServerSetting",
                table: "ServerSetting");

            migrationBuilder.RenameTable(
                name: "ServerSetting",
                newName: "ServerSettingValue");

            migrationBuilder.RenameIndex(
                name: "IX_ServerSetting_SetByMemberId",
                table: "ServerSettingValue",
                newName: "IX_ServerSettingValue_SetByMemberId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServerSettingValue",
                table: "ServerSettingValue",
                column: "ServerSettingValueId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServerSettingValue_Member_SetByMemberId",
                table: "ServerSettingValue",
                column: "SetByMemberId",
                principalTable: "Member",
                principalColumn: "MemberId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServerSettingValue_Member_SetByMemberId",
                table: "ServerSettingValue");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServerSettingValue",
                table: "ServerSettingValue");

            migrationBuilder.RenameTable(
                name: "ServerSettingValue",
                newName: "ServerSetting");

            migrationBuilder.RenameIndex(
                name: "IX_ServerSettingValue_SetByMemberId",
                table: "ServerSetting",
                newName: "IX_ServerSetting_SetByMemberId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServerSetting",
                table: "ServerSetting",
                column: "ServerSettingValueId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServerSetting_Member_SetByMemberId",
                table: "ServerSetting",
                column: "SetByMemberId",
                principalTable: "Member",
                principalColumn: "MemberId");
        }
    }
}
