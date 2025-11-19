using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class RefactorSupplyTableForStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Supplies",
                newName: "SubmissionStatus");

            migrationBuilder.AddColumn<int>(
                name: "FulfillmentStatus",
                table: "Supplies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 514, DateTimeKind.Unspecified).AddTicks(7720));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 514, DateTimeKind.Unspecified).AddTicks(7736));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 514, DateTimeKind.Unspecified).AddTicks(7737));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 514, DateTimeKind.Unspecified).AddTicks(7739));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 514, DateTimeKind.Unspecified).AddTicks(7740));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 514, DateTimeKind.Unspecified).AddTicks(7742));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 514, DateTimeKind.Unspecified).AddTicks(7743));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 514, DateTimeKind.Unspecified).AddTicks(7744));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 515, DateTimeKind.Unspecified).AddTicks(6185));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 515, DateTimeKind.Unspecified).AddTicks(6209));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 515, DateTimeKind.Unspecified).AddTicks(6211));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 515, DateTimeKind.Unspecified).AddTicks(6212));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 515, DateTimeKind.Unspecified).AddTicks(6213));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 515, DateTimeKind.Unspecified).AddTicks(9533));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 515, DateTimeKind.Unspecified).AddTicks(9553));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 515, DateTimeKind.Unspecified).AddTicks(9555));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 515, DateTimeKind.Unspecified).AddTicks(9557));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 515, DateTimeKind.Unspecified).AddTicks(9558));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 516, DateTimeKind.Unspecified).AddTicks(2833));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 516, DateTimeKind.Unspecified).AddTicks(2852));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 516, DateTimeKind.Unspecified).AddTicks(2853));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 516, DateTimeKind.Unspecified).AddTicks(2855));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 516, DateTimeKind.Unspecified).AddTicks(2856));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 516, DateTimeKind.Unspecified).AddTicks(4450));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 516, DateTimeKind.Unspecified).AddTicks(4462));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 516, DateTimeKind.Unspecified).AddTicks(4464));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 516, DateTimeKind.Unspecified).AddTicks(4467));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 516, DateTimeKind.Unspecified).AddTicks(6954));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 516, DateTimeKind.Unspecified).AddTicks(6972));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 516, DateTimeKind.Unspecified).AddTicks(6974));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 516, DateTimeKind.Unspecified).AddTicks(6975));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 516, DateTimeKind.Unspecified).AddTicks(6976));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 516, DateTimeKind.Unspecified).AddTicks(8397));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 516, DateTimeKind.Unspecified).AddTicks(8409));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 516, DateTimeKind.Unspecified).AddTicks(8411));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 516, DateTimeKind.Unspecified).AddTicks(8412));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 516, DateTimeKind.Unspecified).AddTicks(8413));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 517, DateTimeKind.Unspecified).AddTicks(5697));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 517, DateTimeKind.Unspecified).AddTicks(5718));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 517, DateTimeKind.Unspecified).AddTicks(5720));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 517, DateTimeKind.Unspecified).AddTicks(5721));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 517, DateTimeKind.Unspecified).AddTicks(5723));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 517, DateTimeKind.Unspecified).AddTicks(7253));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 517, DateTimeKind.Unspecified).AddTicks(7267));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 517, DateTimeKind.Unspecified).AddTicks(7268));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 517, DateTimeKind.Unspecified).AddTicks(7270));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 517, DateTimeKind.Unspecified).AddTicks(7271));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 518, DateTimeKind.Unspecified).AddTicks(7778));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 518, DateTimeKind.Unspecified).AddTicks(7805));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 518, DateTimeKind.Unspecified).AddTicks(7807));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 518, DateTimeKind.Unspecified).AddTicks(7809));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 518, DateTimeKind.Unspecified).AddTicks(7810));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 518, DateTimeKind.Unspecified).AddTicks(9441));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 518, DateTimeKind.Unspecified).AddTicks(9452));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 518, DateTimeKind.Unspecified).AddTicks(9454));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 518, DateTimeKind.Unspecified).AddTicks(9455));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 518, DateTimeKind.Unspecified).AddTicks(9456));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 519, DateTimeKind.Unspecified).AddTicks(909));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 519, DateTimeKind.Unspecified).AddTicks(992));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 519, DateTimeKind.Unspecified).AddTicks(995));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 519, DateTimeKind.Unspecified).AddTicks(997));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 519, DateTimeKind.Unspecified).AddTicks(998));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 519, DateTimeKind.Unspecified).AddTicks(2447));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 519, DateTimeKind.Unspecified).AddTicks(2458));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 519, DateTimeKind.Unspecified).AddTicks(2460));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 519, DateTimeKind.Unspecified).AddTicks(2461));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 519, DateTimeKind.Unspecified).AddTicks(2488));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 519, DateTimeKind.Unspecified).AddTicks(2489));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 519, DateTimeKind.Unspecified).AddTicks(2491));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 519, DateTimeKind.Unspecified).AddTicks(2492));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 519, DateTimeKind.Unspecified).AddTicks(2493));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 519, DateTimeKind.Unspecified).AddTicks(2494));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 519, DateTimeKind.Unspecified).AddTicks(6529));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 519, DateTimeKind.Unspecified).AddTicks(6550));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 519, DateTimeKind.Unspecified).AddTicks(6552));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 519, DateTimeKind.Unspecified).AddTicks(6553));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 519, DateTimeKind.Unspecified).AddTicks(6554));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 519, DateTimeKind.Unspecified).AddTicks(6556));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 519, DateTimeKind.Unspecified).AddTicks(6557));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 519, DateTimeKind.Unspecified).AddTicks(6558));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 519, DateTimeKind.Unspecified).AddTicks(6559));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 519, DateTimeKind.Unspecified).AddTicks(6560));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 519, DateTimeKind.Unspecified).AddTicks(6561));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 519, DateTimeKind.Unspecified).AddTicks(8708));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 519, DateTimeKind.Unspecified).AddTicks(8724));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 519, DateTimeKind.Unspecified).AddTicks(8726));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 519, DateTimeKind.Unspecified).AddTicks(8728));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 519, DateTimeKind.Unspecified).AddTicks(8729));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 520, DateTimeKind.Unspecified).AddTicks(101));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 520, DateTimeKind.Unspecified).AddTicks(112));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 520, DateTimeKind.Unspecified).AddTicks(114));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 520, DateTimeKind.Unspecified).AddTicks(115));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 520, DateTimeKind.Unspecified).AddTicks(116));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 520, DateTimeKind.Unspecified).AddTicks(7393));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 520, DateTimeKind.Unspecified).AddTicks(7435));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 520, DateTimeKind.Unspecified).AddTicks(7437));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 520, DateTimeKind.Unspecified).AddTicks(7438));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 520, DateTimeKind.Unspecified).AddTicks(7440));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 521, DateTimeKind.Unspecified).AddTicks(9150));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 521, DateTimeKind.Unspecified).AddTicks(9173));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 19, 12, 32, 58, 521, DateTimeKind.Unspecified).AddTicks(9174));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FulfillmentStatus",
                table: "Supplies");

            migrationBuilder.RenameColumn(
                name: "SubmissionStatus",
                table: "Supplies",
                newName: "Status");

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 712, DateTimeKind.Unspecified).AddTicks(8803));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 712, DateTimeKind.Unspecified).AddTicks(8821));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 712, DateTimeKind.Unspecified).AddTicks(8823));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 712, DateTimeKind.Unspecified).AddTicks(8824));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 712, DateTimeKind.Unspecified).AddTicks(8826));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 712, DateTimeKind.Unspecified).AddTicks(8827));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 712, DateTimeKind.Unspecified).AddTicks(8828));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 712, DateTimeKind.Unspecified).AddTicks(8830));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 713, DateTimeKind.Unspecified).AddTicks(6599));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 713, DateTimeKind.Unspecified).AddTicks(6621));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 713, DateTimeKind.Unspecified).AddTicks(6623));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 713, DateTimeKind.Unspecified).AddTicks(6624));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 713, DateTimeKind.Unspecified).AddTicks(6651));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 713, DateTimeKind.Unspecified).AddTicks(9449));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 713, DateTimeKind.Unspecified).AddTicks(9467));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 713, DateTimeKind.Unspecified).AddTicks(9495));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 713, DateTimeKind.Unspecified).AddTicks(9496));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 713, DateTimeKind.Unspecified).AddTicks(9498));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 714, DateTimeKind.Unspecified).AddTicks(2629));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 714, DateTimeKind.Unspecified).AddTicks(2648));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 714, DateTimeKind.Unspecified).AddTicks(2649));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 714, DateTimeKind.Unspecified).AddTicks(2651));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 714, DateTimeKind.Unspecified).AddTicks(2652));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 714, DateTimeKind.Unspecified).AddTicks(4271));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 714, DateTimeKind.Unspecified).AddTicks(4284));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 714, DateTimeKind.Unspecified).AddTicks(4286));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 714, DateTimeKind.Unspecified).AddTicks(4288));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 714, DateTimeKind.Unspecified).AddTicks(6510));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 714, DateTimeKind.Unspecified).AddTicks(6525));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 714, DateTimeKind.Unspecified).AddTicks(6526));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 714, DateTimeKind.Unspecified).AddTicks(6561));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 714, DateTimeKind.Unspecified).AddTicks(6562));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 714, DateTimeKind.Unspecified).AddTicks(8119));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 714, DateTimeKind.Unspecified).AddTicks(8132));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 714, DateTimeKind.Unspecified).AddTicks(8133));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 714, DateTimeKind.Unspecified).AddTicks(8134));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 714, DateTimeKind.Unspecified).AddTicks(8136));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 715, DateTimeKind.Unspecified).AddTicks(5396));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 715, DateTimeKind.Unspecified).AddTicks(5416));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 715, DateTimeKind.Unspecified).AddTicks(5418));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 715, DateTimeKind.Unspecified).AddTicks(5419));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 715, DateTimeKind.Unspecified).AddTicks(5421));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 715, DateTimeKind.Unspecified).AddTicks(6866));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 715, DateTimeKind.Unspecified).AddTicks(6878));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 715, DateTimeKind.Unspecified).AddTicks(6879));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 715, DateTimeKind.Unspecified).AddTicks(6881));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 715, DateTimeKind.Unspecified).AddTicks(6882));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 716, DateTimeKind.Unspecified).AddTicks(4582));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 716, DateTimeKind.Unspecified).AddTicks(4602));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 716, DateTimeKind.Unspecified).AddTicks(4604));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 716, DateTimeKind.Unspecified).AddTicks(4605));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 716, DateTimeKind.Unspecified).AddTicks(4607));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 716, DateTimeKind.Unspecified).AddTicks(5994));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 716, DateTimeKind.Unspecified).AddTicks(6004));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 716, DateTimeKind.Unspecified).AddTicks(6006));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 716, DateTimeKind.Unspecified).AddTicks(6007));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 716, DateTimeKind.Unspecified).AddTicks(6009));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 716, DateTimeKind.Unspecified).AddTicks(7332));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 716, DateTimeKind.Unspecified).AddTicks(7343));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 716, DateTimeKind.Unspecified).AddTicks(7344));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 716, DateTimeKind.Unspecified).AddTicks(7346));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 716, DateTimeKind.Unspecified).AddTicks(7347));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 716, DateTimeKind.Unspecified).AddTicks(8641));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 716, DateTimeKind.Unspecified).AddTicks(8651));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 716, DateTimeKind.Unspecified).AddTicks(8653));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 716, DateTimeKind.Unspecified).AddTicks(8654));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 716, DateTimeKind.Unspecified).AddTicks(8655));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 716, DateTimeKind.Unspecified).AddTicks(8657));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 716, DateTimeKind.Unspecified).AddTicks(8658));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 716, DateTimeKind.Unspecified).AddTicks(8659));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 716, DateTimeKind.Unspecified).AddTicks(8660));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 716, DateTimeKind.Unspecified).AddTicks(8661));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 717, DateTimeKind.Unspecified).AddTicks(2314));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 717, DateTimeKind.Unspecified).AddTicks(2335));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 717, DateTimeKind.Unspecified).AddTicks(2361));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 717, DateTimeKind.Unspecified).AddTicks(2363));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 717, DateTimeKind.Unspecified).AddTicks(2364));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 717, DateTimeKind.Unspecified).AddTicks(2366));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 717, DateTimeKind.Unspecified).AddTicks(2367));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 717, DateTimeKind.Unspecified).AddTicks(2368));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 717, DateTimeKind.Unspecified).AddTicks(2369));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 717, DateTimeKind.Unspecified).AddTicks(2371));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 717, DateTimeKind.Unspecified).AddTicks(2372));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 717, DateTimeKind.Unspecified).AddTicks(4659));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 717, DateTimeKind.Unspecified).AddTicks(4679));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 717, DateTimeKind.Unspecified).AddTicks(4681));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 717, DateTimeKind.Unspecified).AddTicks(4682));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 717, DateTimeKind.Unspecified).AddTicks(4684));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 717, DateTimeKind.Unspecified).AddTicks(6064));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 717, DateTimeKind.Unspecified).AddTicks(6074));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 717, DateTimeKind.Unspecified).AddTicks(6076));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 717, DateTimeKind.Unspecified).AddTicks(6077));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 717, DateTimeKind.Unspecified).AddTicks(6078));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 718, DateTimeKind.Unspecified).AddTicks(1808));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 718, DateTimeKind.Unspecified).AddTicks(1829));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 718, DateTimeKind.Unspecified).AddTicks(1830));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 718, DateTimeKind.Unspecified).AddTicks(1832));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 718, DateTimeKind.Unspecified).AddTicks(1833));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 719, DateTimeKind.Unspecified).AddTicks(2895));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 719, DateTimeKind.Unspecified).AddTicks(2917));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 18, 12, 39, 20, 719, DateTimeKind.Unspecified).AddTicks(2919));
        }
    }
}
