using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YuvaCep.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddExtendedDailyReports : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyReports_Users_TeacherId",
                table: "DailyReports");

            migrationBuilder.DropIndex(
                name: "IX_DailyReports_TeacherId",
                table: "DailyReports");

            migrationBuilder.DropColumn(
                name: "ActivityNotes",
                table: "DailyReports");

            migrationBuilder.DropColumn(
                name: "ActivityType",
                table: "DailyReports");

            migrationBuilder.DropColumn(
                name: "AteWell",
                table: "DailyReports");

            migrationBuilder.DropColumn(
                name: "BehaviorNotes",
                table: "DailyReports");

            migrationBuilder.DropColumn(
                name: "BreakfastNotes",
                table: "DailyReports");

            migrationBuilder.DropColumn(
                name: "GeneralNotes",
                table: "DailyReports");

            migrationBuilder.DropColumn(
                name: "LunchNotes",
                table: "DailyReports");

            migrationBuilder.DropColumn(
                name: "MoodStatus",
                table: "DailyReports");

            migrationBuilder.DropColumn(
                name: "NapDurationMinutes",
                table: "DailyReports");

            migrationBuilder.DropColumn(
                name: "NapTaken",
                table: "DailyReports");

            migrationBuilder.DropColumn(
                name: "SnackNotes",
                table: "DailyReports");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "DailyReports");

            migrationBuilder.DropColumn(
                name: "ToiletUsed",
                table: "DailyReports");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "DailyReports");

            migrationBuilder.RenameColumn(
                name: "NutritionNotes",
                table: "DailyReports",
                newName: "TeacherNote");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "DailyReports",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "BehaviorScore",
                table: "DailyReports",
                newName: "Sleep");

            migrationBuilder.AddColumn<int>(
                name: "Activity",
                table: "DailyReports",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ActivityNote",
                table: "DailyReports",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Breakfast",
                table: "DailyReports",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FoodNote",
                table: "DailyReports",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Lunch",
                table: "DailyReports",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Mood",
                table: "DailyReports",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MoodNote",
                table: "DailyReports",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Activity",
                table: "DailyReports");

            migrationBuilder.DropColumn(
                name: "ActivityNote",
                table: "DailyReports");

            migrationBuilder.DropColumn(
                name: "Breakfast",
                table: "DailyReports");

            migrationBuilder.DropColumn(
                name: "FoodNote",
                table: "DailyReports");

            migrationBuilder.DropColumn(
                name: "Lunch",
                table: "DailyReports");

            migrationBuilder.DropColumn(
                name: "Mood",
                table: "DailyReports");

            migrationBuilder.DropColumn(
                name: "MoodNote",
                table: "DailyReports");

            migrationBuilder.RenameColumn(
                name: "TeacherNote",
                table: "DailyReports",
                newName: "NutritionNotes");

            migrationBuilder.RenameColumn(
                name: "Sleep",
                table: "DailyReports",
                newName: "BehaviorScore");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "DailyReports",
                newName: "CreatedAt");

            migrationBuilder.AddColumn<string>(
                name: "ActivityNotes",
                table: "DailyReports",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ActivityType",
                table: "DailyReports",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "AteWell",
                table: "DailyReports",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "BehaviorNotes",
                table: "DailyReports",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BreakfastNotes",
                table: "DailyReports",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GeneralNotes",
                table: "DailyReports",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LunchNotes",
                table: "DailyReports",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MoodStatus",
                table: "DailyReports",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "NapDurationMinutes",
                table: "DailyReports",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NapTaken",
                table: "DailyReports",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SnackNotes",
                table: "DailyReports",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "TeacherId",
                table: "DailyReports",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "ToiletUsed",
                table: "DailyReports",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "DailyReports",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DailyReports_TeacherId",
                table: "DailyReports",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyReports_Users_TeacherId",
                table: "DailyReports",
                column: "TeacherId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
