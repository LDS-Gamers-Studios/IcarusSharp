using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Icarus.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Member",
                columns: table => new
                {
                    MemberId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DiscordId = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    GuildedId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Member", x => x.MemberId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Filter",
                columns: table => new
                {
                    FilterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<int>(type: "int", nullable: false),
                    FilterText = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FilterTextConverted = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AddedByMemberId = table.Column<int>(type: "int", nullable: true),
                    AddTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filter", x => x.FilterId);
                    table.ForeignKey(
                        name: "FK_Filter_Member_AddedByMemberId",
                        column: x => x.AddedByMemberId,
                        principalTable: "Member",
                        principalColumn: "MemberId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Flag",
                columns: table => new
                {
                    FlagId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Time = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    FlaggedByMemberId = table.Column<int>(type: "int", nullable: true),
                    SourceMessageId = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    SourceChannelId = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    SourceUserId = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    SourcContent = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SourceMatches = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ResolvedByMemberId = table.Column<int>(type: "int", nullable: true),
                    ResolutionTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ResolutionType = table.Column<int>(type: "int", nullable: false),
                    ResolutionPoints = table.Column<int>(type: "int", nullable: false),
                    SystemMessage = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flag", x => x.FlagId);
                    table.ForeignKey(
                        name: "FK_Flag_Member_FlaggedByMemberId",
                        column: x => x.FlaggedByMemberId,
                        principalTable: "Member",
                        principalColumn: "MemberId");
                    table.ForeignKey(
                        name: "FK_Flag_Member_ResolvedByMemberId",
                        column: x => x.ResolvedByMemberId,
                        principalTable: "Member",
                        principalColumn: "MemberId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    TagId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Content = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AttachmentURL = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsEmbed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedByMemberId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.TagId);
                    table.ForeignKey(
                        name: "FK_Tag_Member_CreatedByMemberId",
                        column: x => x.CreatedByMemberId,
                        principalTable: "Member",
                        principalColumn: "MemberId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FilterException",
                columns: table => new
                {
                    FilterExceptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FilterId = table.Column<int>(type: "int", nullable: true),
                    ExceptionText = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AddedByMemberId = table.Column<int>(type: "int", nullable: true),
                    AddTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilterException", x => x.FilterExceptionId);
                    table.ForeignKey(
                        name: "FK_FilterException_Filter_FilterId",
                        column: x => x.FilterId,
                        principalTable: "Filter",
                        principalColumn: "FilterId");
                    table.ForeignKey(
                        name: "FK_FilterException_Member_AddedByMemberId",
                        column: x => x.AddedByMemberId,
                        principalTable: "Member",
                        principalColumn: "MemberId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Filter_AddedByMemberId",
                table: "Filter",
                column: "AddedByMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_FilterException_AddedByMemberId",
                table: "FilterException",
                column: "AddedByMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_FilterException_FilterId",
                table: "FilterException",
                column: "FilterId");

            migrationBuilder.CreateIndex(
                name: "IX_Flag_FlaggedByMemberId",
                table: "Flag",
                column: "FlaggedByMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Flag_ResolvedByMemberId",
                table: "Flag",
                column: "ResolvedByMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Tag_CreatedByMemberId",
                table: "Tag",
                column: "CreatedByMemberId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FilterException");

            migrationBuilder.DropTable(
                name: "Flag");

            migrationBuilder.DropTable(
                name: "Tag");

            migrationBuilder.DropTable(
                name: "Filter");

            migrationBuilder.DropTable(
                name: "Member");
        }
    }
}
