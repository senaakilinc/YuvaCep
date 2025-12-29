using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YuvaCep.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAnnouncementsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Announcements_Students_StudentId",
                table: "Announcements");

            migrationBuilder.DropForeignKey(
                name: "FK_Announcements_Users_TeacherId",
                table: "Announcements");

            migrationBuilder.DropIndex(
                name: "IX_Announcements_StudentId",
                table: "Announcements");

            migrationBuilder.DropIndex(
                name: "IX_Announcements_TeacherId",
                table: "Announcements");

            migrationBuilder.DropColumn(
                name: "NotificationSent",
                table: "Announcements");

            migrationBuilder.DropColumn(
                name: "RecipientCount",
                table: "Announcements");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "Announcements");

            migrationBuilder.DropColumn(
                name: "TargetAudience",
                table: "Announcements");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "Announcements");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Announcements",
                newName: "CreatedDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "Announcements",
                newName: "CreatedAt");

            migrationBuilder.AddColumn<bool>(
                name: "NotificationSent",
                table: "Announcements",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "RecipientCount",
                table: "Announcements",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StudentId",
                table: "Announcements",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TargetAudience",
                table: "Announcements",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "TeacherId",
                table: "Announcements",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_StudentId",
                table: "Announcements",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_TeacherId",
                table: "Announcements",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Announcements_Students_StudentId",
                table: "Announcements",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Announcements_Users_TeacherId",
                table: "Announcements",
                column: "TeacherId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
