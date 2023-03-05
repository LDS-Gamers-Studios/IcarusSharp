using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Icarus.Migrations
{
    public partial class Revamp3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Filter_Member_AddedByMemberId",
                table: "Filter");

            migrationBuilder.DropForeignKey(
                name: "FK_FilterException_Member_AddedByMemberId",
                table: "FilterException");

            migrationBuilder.DropForeignKey(
                name: "FK_ServerSettingValue_Member_SetByMemberId",
                table: "ServerSettingValue");

            migrationBuilder.DropForeignKey(
                name: "FK_Tag_Member_CreatedByMemberId",
                table: "Tag");

            migrationBuilder.DropIndex(
                name: "IX_Tag_CreatedByMemberId",
                table: "Tag");

            migrationBuilder.DropIndex(
                name: "IX_ServerSettingValue_SetByMemberId",
                table: "ServerSettingValue");

            migrationBuilder.DropIndex(
                name: "IX_FilterException_AddedByMemberId",
                table: "FilterException");

            migrationBuilder.DropIndex(
                name: "IX_Filter_AddedByMemberId",
                table: "Filter");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "CreatedByMemberId",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "ServerSettingValue");

            migrationBuilder.DropColumn(
                name: "SetByMemberId",
                table: "ServerSettingValue");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "ServerSettingValue");

            migrationBuilder.DropColumn(
                name: "AddTime",
                table: "FilterException");

            migrationBuilder.DropColumn(
                name: "AddedByMemberId",
                table: "FilterException");

            migrationBuilder.DropColumn(
                name: "AddTime",
                table: "Filter");

            migrationBuilder.DropColumn(
                name: "AddedByMemberId",
                table: "Filter");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Tag",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CreatedByMemberId",
                table: "Tag",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "ServerSettingValue",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "SetByMemberId",
                table: "ServerSettingValue",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "ServerSettingValue",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "AddTime",
                table: "FilterException",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "AddedByMemberId",
                table: "FilterException",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AddTime",
                table: "Filter",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "AddedByMemberId",
                table: "Filter",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tag_CreatedByMemberId",
                table: "Tag",
                column: "CreatedByMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ServerSettingValue_SetByMemberId",
                table: "ServerSettingValue",
                column: "SetByMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_FilterException_AddedByMemberId",
                table: "FilterException",
                column: "AddedByMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Filter_AddedByMemberId",
                table: "Filter",
                column: "AddedByMemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_Filter_Member_AddedByMemberId",
                table: "Filter",
                column: "AddedByMemberId",
                principalTable: "Member",
                principalColumn: "MemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_FilterException_Member_AddedByMemberId",
                table: "FilterException",
                column: "AddedByMemberId",
                principalTable: "Member",
                principalColumn: "MemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServerSettingValue_Member_SetByMemberId",
                table: "ServerSettingValue",
                column: "SetByMemberId",
                principalTable: "Member",
                principalColumn: "MemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_Member_CreatedByMemberId",
                table: "Tag",
                column: "CreatedByMemberId",
                principalTable: "Member",
                principalColumn: "MemberId");
        }
    }
}
