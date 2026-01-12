using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class BlacklistedTokensTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlacklistedTokens",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TokenId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    BlacklistedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlacklistedTokens", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 732, DateTimeKind.Unspecified).AddTicks(3039));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 732, DateTimeKind.Unspecified).AddTicks(3089));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 732, DateTimeKind.Unspecified).AddTicks(3090));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 732, DateTimeKind.Unspecified).AddTicks(3092));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 732, DateTimeKind.Unspecified).AddTicks(3093));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 733, DateTimeKind.Unspecified).AddTicks(3163));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 733, DateTimeKind.Unspecified).AddTicks(3200));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 733, DateTimeKind.Unspecified).AddTicks(3203));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 733, DateTimeKind.Unspecified).AddTicks(3204));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 733, DateTimeKind.Unspecified).AddTicks(3205));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 733, DateTimeKind.Unspecified).AddTicks(7465));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 733, DateTimeKind.Unspecified).AddTicks(7489));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 733, DateTimeKind.Unspecified).AddTicks(7491));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 733, DateTimeKind.Unspecified).AddTicks(7493));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 733, DateTimeKind.Unspecified).AddTicks(7520));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 733, DateTimeKind.Unspecified).AddTicks(7521));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 733, DateTimeKind.Unspecified).AddTicks(7523));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 733, DateTimeKind.Unspecified).AddTicks(7525));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 733, DateTimeKind.Unspecified).AddTicks(7526));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 733, DateTimeKind.Unspecified).AddTicks(7528));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 733, DateTimeKind.Unspecified).AddTicks(7542));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 733, DateTimeKind.Unspecified).AddTicks(7578));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 733, DateTimeKind.Unspecified).AddTicks(7581));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 733, DateTimeKind.Unspecified).AddTicks(7583));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 15L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 733, DateTimeKind.Unspecified).AddTicks(7584));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 16L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 733, DateTimeKind.Unspecified).AddTicks(7585));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 17L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 733, DateTimeKind.Unspecified).AddTicks(7587));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 18L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 733, DateTimeKind.Unspecified).AddTicks(7588));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 733, DateTimeKind.Unspecified).AddTicks(9983));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 734, DateTimeKind.Unspecified).AddTicks(2));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 734, DateTimeKind.Unspecified).AddTicks(8));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 734, DateTimeKind.Unspecified).AddTicks(10));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 735, DateTimeKind.Unspecified).AddTicks(1368));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 735, DateTimeKind.Unspecified).AddTicks(1395));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 735, DateTimeKind.Unspecified).AddTicks(1397));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 735, DateTimeKind.Unspecified).AddTicks(1398));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 735, DateTimeKind.Unspecified).AddTicks(1400));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 736, DateTimeKind.Unspecified).AddTicks(6171));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 736, DateTimeKind.Unspecified).AddTicks(6197));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 736, DateTimeKind.Unspecified).AddTicks(6199));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 736, DateTimeKind.Unspecified).AddTicks(6201));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 736, DateTimeKind.Unspecified).AddTicks(6202));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 736, DateTimeKind.Unspecified).AddTicks(8277));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 736, DateTimeKind.Unspecified).AddTicks(8294));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 736, DateTimeKind.Unspecified).AddTicks(8295));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 736, DateTimeKind.Unspecified).AddTicks(8297));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 736, DateTimeKind.Unspecified).AddTicks(8298));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 736, DateTimeKind.Unspecified).AddTicks(8300));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 736, DateTimeKind.Unspecified).AddTicks(8301));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 736, DateTimeKind.Unspecified).AddTicks(8302));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 736, DateTimeKind.Unspecified).AddTicks(8304));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 736, DateTimeKind.Unspecified).AddTicks(8305));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 736, DateTimeKind.Unspecified).AddTicks(8306));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 737, DateTimeKind.Unspecified).AddTicks(7988));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 737, DateTimeKind.Unspecified).AddTicks(8015));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 737, DateTimeKind.Unspecified).AddTicks(8017));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 737, DateTimeKind.Unspecified).AddTicks(8019));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 737, DateTimeKind.Unspecified).AddTicks(8020));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 738, DateTimeKind.Unspecified).AddTicks(14));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 738, DateTimeKind.Unspecified).AddTicks(32));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 738, DateTimeKind.Unspecified).AddTicks(34));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 738, DateTimeKind.Unspecified).AddTicks(35));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 738, DateTimeKind.Unspecified).AddTicks(36));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 738, DateTimeKind.Unspecified).AddTicks(1875));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 738, DateTimeKind.Unspecified).AddTicks(1889));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 738, DateTimeKind.Unspecified).AddTicks(1891));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 738, DateTimeKind.Unspecified).AddTicks(1892));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 738, DateTimeKind.Unspecified).AddTicks(1894));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 738, DateTimeKind.Unspecified).AddTicks(3579));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 738, DateTimeKind.Unspecified).AddTicks(3591));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 738, DateTimeKind.Unspecified).AddTicks(3592));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 738, DateTimeKind.Unspecified).AddTicks(3594));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 738, DateTimeKind.Unspecified).AddTicks(3595));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 738, DateTimeKind.Unspecified).AddTicks(3596));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 738, DateTimeKind.Unspecified).AddTicks(3597));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 738, DateTimeKind.Unspecified).AddTicks(3599));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 738, DateTimeKind.Unspecified).AddTicks(3600));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 738, DateTimeKind.Unspecified).AddTicks(3601));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 738, DateTimeKind.Unspecified).AddTicks(8241));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 738, DateTimeKind.Unspecified).AddTicks(8268));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 738, DateTimeKind.Unspecified).AddTicks(8270));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 738, DateTimeKind.Unspecified).AddTicks(8272));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 738, DateTimeKind.Unspecified).AddTicks(8274));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 738, DateTimeKind.Unspecified).AddTicks(8276));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 738, DateTimeKind.Unspecified).AddTicks(8277));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 738, DateTimeKind.Unspecified).AddTicks(8278));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 738, DateTimeKind.Unspecified).AddTicks(8280));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 738, DateTimeKind.Unspecified).AddTicks(8281));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 738, DateTimeKind.Unspecified).AddTicks(8282));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 738, DateTimeKind.Unspecified).AddTicks(8273));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 739, DateTimeKind.Unspecified).AddTicks(673));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 739, DateTimeKind.Unspecified).AddTicks(691));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 739, DateTimeKind.Unspecified).AddTicks(693));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 739, DateTimeKind.Unspecified).AddTicks(694));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 739, DateTimeKind.Unspecified).AddTicks(696));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 739, DateTimeKind.Unspecified).AddTicks(2390));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 739, DateTimeKind.Unspecified).AddTicks(2403));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 739, DateTimeKind.Unspecified).AddTicks(2405));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 739, DateTimeKind.Unspecified).AddTicks(2406));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 739, DateTimeKind.Unspecified).AddTicks(2408));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 739, DateTimeKind.Unspecified).AddTicks(9645));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 739, DateTimeKind.Unspecified).AddTicks(9670));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 739, DateTimeKind.Unspecified).AddTicks(9672));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 741, DateTimeKind.Unspecified).AddTicks(7992));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 741, DateTimeKind.Unspecified).AddTicks(8020));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 18, 59, 1, 741, DateTimeKind.Unspecified).AddTicks(8022));

            migrationBuilder.CreateIndex(
                name: "IX_BlacklistedTokens_ExpiresAt",
                table: "BlacklistedTokens",
                column: "ExpiresAt");

            migrationBuilder.CreateIndex(
                name: "IX_BlacklistedTokens_TokenId",
                table: "BlacklistedTokens",
                column: "TokenId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlacklistedTokens_UserId",
                table: "BlacklistedTokens",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlacklistedTokens");

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 944, DateTimeKind.Unspecified).AddTicks(2361));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 944, DateTimeKind.Unspecified).AddTicks(2389));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 944, DateTimeKind.Unspecified).AddTicks(2391));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 944, DateTimeKind.Unspecified).AddTicks(2393));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 944, DateTimeKind.Unspecified).AddTicks(2394));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 944, DateTimeKind.Unspecified).AddTicks(7153));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 944, DateTimeKind.Unspecified).AddTicks(7175));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 944, DateTimeKind.Unspecified).AddTicks(7177));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 944, DateTimeKind.Unspecified).AddTicks(7178));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 944, DateTimeKind.Unspecified).AddTicks(7180));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 945, DateTimeKind.Unspecified).AddTicks(910));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 945, DateTimeKind.Unspecified).AddTicks(931));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 945, DateTimeKind.Unspecified).AddTicks(932));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 945, DateTimeKind.Unspecified).AddTicks(934));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 945, DateTimeKind.Unspecified).AddTicks(935));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 945, DateTimeKind.Unspecified).AddTicks(936));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 945, DateTimeKind.Unspecified).AddTicks(938));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 945, DateTimeKind.Unspecified).AddTicks(939));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 945, DateTimeKind.Unspecified).AddTicks(940));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 945, DateTimeKind.Unspecified).AddTicks(942));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 945, DateTimeKind.Unspecified).AddTicks(943));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 945, DateTimeKind.Unspecified).AddTicks(944));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 945, DateTimeKind.Unspecified).AddTicks(945));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 945, DateTimeKind.Unspecified).AddTicks(947));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 15L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 945, DateTimeKind.Unspecified).AddTicks(948));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 16L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 945, DateTimeKind.Unspecified).AddTicks(949));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 17L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 945, DateTimeKind.Unspecified).AddTicks(950));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 18L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 945, DateTimeKind.Unspecified).AddTicks(952));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 945, DateTimeKind.Unspecified).AddTicks(2911));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 945, DateTimeKind.Unspecified).AddTicks(2929));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 945, DateTimeKind.Unspecified).AddTicks(2932));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 945, DateTimeKind.Unspecified).AddTicks(2934));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 946, DateTimeKind.Unspecified).AddTicks(3584));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 946, DateTimeKind.Unspecified).AddTicks(3607));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 946, DateTimeKind.Unspecified).AddTicks(3608));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 946, DateTimeKind.Unspecified).AddTicks(3609));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 946, DateTimeKind.Unspecified).AddTicks(3611));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 947, DateTimeKind.Unspecified).AddTicks(6963));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 947, DateTimeKind.Unspecified).AddTicks(6986));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 947, DateTimeKind.Unspecified).AddTicks(6988));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 947, DateTimeKind.Unspecified).AddTicks(6990));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 947, DateTimeKind.Unspecified).AddTicks(6991));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 947, DateTimeKind.Unspecified).AddTicks(8597));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 947, DateTimeKind.Unspecified).AddTicks(8610));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 947, DateTimeKind.Unspecified).AddTicks(8612));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 947, DateTimeKind.Unspecified).AddTicks(8613));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 947, DateTimeKind.Unspecified).AddTicks(8614));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 947, DateTimeKind.Unspecified).AddTicks(8616));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 947, DateTimeKind.Unspecified).AddTicks(8617));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 947, DateTimeKind.Unspecified).AddTicks(8618));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 947, DateTimeKind.Unspecified).AddTicks(8619));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 947, DateTimeKind.Unspecified).AddTicks(8620));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 947, DateTimeKind.Unspecified).AddTicks(8621));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 948, DateTimeKind.Unspecified).AddTicks(7528));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 948, DateTimeKind.Unspecified).AddTicks(7552));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 948, DateTimeKind.Unspecified).AddTicks(7554));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 948, DateTimeKind.Unspecified).AddTicks(7555));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 948, DateTimeKind.Unspecified).AddTicks(7557));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 948, DateTimeKind.Unspecified).AddTicks(9155));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 948, DateTimeKind.Unspecified).AddTicks(9169));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 948, DateTimeKind.Unspecified).AddTicks(9170));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 948, DateTimeKind.Unspecified).AddTicks(9172));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 948, DateTimeKind.Unspecified).AddTicks(9173));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(758));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(771));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(773));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(774));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(776));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(2368));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(2379));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(2380));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(2382));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(2383));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(2384));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(2385));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(2387));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(2388));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(2389));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(6494));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(6518));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(6520));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(6521));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(6580));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(6582));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(6583));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(6584));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(6585));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(6586));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(6588));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(6523));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(8842));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(8859));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(8861));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(8863));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(8864));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 950, DateTimeKind.Unspecified).AddTicks(380));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 950, DateTimeKind.Unspecified).AddTicks(393));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 950, DateTimeKind.Unspecified).AddTicks(394));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 950, DateTimeKind.Unspecified).AddTicks(396));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 950, DateTimeKind.Unspecified).AddTicks(397));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 950, DateTimeKind.Unspecified).AddTicks(7050));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 950, DateTimeKind.Unspecified).AddTicks(7072));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 950, DateTimeKind.Unspecified).AddTicks(7074));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 952, DateTimeKind.Unspecified).AddTicks(3635));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 952, DateTimeKind.Unspecified).AddTicks(3658));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 952, DateTimeKind.Unspecified).AddTicks(3659));
        }
    }
}
