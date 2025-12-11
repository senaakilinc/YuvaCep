using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YuvaCep.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddGamificationSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParentStudent_Parents_ParentId",
                table: "ParentStudent");

            migrationBuilder.DropForeignKey(
                name: "FK_ParentStudent_Students_StudentId",
                table: "ParentStudent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ParentStudent",
                table: "ParentStudent");

            migrationBuilder.RenameTable(
                name: "ParentStudent",
                newName: "ParentStudents");

            migrationBuilder.RenameColumn(
                name: "TC_IdNumber",
                table: "Teachers",
                newName: "TCIDNumber");

            migrationBuilder.RenameColumn(
                name: "TC_IdNumber",
                table: "Parents",
                newName: "TCIDNumber");

            migrationBuilder.RenameIndex(
                name: "IX_ParentStudent_StudentId",
                table: "ParentStudents",
                newName: "IX_ParentStudents_StudentId");

            migrationBuilder.AlterColumn<string>(
                name: "HealthNotes",
                table: "Students",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Surname",
                table: "Students",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Parents",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ParentStudents",
                table: "ParentStudents",
                columns: new[] { "ParentId", "StudentId" });

            migrationBuilder.CreateTable(
                name: "Badges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ImagePath = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Badges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudentMoods",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Emoji = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentMoods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentMoods_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentBadges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EarnedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    BadgeId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentBadges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentBadges_Badges_BadgeId",
                        column: x => x.BadgeId,
                        principalTable: "Badges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentBadges_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentBadges_BadgeId",
                table: "StudentBadges",
                column: "BadgeId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentBadges_StudentId",
                table: "StudentBadges",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentMoods_StudentId",
                table: "StudentMoods",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ParentStudents_Parents_ParentId",
                table: "ParentStudents",
                column: "ParentId",
                principalTable: "Parents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ParentStudents_Students_StudentId",
                table: "ParentStudents",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParentStudents_Parents_ParentId",
                table: "ParentStudents");

            migrationBuilder.DropForeignKey(
                name: "FK_ParentStudents_Students_StudentId",
                table: "ParentStudents");

            migrationBuilder.DropTable(
                name: "StudentBadges");

            migrationBuilder.DropTable(
                name: "StudentMoods");

            migrationBuilder.DropTable(
                name: "Badges");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ParentStudents",
                table: "ParentStudents");

            migrationBuilder.DropColumn(
                name: "Surname",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Parents");

            migrationBuilder.RenameTable(
                name: "ParentStudents",
                newName: "ParentStudent");

            migrationBuilder.RenameColumn(
                name: "TCIDNumber",
                table: "Teachers",
                newName: "TC_IdNumber");

            migrationBuilder.RenameColumn(
                name: "TCIDNumber",
                table: "Parents",
                newName: "TC_IdNumber");

            migrationBuilder.RenameIndex(
                name: "IX_ParentStudents_StudentId",
                table: "ParentStudent",
                newName: "IX_ParentStudent_StudentId");

            migrationBuilder.AlterColumn<string>(
                name: "HealthNotes",
                table: "Students",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ParentStudent",
                table: "ParentStudent",
                columns: new[] { "ParentId", "StudentId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ParentStudent_Parents_ParentId",
                table: "ParentStudent",
                column: "ParentId",
                principalTable: "Parents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ParentStudent_Students_StudentId",
                table: "ParentStudent",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
