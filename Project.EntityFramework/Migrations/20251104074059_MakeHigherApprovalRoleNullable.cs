using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class MakeHigherApprovalRoleNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 73, DateTimeKind.Unspecified).AddTicks(8436));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 73, DateTimeKind.Unspecified).AddTicks(8504));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 73, DateTimeKind.Unspecified).AddTicks(8506));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 73, DateTimeKind.Unspecified).AddTicks(8508));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 73, DateTimeKind.Unspecified).AddTicks(8510));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 73, DateTimeKind.Unspecified).AddTicks(8511));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 73, DateTimeKind.Unspecified).AddTicks(8513));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 73, DateTimeKind.Unspecified).AddTicks(8515));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 74, DateTimeKind.Unspecified).AddTicks(3156));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 74, DateTimeKind.Unspecified).AddTicks(3180));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 74, DateTimeKind.Unspecified).AddTicks(3182));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 74, DateTimeKind.Unspecified).AddTicks(3184));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 74, DateTimeKind.Unspecified).AddTicks(3185));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 74, DateTimeKind.Unspecified).AddTicks(6147));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 74, DateTimeKind.Unspecified).AddTicks(6164));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 74, DateTimeKind.Unspecified).AddTicks(6166));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 74, DateTimeKind.Unspecified).AddTicks(6168));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 74, DateTimeKind.Unspecified).AddTicks(6169));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 74, DateTimeKind.Unspecified).AddTicks(9439));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 74, DateTimeKind.Unspecified).AddTicks(9459));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 74, DateTimeKind.Unspecified).AddTicks(9461));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 74, DateTimeKind.Unspecified).AddTicks(9463));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 74, DateTimeKind.Unspecified).AddTicks(9465));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 75, DateTimeKind.Unspecified).AddTicks(1053));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 75, DateTimeKind.Unspecified).AddTicks(1071));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 75, DateTimeKind.Unspecified).AddTicks(1073));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 75, DateTimeKind.Unspecified).AddTicks(1075));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 75, DateTimeKind.Unspecified).AddTicks(2521));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 75, DateTimeKind.Unspecified).AddTicks(2531));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 75, DateTimeKind.Unspecified).AddTicks(2533));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 75, DateTimeKind.Unspecified).AddTicks(2535));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 75, DateTimeKind.Unspecified).AddTicks(2537));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 75, DateTimeKind.Unspecified).AddTicks(4006));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 75, DateTimeKind.Unspecified).AddTicks(4016));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 75, DateTimeKind.Unspecified).AddTicks(4018));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 75, DateTimeKind.Unspecified).AddTicks(4020));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 75, DateTimeKind.Unspecified).AddTicks(4022));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 76, DateTimeKind.Unspecified).AddTicks(1586));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 76, DateTimeKind.Unspecified).AddTicks(1608));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 76, DateTimeKind.Unspecified).AddTicks(1611));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 76, DateTimeKind.Unspecified).AddTicks(1613));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 76, DateTimeKind.Unspecified).AddTicks(1615));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 76, DateTimeKind.Unspecified).AddTicks(3137));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 76, DateTimeKind.Unspecified).AddTicks(3148));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 76, DateTimeKind.Unspecified).AddTicks(3150));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 76, DateTimeKind.Unspecified).AddTicks(3152));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 76, DateTimeKind.Unspecified).AddTicks(3153));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 76, DateTimeKind.Unspecified).AddTicks(4587));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 76, DateTimeKind.Unspecified).AddTicks(4597));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 76, DateTimeKind.Unspecified).AddTicks(4599));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 76, DateTimeKind.Unspecified).AddTicks(4601));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 76, DateTimeKind.Unspecified).AddTicks(4602));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 76, DateTimeKind.Unspecified).AddTicks(6045));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 76, DateTimeKind.Unspecified).AddTicks(6055));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 76, DateTimeKind.Unspecified).AddTicks(6056));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 76, DateTimeKind.Unspecified).AddTicks(6058));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 76, DateTimeKind.Unspecified).AddTicks(6060));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 76, DateTimeKind.Unspecified).AddTicks(7435));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 76, DateTimeKind.Unspecified).AddTicks(7446));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 76, DateTimeKind.Unspecified).AddTicks(7448));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 76, DateTimeKind.Unspecified).AddTicks(7450));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 76, DateTimeKind.Unspecified).AddTicks(7452));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 76, DateTimeKind.Unspecified).AddTicks(8912));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 76, DateTimeKind.Unspecified).AddTicks(8922));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 76, DateTimeKind.Unspecified).AddTicks(8923));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 76, DateTimeKind.Unspecified).AddTicks(8925));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 76, DateTimeKind.Unspecified).AddTicks(8927));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 77, DateTimeKind.Unspecified).AddTicks(320));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 77, DateTimeKind.Unspecified).AddTicks(330));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 77, DateTimeKind.Unspecified).AddTicks(332));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 77, DateTimeKind.Unspecified).AddTicks(334));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 77, DateTimeKind.Unspecified).AddTicks(335));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 77, DateTimeKind.Unspecified).AddTicks(2163));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 77, DateTimeKind.Unspecified).AddTicks(2183));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 77, DateTimeKind.Unspecified).AddTicks(2185));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 77, DateTimeKind.Unspecified).AddTicks(2187));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 40, 59, 77, DateTimeKind.Unspecified).AddTicks(2189));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 206, DateTimeKind.Unspecified).AddTicks(1237));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 206, DateTimeKind.Unspecified).AddTicks(1311));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 206, DateTimeKind.Unspecified).AddTicks(1313));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 206, DateTimeKind.Unspecified).AddTicks(1315));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 206, DateTimeKind.Unspecified).AddTicks(1317));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 206, DateTimeKind.Unspecified).AddTicks(1318));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 206, DateTimeKind.Unspecified).AddTicks(1320));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 206, DateTimeKind.Unspecified).AddTicks(1322));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 206, DateTimeKind.Unspecified).AddTicks(6210));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 206, DateTimeKind.Unspecified).AddTicks(6232));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 206, DateTimeKind.Unspecified).AddTicks(6234));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 206, DateTimeKind.Unspecified).AddTicks(6236));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 206, DateTimeKind.Unspecified).AddTicks(6238));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 206, DateTimeKind.Unspecified).AddTicks(9691));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 206, DateTimeKind.Unspecified).AddTicks(9711));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 206, DateTimeKind.Unspecified).AddTicks(9713));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 206, DateTimeKind.Unspecified).AddTicks(9714));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 206, DateTimeKind.Unspecified).AddTicks(9716));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 207, DateTimeKind.Unspecified).AddTicks(3559));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 207, DateTimeKind.Unspecified).AddTicks(3583));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 207, DateTimeKind.Unspecified).AddTicks(3585));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 207, DateTimeKind.Unspecified).AddTicks(3587));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 207, DateTimeKind.Unspecified).AddTicks(3589));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 207, DateTimeKind.Unspecified).AddTicks(5255));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 207, DateTimeKind.Unspecified).AddTicks(5276));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 207, DateTimeKind.Unspecified).AddTicks(5278));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 207, DateTimeKind.Unspecified).AddTicks(5280));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 207, DateTimeKind.Unspecified).AddTicks(6748));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 207, DateTimeKind.Unspecified).AddTicks(6784));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 207, DateTimeKind.Unspecified).AddTicks(6786));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 207, DateTimeKind.Unspecified).AddTicks(6788));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 207, DateTimeKind.Unspecified).AddTicks(6789));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 207, DateTimeKind.Unspecified).AddTicks(8266));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 207, DateTimeKind.Unspecified).AddTicks(8276));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 207, DateTimeKind.Unspecified).AddTicks(8278));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 207, DateTimeKind.Unspecified).AddTicks(8280));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 207, DateTimeKind.Unspecified).AddTicks(8282));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 208, DateTimeKind.Unspecified).AddTicks(5886));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 208, DateTimeKind.Unspecified).AddTicks(5908));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 208, DateTimeKind.Unspecified).AddTicks(5910));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 208, DateTimeKind.Unspecified).AddTicks(5912));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 208, DateTimeKind.Unspecified).AddTicks(5914));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 208, DateTimeKind.Unspecified).AddTicks(7394));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 208, DateTimeKind.Unspecified).AddTicks(7404));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 208, DateTimeKind.Unspecified).AddTicks(7406));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 208, DateTimeKind.Unspecified).AddTicks(7408));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 208, DateTimeKind.Unspecified).AddTicks(7410));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 208, DateTimeKind.Unspecified).AddTicks(8854));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 208, DateTimeKind.Unspecified).AddTicks(8864));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 208, DateTimeKind.Unspecified).AddTicks(8867));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 208, DateTimeKind.Unspecified).AddTicks(8869));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 208, DateTimeKind.Unspecified).AddTicks(8870));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 209, DateTimeKind.Unspecified).AddTicks(288));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 209, DateTimeKind.Unspecified).AddTicks(298));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 209, DateTimeKind.Unspecified).AddTicks(300));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 209, DateTimeKind.Unspecified).AddTicks(302));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 209, DateTimeKind.Unspecified).AddTicks(304));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 209, DateTimeKind.Unspecified).AddTicks(1724));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 209, DateTimeKind.Unspecified).AddTicks(1819));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 209, DateTimeKind.Unspecified).AddTicks(1821));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 209, DateTimeKind.Unspecified).AddTicks(1823));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 209, DateTimeKind.Unspecified).AddTicks(1824));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 209, DateTimeKind.Unspecified).AddTicks(3316));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 209, DateTimeKind.Unspecified).AddTicks(3326));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 209, DateTimeKind.Unspecified).AddTicks(3328));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 209, DateTimeKind.Unspecified).AddTicks(3330));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 209, DateTimeKind.Unspecified).AddTicks(3332));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 209, DateTimeKind.Unspecified).AddTicks(4868));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 209, DateTimeKind.Unspecified).AddTicks(4878));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 209, DateTimeKind.Unspecified).AddTicks(4880));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 209, DateTimeKind.Unspecified).AddTicks(4882));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 209, DateTimeKind.Unspecified).AddTicks(4884));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 209, DateTimeKind.Unspecified).AddTicks(6376));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 209, DateTimeKind.Unspecified).AddTicks(6385));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 209, DateTimeKind.Unspecified).AddTicks(6388));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 209, DateTimeKind.Unspecified).AddTicks(6390));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 33, 46, 209, DateTimeKind.Unspecified).AddTicks(6392));
        }
    }
}
