using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YuvaCep.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddGenericBadgeSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentBadges_Badges_BadgeId",
                table: "StudentBadges");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentBadges_Students_StudentId",
                table: "StudentBadges");

            migrationBuilder.DropTable(
                name: "Badges");

            migrationBuilder.DropIndex(
                name: "IX_StudentBadges_StudentId",
                table: "StudentBadges");

            migrationBuilder.RenameColumn(
                name: "EarnedAt",
                table: "StudentBadges",
                newName: "EarnedDate");

            migrationBuilder.RenameColumn(
                name: "BadgeId",
                table: "StudentBadges",
                newName: "BadgeDefinitionId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentBadges_BadgeId",
                table: "StudentBadges",
                newName: "IX_StudentBadges_BadgeDefinitionId");

            migrationBuilder.CreateTable(
                name: "BadgeDefinitions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: false),
                    TargetCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BadgeDefinitions", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_StudentBadges_BadgeDefinitions_BadgeDefinitionId",
                table: "StudentBadges",
                column: "BadgeDefinitionId",
                principalTable: "BadgeDefinitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentBadges_BadgeDefinitions_BadgeDefinitionId",
                table: "StudentBadges");

            migrationBuilder.DropTable(
                name: "BadgeDefinitions");

            migrationBuilder.RenameColumn(
                name: "EarnedDate",
                table: "StudentBadges",
                newName: "EarnedAt");

            migrationBuilder.RenameColumn(
                name: "BadgeDefinitionId",
                table: "StudentBadges",
                newName: "BadgeId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentBadges_BadgeDefinitionId",
                table: "StudentBadges",
                newName: "IX_StudentBadges_BadgeId");

            migrationBuilder.CreateTable(
                name: "Badges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    ImagePath = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Badges", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentBadges_StudentId",
                table: "StudentBadges",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentBadges_Badges_BadgeId",
                table: "StudentBadges",
                column: "BadgeId",
                principalTable: "Badges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentBadges_Students_StudentId",
                table: "StudentBadges",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
