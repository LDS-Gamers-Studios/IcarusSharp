using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Icarus.Migrations
{
    public partial class AdjustFlagInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flag_Member_FlaggedByMemberId",
                table: "Flag");

            migrationBuilder.DropForeignKey(
                name: "FK_Flag_Member_ResolvedByMemberId",
                table: "Flag");

            migrationBuilder.DropIndex(
                name: "IX_Flag_FlaggedByMemberId",
                table: "Flag");

            migrationBuilder.DropIndex(
                name: "IX_Flag_ResolvedByMemberId",
                table: "Flag");

            migrationBuilder.DropColumn(
                name: "FlaggedByMemberId",
                table: "Flag");

            migrationBuilder.DropColumn(
                name: "ResolvedByMemberId",
                table: "Flag");

            migrationBuilder.AddColumn<ulong>(
                name: "FlaggedById",
                table: "Flag",
                type: "bigint unsigned",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.AddColumn<ulong>(
                name: "ResolvedById",
                table: "Flag",
                type: "bigint unsigned",
                nullable: false,
                defaultValue: 0ul);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FlaggedById",
                table: "Flag");

            migrationBuilder.DropColumn(
                name: "ResolvedById",
                table: "Flag");

            migrationBuilder.AddColumn<int>(
                name: "FlaggedByMemberId",
                table: "Flag",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ResolvedByMemberId",
                table: "Flag",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Flag_FlaggedByMemberId",
                table: "Flag",
                column: "FlaggedByMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Flag_ResolvedByMemberId",
                table: "Flag",
                column: "ResolvedByMemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_Flag_Member_FlaggedByMemberId",
                table: "Flag",
                column: "FlaggedByMemberId",
                principalTable: "Member",
                principalColumn: "MemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_Flag_Member_ResolvedByMemberId",
                table: "Flag",
                column: "ResolvedByMemberId",
                principalTable: "Member",
                principalColumn: "MemberId");
        }
    }
}
