using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class explosivefirearm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Explosives",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Explosives", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Explosives_BaseItems_Id",
                        column: x => x.Id,
                        principalTable: "BaseItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Weapons",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weapons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Weapons_BaseItems_Id",
                        column: x => x.Id,
                        principalTable: "BaseItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 987, DateTimeKind.Unspecified).AddTicks(3324));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 987, DateTimeKind.Unspecified).AddTicks(3341));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 987, DateTimeKind.Unspecified).AddTicks(3342));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 987, DateTimeKind.Unspecified).AddTicks(3344));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 987, DateTimeKind.Unspecified).AddTicks(3345));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 987, DateTimeKind.Unspecified).AddTicks(3346));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 987, DateTimeKind.Unspecified).AddTicks(3348));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 987, DateTimeKind.Unspecified).AddTicks(3349));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 988, DateTimeKind.Unspecified).AddTicks(3038));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 988, DateTimeKind.Unspecified).AddTicks(3058));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 988, DateTimeKind.Unspecified).AddTicks(3060));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 988, DateTimeKind.Unspecified).AddTicks(3061));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 988, DateTimeKind.Unspecified).AddTicks(3063));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 988, DateTimeKind.Unspecified).AddTicks(6023));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 988, DateTimeKind.Unspecified).AddTicks(6041));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 988, DateTimeKind.Unspecified).AddTicks(6043));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 988, DateTimeKind.Unspecified).AddTicks(6044));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 988, DateTimeKind.Unspecified).AddTicks(6045));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 988, DateTimeKind.Unspecified).AddTicks(9069));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 988, DateTimeKind.Unspecified).AddTicks(9085));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 988, DateTimeKind.Unspecified).AddTicks(9087));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 988, DateTimeKind.Unspecified).AddTicks(9088));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 988, DateTimeKind.Unspecified).AddTicks(9090));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 989, DateTimeKind.Unspecified).AddTicks(590));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 989, DateTimeKind.Unspecified).AddTicks(602));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 989, DateTimeKind.Unspecified).AddTicks(604));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 989, DateTimeKind.Unspecified).AddTicks(606));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 989, DateTimeKind.Unspecified).AddTicks(3645));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 989, DateTimeKind.Unspecified).AddTicks(3661));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 989, DateTimeKind.Unspecified).AddTicks(3663));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 989, DateTimeKind.Unspecified).AddTicks(3665));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 989, DateTimeKind.Unspecified).AddTicks(3666));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 989, DateTimeKind.Unspecified).AddTicks(5013));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 989, DateTimeKind.Unspecified).AddTicks(5023));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 989, DateTimeKind.Unspecified).AddTicks(5025));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 989, DateTimeKind.Unspecified).AddTicks(5026));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 989, DateTimeKind.Unspecified).AddTicks(5027));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 990, DateTimeKind.Unspecified).AddTicks(1689));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 990, DateTimeKind.Unspecified).AddTicks(1710));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 990, DateTimeKind.Unspecified).AddTicks(1712));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 990, DateTimeKind.Unspecified).AddTicks(1713));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 990, DateTimeKind.Unspecified).AddTicks(1714));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 990, DateTimeKind.Unspecified).AddTicks(3211));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 990, DateTimeKind.Unspecified).AddTicks(3222));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 990, DateTimeKind.Unspecified).AddTicks(3224));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 990, DateTimeKind.Unspecified).AddTicks(3225));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 990, DateTimeKind.Unspecified).AddTicks(3226));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 991, DateTimeKind.Unspecified).AddTicks(1355));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 991, DateTimeKind.Unspecified).AddTicks(1378));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 991, DateTimeKind.Unspecified).AddTicks(1379));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 991, DateTimeKind.Unspecified).AddTicks(1381));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 991, DateTimeKind.Unspecified).AddTicks(1382));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 991, DateTimeKind.Unspecified).AddTicks(2739));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 991, DateTimeKind.Unspecified).AddTicks(2773));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 991, DateTimeKind.Unspecified).AddTicks(2776));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 991, DateTimeKind.Unspecified).AddTicks(2777));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 991, DateTimeKind.Unspecified).AddTicks(2778));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 991, DateTimeKind.Unspecified).AddTicks(4065));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 991, DateTimeKind.Unspecified).AddTicks(4074));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 991, DateTimeKind.Unspecified).AddTicks(4075));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 991, DateTimeKind.Unspecified).AddTicks(4077));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 991, DateTimeKind.Unspecified).AddTicks(4078));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 991, DateTimeKind.Unspecified).AddTicks(5356));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 991, DateTimeKind.Unspecified).AddTicks(5389));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 991, DateTimeKind.Unspecified).AddTicks(5391));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 991, DateTimeKind.Unspecified).AddTicks(5392));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 991, DateTimeKind.Unspecified).AddTicks(5393));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 991, DateTimeKind.Unspecified).AddTicks(5394));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 991, DateTimeKind.Unspecified).AddTicks(5396));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 991, DateTimeKind.Unspecified).AddTicks(5397));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 991, DateTimeKind.Unspecified).AddTicks(5398));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 991, DateTimeKind.Unspecified).AddTicks(5399));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 991, DateTimeKind.Unspecified).AddTicks(8800));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 991, DateTimeKind.Unspecified).AddTicks(8820));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 991, DateTimeKind.Unspecified).AddTicks(8821));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 991, DateTimeKind.Unspecified).AddTicks(8823));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 991, DateTimeKind.Unspecified).AddTicks(8824));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 991, DateTimeKind.Unspecified).AddTicks(8825));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 991, DateTimeKind.Unspecified).AddTicks(8827));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 991, DateTimeKind.Unspecified).AddTicks(8828));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 991, DateTimeKind.Unspecified).AddTicks(8829));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 991, DateTimeKind.Unspecified).AddTicks(8830));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 991, DateTimeKind.Unspecified).AddTicks(8831));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 992, DateTimeKind.Unspecified).AddTicks(843));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 992, DateTimeKind.Unspecified).AddTicks(858));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 992, DateTimeKind.Unspecified).AddTicks(859));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 992, DateTimeKind.Unspecified).AddTicks(861));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 992, DateTimeKind.Unspecified).AddTicks(862));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 992, DateTimeKind.Unspecified).AddTicks(2121));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 992, DateTimeKind.Unspecified).AddTicks(2131));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 992, DateTimeKind.Unspecified).AddTicks(2132));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 992, DateTimeKind.Unspecified).AddTicks(2133));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 992, DateTimeKind.Unspecified).AddTicks(2135));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 992, DateTimeKind.Unspecified).AddTicks(3403));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 992, DateTimeKind.Unspecified).AddTicks(3413));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 992, DateTimeKind.Unspecified).AddTicks(3415));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 992, DateTimeKind.Unspecified).AddTicks(3416));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 992, DateTimeKind.Unspecified).AddTicks(3417));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 993, DateTimeKind.Unspecified).AddTicks(8657));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 993, DateTimeKind.Unspecified).AddTicks(8679));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 9, 25, 40, 993, DateTimeKind.Unspecified).AddTicks(8681));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Explosives");

            migrationBuilder.DropTable(
                name: "Weapons");

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 460, DateTimeKind.Unspecified).AddTicks(8636));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 460, DateTimeKind.Unspecified).AddTicks(8654));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 460, DateTimeKind.Unspecified).AddTicks(8656));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 460, DateTimeKind.Unspecified).AddTicks(8657));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 460, DateTimeKind.Unspecified).AddTicks(8659));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 460, DateTimeKind.Unspecified).AddTicks(8660));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 460, DateTimeKind.Unspecified).AddTicks(8661));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 460, DateTimeKind.Unspecified).AddTicks(8663));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 461, DateTimeKind.Unspecified).AddTicks(8687));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 461, DateTimeKind.Unspecified).AddTicks(8710));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 461, DateTimeKind.Unspecified).AddTicks(8712));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 461, DateTimeKind.Unspecified).AddTicks(8713));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 461, DateTimeKind.Unspecified).AddTicks(8714));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 462, DateTimeKind.Unspecified).AddTicks(1607));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 462, DateTimeKind.Unspecified).AddTicks(1624));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 462, DateTimeKind.Unspecified).AddTicks(1625));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 462, DateTimeKind.Unspecified).AddTicks(1626));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 462, DateTimeKind.Unspecified).AddTicks(1628));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 462, DateTimeKind.Unspecified).AddTicks(5115));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 462, DateTimeKind.Unspecified).AddTicks(5134));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 462, DateTimeKind.Unspecified).AddTicks(5136));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 462, DateTimeKind.Unspecified).AddTicks(5137));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 462, DateTimeKind.Unspecified).AddTicks(5139));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 462, DateTimeKind.Unspecified).AddTicks(6806));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 462, DateTimeKind.Unspecified).AddTicks(6821));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 462, DateTimeKind.Unspecified).AddTicks(6823));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 462, DateTimeKind.Unspecified).AddTicks(6825));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 462, DateTimeKind.Unspecified).AddTicks(9291));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 462, DateTimeKind.Unspecified).AddTicks(9305));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 462, DateTimeKind.Unspecified).AddTicks(9307));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 462, DateTimeKind.Unspecified).AddTicks(9308));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 462, DateTimeKind.Unspecified).AddTicks(9309));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 463, DateTimeKind.Unspecified).AddTicks(626));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 463, DateTimeKind.Unspecified).AddTicks(635));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 463, DateTimeKind.Unspecified).AddTicks(637));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 463, DateTimeKind.Unspecified).AddTicks(638));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 463, DateTimeKind.Unspecified).AddTicks(640));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 463, DateTimeKind.Unspecified).AddTicks(7724));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 463, DateTimeKind.Unspecified).AddTicks(7744));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 463, DateTimeKind.Unspecified).AddTicks(7746));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 463, DateTimeKind.Unspecified).AddTicks(7748));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 463, DateTimeKind.Unspecified).AddTicks(7749));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 463, DateTimeKind.Unspecified).AddTicks(9465));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 463, DateTimeKind.Unspecified).AddTicks(9482));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 463, DateTimeKind.Unspecified).AddTicks(9508));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 463, DateTimeKind.Unspecified).AddTicks(9510));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 463, DateTimeKind.Unspecified).AddTicks(9511));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 464, DateTimeKind.Unspecified).AddTicks(8279));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 464, DateTimeKind.Unspecified).AddTicks(8301));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 464, DateTimeKind.Unspecified).AddTicks(8303));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 464, DateTimeKind.Unspecified).AddTicks(8305));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 464, DateTimeKind.Unspecified).AddTicks(8306));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 464, DateTimeKind.Unspecified).AddTicks(9841));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 464, DateTimeKind.Unspecified).AddTicks(9852));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 464, DateTimeKind.Unspecified).AddTicks(9853));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 464, DateTimeKind.Unspecified).AddTicks(9855));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 464, DateTimeKind.Unspecified).AddTicks(9856));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 465, DateTimeKind.Unspecified).AddTicks(1263));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 465, DateTimeKind.Unspecified).AddTicks(1273));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 465, DateTimeKind.Unspecified).AddTicks(1275));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 465, DateTimeKind.Unspecified).AddTicks(1276));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 465, DateTimeKind.Unspecified).AddTicks(1277));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 465, DateTimeKind.Unspecified).AddTicks(2659));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 465, DateTimeKind.Unspecified).AddTicks(2670));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 465, DateTimeKind.Unspecified).AddTicks(2671));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 465, DateTimeKind.Unspecified).AddTicks(2672));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 465, DateTimeKind.Unspecified).AddTicks(2674));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 465, DateTimeKind.Unspecified).AddTicks(2675));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 465, DateTimeKind.Unspecified).AddTicks(2676));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 465, DateTimeKind.Unspecified).AddTicks(2678));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 465, DateTimeKind.Unspecified).AddTicks(2679));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 465, DateTimeKind.Unspecified).AddTicks(2680));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 465, DateTimeKind.Unspecified).AddTicks(6307));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 465, DateTimeKind.Unspecified).AddTicks(6331));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 465, DateTimeKind.Unspecified).AddTicks(6332));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 465, DateTimeKind.Unspecified).AddTicks(6334));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 465, DateTimeKind.Unspecified).AddTicks(6335));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 465, DateTimeKind.Unspecified).AddTicks(6336));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 465, DateTimeKind.Unspecified).AddTicks(6338));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 465, DateTimeKind.Unspecified).AddTicks(6339));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 465, DateTimeKind.Unspecified).AddTicks(6340));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 465, DateTimeKind.Unspecified).AddTicks(6341));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 465, DateTimeKind.Unspecified).AddTicks(6342));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 465, DateTimeKind.Unspecified).AddTicks(8429));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 465, DateTimeKind.Unspecified).AddTicks(8446));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 465, DateTimeKind.Unspecified).AddTicks(8448));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 465, DateTimeKind.Unspecified).AddTicks(8449));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 465, DateTimeKind.Unspecified).AddTicks(8450));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 465, DateTimeKind.Unspecified).AddTicks(9789));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 465, DateTimeKind.Unspecified).AddTicks(9799));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 465, DateTimeKind.Unspecified).AddTicks(9801));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 465, DateTimeKind.Unspecified).AddTicks(9802));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 465, DateTimeKind.Unspecified).AddTicks(9803));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 466, DateTimeKind.Unspecified).AddTicks(1112));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 466, DateTimeKind.Unspecified).AddTicks(1121));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 466, DateTimeKind.Unspecified).AddTicks(1123));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 466, DateTimeKind.Unspecified).AddTicks(1124));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 466, DateTimeKind.Unspecified).AddTicks(1126));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 467, DateTimeKind.Unspecified).AddTicks(3177));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 467, DateTimeKind.Unspecified).AddTicks(3200));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 8, 11, 28, 467, DateTimeKind.Unspecified).AddTicks(3226));
        }
    }
}
