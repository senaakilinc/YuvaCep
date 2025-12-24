using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YuvaCep.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Announcements_Classes_TargetClassId",
                table: "Announcements");

            migrationBuilder.RenameColumn(
                name: "Surname",
                table: "Teachers",
                newName: "PasswordSalt");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Teachers",
                newName: "PasswordHash");

            migrationBuilder.RenameColumn(
                name: "Surname",
                table: "Parents",
                newName: "ReferenceCode");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Parents",
                newName: "PasswordSalt");

            migrationBuilder.RenameColumn(
                name: "TargetClassId",
                table: "Announcements",
                newName: "StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_Announcements_TargetClassId",
                table: "Announcements",
                newName: "IX_Announcements_StudentId");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Teachers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "HireDate",
                table: "Teachers",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Teachers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Teachers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "Teachers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Parents",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Parents",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Parents",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Parents",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "Parents",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "RespondedByTeacherId",
                table: "Feedbacks",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ClassId",
                table: "Announcements",
                type: "uuid",
                nullable: true);

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
                name: "IX_Announcements_ClassId",
                table: "Announcements",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_TeacherId",
                table: "Announcements",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Announcements_Classes_ClassId",
                table: "Announcements",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Announcements_Students_StudentId",
                table: "Announcements",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Announcements_Teachers_TeacherId",
                table: "Announcements",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Announcements_Classes_ClassId",
                table: "Announcements");

            migrationBuilder.DropForeignKey(
                name: "FK_Announcements_Students_StudentId",
                table: "Announcements");

            migrationBuilder.DropForeignKey(
                name: "FK_Announcements_Teachers_TeacherId",
                table: "Announcements");

            migrationBuilder.DropIndex(
                name: "IX_Announcements_ClassId",
                table: "Announcements");

            migrationBuilder.DropIndex(
                name: "IX_Announcements_TeacherId",
                table: "Announcements");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "HireDate",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Parents");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Parents");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Parents");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Parents");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Parents");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "Announcements");

            migrationBuilder.DropColumn(
                name: "NotificationSent",
                table: "Announcements");

            migrationBuilder.DropColumn(
                name: "RecipientCount",
                table: "Announcements");

            migrationBuilder.DropColumn(
                name: "TargetAudience",
                table: "Announcements");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "Announcements");

            migrationBuilder.RenameColumn(
                name: "PasswordSalt",
                table: "Teachers",
                newName: "Surname");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Teachers",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "ReferenceCode",
                table: "Parents",
                newName: "Surname");

            migrationBuilder.RenameColumn(
                name: "PasswordSalt",
                table: "Parents",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "Announcements",
                newName: "TargetClassId");

            migrationBuilder.RenameIndex(
                name: "IX_Announcements_StudentId",
                table: "Announcements",
                newName: "IX_Announcements_TargetClassId");

            migrationBuilder.AlterColumn<int>(
                name: "RespondedByTeacherId",
                table: "Feedbacks",
                type: "integer",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Announcements_Classes_TargetClassId",
                table: "Announcements",
                column: "TargetClassId",
                principalTable: "Classes",
                principalColumn: "Id");
        }
    }
}
