using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace YuvaCep.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class YeniSeedPaketi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParentStudents_Parents_ParentId",
                table: "ParentStudents");

            migrationBuilder.DropForeignKey(
                name: "FK_ParentStudents_Students_StudentId",
                table: "ParentStudents");

            migrationBuilder.DropTable(
                name: "ParentMessages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ParentStudents",
                table: "ParentStudents");

            migrationBuilder.RenameTable(
                name: "ParentStudents",
                newName: "ParentStudent");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "DailyReports",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_ParentStudents_StudentId",
                table: "ParentStudent",
                newName: "IX_ParentStudent_StudentId");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PasswordSalt",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReferenceCode",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TCIDNumber",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TCIDNumber",
                table: "Students",
                type: "text",
                nullable: false,
                defaultValue: "");

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

            migrationBuilder.AlterColumn<string>(
                name: "YearLevel",
                table: "Classes",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ParentStudent",
                table: "ParentStudent",
                columns: new[] { "ParentId", "StudentId" });

            migrationBuilder.CreateTable(
                name: "Feedbacks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    FeedBackType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Subject = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Content = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    Priority = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    TeacherResponse = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    RespondedByTeacherId = table.Column<int>(type: "integer", nullable: true),
                    TeacherId = table.Column<Guid>(type: "uuid", nullable: false),
                    RespondedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedbacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Feedbacks_Parents_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Parents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Feedbacks_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Feedbacks_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MonthlyPlans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClassId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    Month = table.Column<int>(type: "integer", nullable: false),
                    PlanType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ImageUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
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

            migrationBuilder.InsertData(
                table: "Badges",
                columns: new[] { "Id", "Code", "ImagePath", "Name" },
                values: new object[,]
                {
                    { new Guid("77777777-7777-7777-7777-777777777777"), "WEEKLY", "badges/weekly.png", "Haftanın Yıldızı" },
                    { new Guid("88888888-8888-8888-8888-888888888888"), "GOLD", "badges/gold.png", "Süper Başarı" },
                    { new Guid("99999999-9999-9999-9999-999999999999"), "SILVER", "badges/silver.png", "Örnek Davranış" }
                });

            migrationBuilder.InsertData(
                table: "Classes",
                columns: new[] { "Id", "MaxCapacity", "Name", "YearLevel" },
                values: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), 0, "Papatyalar Sınıfı", "4-5 Yaş Grubu" });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "ClassId", "DateOfBirth", "HealthNotes", "Name", "ReferenceCode", "Surname", "TCIDNumber" },
                values: new object[,]
                {
                    { new Guid("22222222-2222-2222-2222-222222222222"), new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Ali Yılmaz", "", "", "11111111111" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Ayşe Demir", "", "", "22222222222" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_ParentId",
                table: "Feedbacks",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_StudentId",
                table: "Feedbacks",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_TeacherId",
                table: "Feedbacks",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyPlans_ClassId",
                table: "MonthlyPlans",
                column: "ClassId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParentStudent_Parents_ParentId",
                table: "ParentStudent");

            migrationBuilder.DropForeignKey(
                name: "FK_ParentStudent_Students_StudentId",
                table: "ParentStudent");

            migrationBuilder.DropTable(
                name: "Feedbacks");

            migrationBuilder.DropTable(
                name: "MonthlyPlans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ParentStudent",
                table: "ParentStudent");

            migrationBuilder.DeleteData(
                table: "Badges",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"));

            migrationBuilder.DeleteData(
                table: "Badges",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"));

            migrationBuilder.DeleteData(
                table: "Badges",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"));

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ReferenceCode",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TCIDNumber",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TCIDNumber",
                table: "Students");

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
                name: "ToiletUsed",
                table: "DailyReports");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "DailyReports");

            migrationBuilder.RenameTable(
                name: "ParentStudent",
                newName: "ParentStudents");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "DailyReports",
                newName: "Date");

            migrationBuilder.RenameIndex(
                name: "IX_ParentStudent_StudentId",
                table: "ParentStudents",
                newName: "IX_ParentStudents_StudentId");

            migrationBuilder.AlterColumn<int>(
                name: "YearLevel",
                table: "Classes",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ParentStudents",
                table: "ParentStudents",
                columns: new[] { "ParentId", "StudentId" });

            migrationBuilder.CreateTable(
                name: "ParentMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ReceiverTeacherId = table.Column<Guid>(type: "uuid", nullable: false),
                    SenderParentId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    IsRead = table.Column<bool>(type: "boolean", nullable: false),
                    SentAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParentMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParentMessages_Parents_SenderParentId",
                        column: x => x.SenderParentId,
                        principalTable: "Parents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentMessages_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentMessages_Teachers_ReceiverTeacherId",
                        column: x => x.ReceiverTeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ParentMessages_ReceiverTeacherId",
                table: "ParentMessages",
                column: "ReceiverTeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentMessages_SenderParentId",
                table: "ParentMessages",
                column: "SenderParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentMessages_StudentId",
                table: "ParentMessages",
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
    }
}
