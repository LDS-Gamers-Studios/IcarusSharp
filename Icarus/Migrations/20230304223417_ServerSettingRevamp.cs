using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Icarus.Migrations
{
    public partial class ServerSettingRevamp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ServerSettingId",
                table: "ServerSetting",
                newName: "ServerSettingValueId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ServerSettingValueId",
                table: "ServerSetting",
                newName: "ServerSettingId");
        }
    }
}
