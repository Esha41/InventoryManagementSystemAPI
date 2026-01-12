using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class LoginAttempts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LoginAttempts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    IsSuccessful = table.Column<bool>(type: "bit", nullable: false),
                    FailureReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AttemptDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LoginType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginAttempts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoginAttempts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 972, DateTimeKind.Unspecified).AddTicks(3016));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 972, DateTimeKind.Unspecified).AddTicks(3040));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 972, DateTimeKind.Unspecified).AddTicks(3041));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 972, DateTimeKind.Unspecified).AddTicks(3043));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 972, DateTimeKind.Unspecified).AddTicks(3044));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 972, DateTimeKind.Unspecified).AddTicks(7211));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 972, DateTimeKind.Unspecified).AddTicks(7228));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 972, DateTimeKind.Unspecified).AddTicks(7230));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 972, DateTimeKind.Unspecified).AddTicks(7231));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 972, DateTimeKind.Unspecified).AddTicks(7232));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 973, DateTimeKind.Unspecified).AddTicks(420));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 973, DateTimeKind.Unspecified).AddTicks(504));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 973, DateTimeKind.Unspecified).AddTicks(506));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 973, DateTimeKind.Unspecified).AddTicks(508));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 973, DateTimeKind.Unspecified).AddTicks(509));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 973, DateTimeKind.Unspecified).AddTicks(510));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 973, DateTimeKind.Unspecified).AddTicks(512));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 973, DateTimeKind.Unspecified).AddTicks(513));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 973, DateTimeKind.Unspecified).AddTicks(514));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 973, DateTimeKind.Unspecified).AddTicks(516));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 973, DateTimeKind.Unspecified).AddTicks(517));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 973, DateTimeKind.Unspecified).AddTicks(518));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 973, DateTimeKind.Unspecified).AddTicks(519));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 973, DateTimeKind.Unspecified).AddTicks(521));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 15L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 973, DateTimeKind.Unspecified).AddTicks(522));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 16L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 973, DateTimeKind.Unspecified).AddTicks(523));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 17L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 973, DateTimeKind.Unspecified).AddTicks(525));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 18L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 973, DateTimeKind.Unspecified).AddTicks(526));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 973, DateTimeKind.Unspecified).AddTicks(2182));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 973, DateTimeKind.Unspecified).AddTicks(2193));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 973, DateTimeKind.Unspecified).AddTicks(2195));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 973, DateTimeKind.Unspecified).AddTicks(2197));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 974, DateTimeKind.Unspecified).AddTicks(877));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 974, DateTimeKind.Unspecified).AddTicks(899));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 974, DateTimeKind.Unspecified).AddTicks(901));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 974, DateTimeKind.Unspecified).AddTicks(902));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 974, DateTimeKind.Unspecified).AddTicks(903));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 975, DateTimeKind.Unspecified).AddTicks(2326));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 975, DateTimeKind.Unspecified).AddTicks(2348));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 975, DateTimeKind.Unspecified).AddTicks(2350));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 975, DateTimeKind.Unspecified).AddTicks(2351));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 975, DateTimeKind.Unspecified).AddTicks(2352));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 975, DateTimeKind.Unspecified).AddTicks(3770));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 975, DateTimeKind.Unspecified).AddTicks(3779));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 975, DateTimeKind.Unspecified).AddTicks(3780));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 975, DateTimeKind.Unspecified).AddTicks(3782));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 975, DateTimeKind.Unspecified).AddTicks(3783));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 975, DateTimeKind.Unspecified).AddTicks(3784));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 975, DateTimeKind.Unspecified).AddTicks(3785));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 975, DateTimeKind.Unspecified).AddTicks(3786));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 975, DateTimeKind.Unspecified).AddTicks(3788));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 975, DateTimeKind.Unspecified).AddTicks(3789));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 975, DateTimeKind.Unspecified).AddTicks(3790));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 976, DateTimeKind.Unspecified).AddTicks(2584));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 976, DateTimeKind.Unspecified).AddTicks(2609));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 976, DateTimeKind.Unspecified).AddTicks(2611));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 976, DateTimeKind.Unspecified).AddTicks(2612));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 976, DateTimeKind.Unspecified).AddTicks(2614));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 976, DateTimeKind.Unspecified).AddTicks(4530));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 976, DateTimeKind.Unspecified).AddTicks(4541));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 976, DateTimeKind.Unspecified).AddTicks(4542));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 976, DateTimeKind.Unspecified).AddTicks(4544));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 976, DateTimeKind.Unspecified).AddTicks(4545));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 976, DateTimeKind.Unspecified).AddTicks(6038));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 976, DateTimeKind.Unspecified).AddTicks(6047));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 976, DateTimeKind.Unspecified).AddTicks(6049));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 976, DateTimeKind.Unspecified).AddTicks(6050));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 976, DateTimeKind.Unspecified).AddTicks(6051));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 976, DateTimeKind.Unspecified).AddTicks(7438));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 976, DateTimeKind.Unspecified).AddTicks(7447));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 976, DateTimeKind.Unspecified).AddTicks(7449));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 976, DateTimeKind.Unspecified).AddTicks(7450));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 976, DateTimeKind.Unspecified).AddTicks(7451));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 976, DateTimeKind.Unspecified).AddTicks(7453));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 976, DateTimeKind.Unspecified).AddTicks(7454));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 976, DateTimeKind.Unspecified).AddTicks(7455));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 976, DateTimeKind.Unspecified).AddTicks(7456));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 976, DateTimeKind.Unspecified).AddTicks(7457));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 977, DateTimeKind.Unspecified).AddTicks(1211));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 977, DateTimeKind.Unspecified).AddTicks(1231));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 977, DateTimeKind.Unspecified).AddTicks(1232));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 977, DateTimeKind.Unspecified).AddTicks(1234));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 977, DateTimeKind.Unspecified).AddTicks(1236));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 977, DateTimeKind.Unspecified).AddTicks(1237));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 977, DateTimeKind.Unspecified).AddTicks(1239));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 977, DateTimeKind.Unspecified).AddTicks(1240));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 977, DateTimeKind.Unspecified).AddTicks(1241));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 977, DateTimeKind.Unspecified).AddTicks(1242));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 977, DateTimeKind.Unspecified).AddTicks(1243));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 977, DateTimeKind.Unspecified).AddTicks(1235));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 977, DateTimeKind.Unspecified).AddTicks(3329));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 977, DateTimeKind.Unspecified).AddTicks(3347));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 977, DateTimeKind.Unspecified).AddTicks(3349));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 977, DateTimeKind.Unspecified).AddTicks(3350));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 977, DateTimeKind.Unspecified).AddTicks(3351));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 977, DateTimeKind.Unspecified).AddTicks(5073));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 977, DateTimeKind.Unspecified).AddTicks(5086));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 977, DateTimeKind.Unspecified).AddTicks(5088));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 977, DateTimeKind.Unspecified).AddTicks(5089));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 977, DateTimeKind.Unspecified).AddTicks(5090));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 978, DateTimeKind.Unspecified).AddTicks(1574));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 978, DateTimeKind.Unspecified).AddTicks(1593));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 978, DateTimeKind.Unspecified).AddTicks(1595));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 979, DateTimeKind.Unspecified).AddTicks(5987));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 979, DateTimeKind.Unspecified).AddTicks(6008));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 30, 13, 15, 12, 979, DateTimeKind.Unspecified).AddTicks(6010));

            migrationBuilder.CreateIndex(
                name: "IX_LoginAttempts_AttemptDate",
                table: "LoginAttempts",
                column: "AttemptDate");

            migrationBuilder.CreateIndex(
                name: "IX_LoginAttempts_UserId",
                table: "LoginAttempts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LoginAttempts_Username",
                table: "LoginAttempts",
                column: "Username");

            migrationBuilder.CreateIndex(
                name: "IX_LoginAttempts_Username_AttemptDate",
                table: "LoginAttempts",
                columns: new[] { "Username", "AttemptDate" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoginAttempts");

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 53, DateTimeKind.Unspecified).AddTicks(4891));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 53, DateTimeKind.Unspecified).AddTicks(4929));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 53, DateTimeKind.Unspecified).AddTicks(4932));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 53, DateTimeKind.Unspecified).AddTicks(4934));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 53, DateTimeKind.Unspecified).AddTicks(4936));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 54, DateTimeKind.Unspecified).AddTicks(1873));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 54, DateTimeKind.Unspecified).AddTicks(1900));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 54, DateTimeKind.Unspecified).AddTicks(1903));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 54, DateTimeKind.Unspecified).AddTicks(1905));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 54, DateTimeKind.Unspecified).AddTicks(1908));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 54, DateTimeKind.Unspecified).AddTicks(6880));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 54, DateTimeKind.Unspecified).AddTicks(6906));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 54, DateTimeKind.Unspecified).AddTicks(6909));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 54, DateTimeKind.Unspecified).AddTicks(6912));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 54, DateTimeKind.Unspecified).AddTicks(6915));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 54, DateTimeKind.Unspecified).AddTicks(6918));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 54, DateTimeKind.Unspecified).AddTicks(6920));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 54, DateTimeKind.Unspecified).AddTicks(6922));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 54, DateTimeKind.Unspecified).AddTicks(6925));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 54, DateTimeKind.Unspecified).AddTicks(6927));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 54, DateTimeKind.Unspecified).AddTicks(6930));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 54, DateTimeKind.Unspecified).AddTicks(6932));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 54, DateTimeKind.Unspecified).AddTicks(6934));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 54, DateTimeKind.Unspecified).AddTicks(6937));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 15L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 54, DateTimeKind.Unspecified).AddTicks(6939));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 16L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 54, DateTimeKind.Unspecified).AddTicks(6941));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 17L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 54, DateTimeKind.Unspecified).AddTicks(6944));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 18L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 54, DateTimeKind.Unspecified).AddTicks(6946));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 54, DateTimeKind.Unspecified).AddTicks(9438));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 54, DateTimeKind.Unspecified).AddTicks(9457));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 54, DateTimeKind.Unspecified).AddTicks(9460));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 54, DateTimeKind.Unspecified).AddTicks(9463));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 56, DateTimeKind.Unspecified).AddTicks(3260));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 56, DateTimeKind.Unspecified).AddTicks(3293));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 56, DateTimeKind.Unspecified).AddTicks(3296));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 56, DateTimeKind.Unspecified).AddTicks(3298));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 56, DateTimeKind.Unspecified).AddTicks(3301));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 57, DateTimeKind.Unspecified).AddTicks(7747));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 57, DateTimeKind.Unspecified).AddTicks(7783));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 57, DateTimeKind.Unspecified).AddTicks(7786));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 57, DateTimeKind.Unspecified).AddTicks(7789));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 57, DateTimeKind.Unspecified).AddTicks(7792));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 57, DateTimeKind.Unspecified).AddTicks(9922));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 57, DateTimeKind.Unspecified).AddTicks(9936));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 57, DateTimeKind.Unspecified).AddTicks(9939));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 57, DateTimeKind.Unspecified).AddTicks(9942));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 57, DateTimeKind.Unspecified).AddTicks(9944));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 57, DateTimeKind.Unspecified).AddTicks(9947));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 57, DateTimeKind.Unspecified).AddTicks(9949));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 57, DateTimeKind.Unspecified).AddTicks(9951));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 57, DateTimeKind.Unspecified).AddTicks(9953));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 57, DateTimeKind.Unspecified).AddTicks(9955));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 57, DateTimeKind.Unspecified).AddTicks(9957));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 59, DateTimeKind.Unspecified).AddTicks(1241));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 59, DateTimeKind.Unspecified).AddTicks(1271));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 59, DateTimeKind.Unspecified).AddTicks(1274));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 59, DateTimeKind.Unspecified).AddTicks(1277));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 59, DateTimeKind.Unspecified).AddTicks(1279));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 59, DateTimeKind.Unspecified).AddTicks(3416));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 59, DateTimeKind.Unspecified).AddTicks(3431));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 59, DateTimeKind.Unspecified).AddTicks(3434));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 59, DateTimeKind.Unspecified).AddTicks(3437));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 59, DateTimeKind.Unspecified).AddTicks(3439));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 59, DateTimeKind.Unspecified).AddTicks(5293));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 59, DateTimeKind.Unspecified).AddTicks(5304));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 59, DateTimeKind.Unspecified).AddTicks(5306));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 59, DateTimeKind.Unspecified).AddTicks(5308));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 59, DateTimeKind.Unspecified).AddTicks(5310));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 59, DateTimeKind.Unspecified).AddTicks(7126));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 59, DateTimeKind.Unspecified).AddTicks(7143));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 59, DateTimeKind.Unspecified).AddTicks(7145));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 59, DateTimeKind.Unspecified).AddTicks(7147));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 59, DateTimeKind.Unspecified).AddTicks(7149));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 59, DateTimeKind.Unspecified).AddTicks(7151));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 59, DateTimeKind.Unspecified).AddTicks(7153));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 59, DateTimeKind.Unspecified).AddTicks(7154));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 59, DateTimeKind.Unspecified).AddTicks(7156));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 59, DateTimeKind.Unspecified).AddTicks(7158));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 60, DateTimeKind.Unspecified).AddTicks(2031));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 60, DateTimeKind.Unspecified).AddTicks(2055));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 60, DateTimeKind.Unspecified).AddTicks(2057));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 60, DateTimeKind.Unspecified).AddTicks(2059));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 60, DateTimeKind.Unspecified).AddTicks(2063));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 60, DateTimeKind.Unspecified).AddTicks(2065));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 60, DateTimeKind.Unspecified).AddTicks(2067));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 60, DateTimeKind.Unspecified).AddTicks(2069));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 60, DateTimeKind.Unspecified).AddTicks(2071));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 60, DateTimeKind.Unspecified).AddTicks(2073));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 60, DateTimeKind.Unspecified).AddTicks(2074));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 60, DateTimeKind.Unspecified).AddTicks(2062));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 60, DateTimeKind.Unspecified).AddTicks(4687));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 60, DateTimeKind.Unspecified).AddTicks(4710));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 60, DateTimeKind.Unspecified).AddTicks(4712));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 60, DateTimeKind.Unspecified).AddTicks(4714));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 60, DateTimeKind.Unspecified).AddTicks(4717));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 60, DateTimeKind.Unspecified).AddTicks(6534));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 60, DateTimeKind.Unspecified).AddTicks(6547));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 60, DateTimeKind.Unspecified).AddTicks(6549));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 60, DateTimeKind.Unspecified).AddTicks(6552));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 60, DateTimeKind.Unspecified).AddTicks(6554));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 61, DateTimeKind.Unspecified).AddTicks(4613));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 61, DateTimeKind.Unspecified).AddTicks(4641));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 61, DateTimeKind.Unspecified).AddTicks(4643));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 63, DateTimeKind.Unspecified).AddTicks(3912));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 63, DateTimeKind.Unspecified).AddTicks(3939));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 28, 16, 12, 26, 63, DateTimeKind.Unspecified).AddTicks(3941));
        }
    }
}
