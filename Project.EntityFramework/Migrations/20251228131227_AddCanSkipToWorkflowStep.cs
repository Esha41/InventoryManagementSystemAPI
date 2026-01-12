using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class AddCanSkipToWorkflowStep : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CanReturn",
                table: "WorkflowSteps",
                type: "bit",
                nullable: false,
                defaultValue: false);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanReturn",
                table: "WorkflowSteps");

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 242, DateTimeKind.Unspecified).AddTicks(3215));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 242, DateTimeKind.Unspecified).AddTicks(3241));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 242, DateTimeKind.Unspecified).AddTicks(3243));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 242, DateTimeKind.Unspecified).AddTicks(3244));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 242, DateTimeKind.Unspecified).AddTicks(3245));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 242, DateTimeKind.Unspecified).AddTicks(7411));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 242, DateTimeKind.Unspecified).AddTicks(7429));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 242, DateTimeKind.Unspecified).AddTicks(7431));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 242, DateTimeKind.Unspecified).AddTicks(7432));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 242, DateTimeKind.Unspecified).AddTicks(7433));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 243, DateTimeKind.Unspecified).AddTicks(365));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 243, DateTimeKind.Unspecified).AddTicks(380));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 243, DateTimeKind.Unspecified).AddTicks(382));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 243, DateTimeKind.Unspecified).AddTicks(384));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 243, DateTimeKind.Unspecified).AddTicks(385));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 243, DateTimeKind.Unspecified).AddTicks(386));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 243, DateTimeKind.Unspecified).AddTicks(388));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 243, DateTimeKind.Unspecified).AddTicks(389));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 243, DateTimeKind.Unspecified).AddTicks(390));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 243, DateTimeKind.Unspecified).AddTicks(392));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 243, DateTimeKind.Unspecified).AddTicks(393));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 243, DateTimeKind.Unspecified).AddTicks(394));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 243, DateTimeKind.Unspecified).AddTicks(396));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 243, DateTimeKind.Unspecified).AddTicks(397));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 15L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 243, DateTimeKind.Unspecified).AddTicks(398));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 16L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 243, DateTimeKind.Unspecified).AddTicks(399));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 17L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 243, DateTimeKind.Unspecified).AddTicks(400));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 18L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 243, DateTimeKind.Unspecified).AddTicks(401));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 243, DateTimeKind.Unspecified).AddTicks(1860));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 243, DateTimeKind.Unspecified).AddTicks(1875));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 243, DateTimeKind.Unspecified).AddTicks(1877));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 243, DateTimeKind.Unspecified).AddTicks(1879));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 244, DateTimeKind.Unspecified).AddTicks(348));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 244, DateTimeKind.Unspecified).AddTicks(369));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 244, DateTimeKind.Unspecified).AddTicks(370));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 244, DateTimeKind.Unspecified).AddTicks(372));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 244, DateTimeKind.Unspecified).AddTicks(373));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 244, DateTimeKind.Unspecified).AddTicks(9344));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 244, DateTimeKind.Unspecified).AddTicks(9364));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 244, DateTimeKind.Unspecified).AddTicks(9366));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 244, DateTimeKind.Unspecified).AddTicks(9368));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 244, DateTimeKind.Unspecified).AddTicks(9369));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 245, DateTimeKind.Unspecified).AddTicks(586));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 245, DateTimeKind.Unspecified).AddTicks(595));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 245, DateTimeKind.Unspecified).AddTicks(596));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 245, DateTimeKind.Unspecified).AddTicks(598));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 245, DateTimeKind.Unspecified).AddTicks(599));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 245, DateTimeKind.Unspecified).AddTicks(600));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 245, DateTimeKind.Unspecified).AddTicks(601));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 245, DateTimeKind.Unspecified).AddTicks(603));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 245, DateTimeKind.Unspecified).AddTicks(604));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 245, DateTimeKind.Unspecified).AddTicks(605));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 245, DateTimeKind.Unspecified).AddTicks(606));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 245, DateTimeKind.Unspecified).AddTicks(7991));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 245, DateTimeKind.Unspecified).AddTicks(8012));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 245, DateTimeKind.Unspecified).AddTicks(8014));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 245, DateTimeKind.Unspecified).AddTicks(8015));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 245, DateTimeKind.Unspecified).AddTicks(8016));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 245, DateTimeKind.Unspecified).AddTicks(9295));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 245, DateTimeKind.Unspecified).AddTicks(9304));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 245, DateTimeKind.Unspecified).AddTicks(9305));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 245, DateTimeKind.Unspecified).AddTicks(9307));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 245, DateTimeKind.Unspecified).AddTicks(9308));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 246, DateTimeKind.Unspecified).AddTicks(639));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 246, DateTimeKind.Unspecified).AddTicks(649));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 246, DateTimeKind.Unspecified).AddTicks(651));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 246, DateTimeKind.Unspecified).AddTicks(652));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 246, DateTimeKind.Unspecified).AddTicks(653));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 246, DateTimeKind.Unspecified).AddTicks(1970));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 246, DateTimeKind.Unspecified).AddTicks(1979));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 246, DateTimeKind.Unspecified).AddTicks(1981));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 246, DateTimeKind.Unspecified).AddTicks(1982));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 246, DateTimeKind.Unspecified).AddTicks(1983));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 246, DateTimeKind.Unspecified).AddTicks(1985));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 246, DateTimeKind.Unspecified).AddTicks(1986));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 246, DateTimeKind.Unspecified).AddTicks(1987));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 246, DateTimeKind.Unspecified).AddTicks(1988));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 246, DateTimeKind.Unspecified).AddTicks(1989));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 246, DateTimeKind.Unspecified).AddTicks(5476));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 246, DateTimeKind.Unspecified).AddTicks(5494));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 246, DateTimeKind.Unspecified).AddTicks(5496));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 246, DateTimeKind.Unspecified).AddTicks(5497));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 246, DateTimeKind.Unspecified).AddTicks(5500));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 246, DateTimeKind.Unspecified).AddTicks(5501));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 246, DateTimeKind.Unspecified).AddTicks(5502));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 246, DateTimeKind.Unspecified).AddTicks(5503));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 246, DateTimeKind.Unspecified).AddTicks(5504));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 246, DateTimeKind.Unspecified).AddTicks(5555));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 246, DateTimeKind.Unspecified).AddTicks(5556));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 246, DateTimeKind.Unspecified).AddTicks(5498));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 246, DateTimeKind.Unspecified).AddTicks(7332));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 246, DateTimeKind.Unspecified).AddTicks(7346));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 246, DateTimeKind.Unspecified).AddTicks(7348));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 246, DateTimeKind.Unspecified).AddTicks(7349));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 246, DateTimeKind.Unspecified).AddTicks(7350));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 246, DateTimeKind.Unspecified).AddTicks(8566));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 246, DateTimeKind.Unspecified).AddTicks(8575));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 246, DateTimeKind.Unspecified).AddTicks(8576));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 246, DateTimeKind.Unspecified).AddTicks(8578));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 246, DateTimeKind.Unspecified).AddTicks(8579));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 247, DateTimeKind.Unspecified).AddTicks(4106));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 247, DateTimeKind.Unspecified).AddTicks(4126));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 247, DateTimeKind.Unspecified).AddTicks(4127));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 248, DateTimeKind.Unspecified).AddTicks(8353));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 248, DateTimeKind.Unspecified).AddTicks(8374));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 25, 9, 54, 33, 248, DateTimeKind.Unspecified).AddTicks(8375));
        }
    }
}
