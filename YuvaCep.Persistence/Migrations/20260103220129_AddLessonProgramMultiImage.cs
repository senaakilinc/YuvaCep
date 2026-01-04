using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YuvaCep.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddLessonProgramMultiImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LessonPrograms_Classes_ClassId",
                table: "LessonPrograms");

            migrationBuilder.DropTable(
                name: "MonthlyPlans");

            migrationBuilder.DropIndex(
                name: "IX_LessonPrograms_ClassId",
                table: "LessonPrograms");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "LessonPrograms");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "LessonPrograms");

            migrationBuilder.AddColumn<int>(
                name: "Month",
                table: "LessonPrograms",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "LessonPrograms",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "LessonProgramImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LessonProgramId = table.Column<Guid>(type: "uuid", nullable: false),
                    ImageBase64 = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonProgramImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LessonProgramImages_LessonPrograms_LessonProgramId",
                        column: x => x.LessonProgramId,
                        principalTable: "LessonPrograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LessonProgramImages_LessonProgramId",
                table: "LessonProgramImages",
                column: "LessonProgramId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LessonProgramImages");

            migrationBuilder.DropColumn(
                name: "Month",
                table: "LessonPrograms");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "LessonPrograms");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "LessonPrograms",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "LessonPrograms",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "MonthlyPlans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClassId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ImageUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Month = table.Column<int>(type: "integer", nullable: false),
                    PlanType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Year = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlyPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MonthlyPlans_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LessonPrograms_ClassId",
                table: "LessonPrograms",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyPlans_ClassId",
                table: "MonthlyPlans",
                column: "ClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_LessonPrograms_Classes_ClassId",
                table: "LessonPrograms",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
