using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class deligationApproval : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DelegationStatus",
                table: "UserDelegations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 36, DateTimeKind.Unspecified).AddTicks(1637));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 36, DateTimeKind.Unspecified).AddTicks(1665));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 36, DateTimeKind.Unspecified).AddTicks(1667));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 36, DateTimeKind.Unspecified).AddTicks(1668));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 36, DateTimeKind.Unspecified).AddTicks(1669));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 36, DateTimeKind.Unspecified).AddTicks(6042));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 36, DateTimeKind.Unspecified).AddTicks(6060));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 36, DateTimeKind.Unspecified).AddTicks(6062));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 36, DateTimeKind.Unspecified).AddTicks(6063));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 36, DateTimeKind.Unspecified).AddTicks(6064));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 36, DateTimeKind.Unspecified).AddTicks(9223));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 36, DateTimeKind.Unspecified).AddTicks(9238));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 36, DateTimeKind.Unspecified).AddTicks(9240));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 36, DateTimeKind.Unspecified).AddTicks(9242));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 36, DateTimeKind.Unspecified).AddTicks(9243));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 36, DateTimeKind.Unspecified).AddTicks(9263));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 36, DateTimeKind.Unspecified).AddTicks(9265));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 36, DateTimeKind.Unspecified).AddTicks(9267));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 36, DateTimeKind.Unspecified).AddTicks(9268));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 36, DateTimeKind.Unspecified).AddTicks(9269));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 36, DateTimeKind.Unspecified).AddTicks(9271));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 36, DateTimeKind.Unspecified).AddTicks(9272));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 36, DateTimeKind.Unspecified).AddTicks(9273));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 36, DateTimeKind.Unspecified).AddTicks(9275));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 15L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 36, DateTimeKind.Unspecified).AddTicks(9276));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 16L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 36, DateTimeKind.Unspecified).AddTicks(9277));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 17L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 36, DateTimeKind.Unspecified).AddTicks(9278));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 18L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 36, DateTimeKind.Unspecified).AddTicks(9280));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 37, DateTimeKind.Unspecified).AddTicks(924));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 37, DateTimeKind.Unspecified).AddTicks(937));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 37, DateTimeKind.Unspecified).AddTicks(939));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 37, DateTimeKind.Unspecified).AddTicks(942));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 37, DateTimeKind.Unspecified).AddTicks(9759));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 37, DateTimeKind.Unspecified).AddTicks(9781));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 37, DateTimeKind.Unspecified).AddTicks(9783));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 37, DateTimeKind.Unspecified).AddTicks(9784));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 37, DateTimeKind.Unspecified).AddTicks(9786));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 39, DateTimeKind.Unspecified).AddTicks(2259));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 39, DateTimeKind.Unspecified).AddTicks(2280));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 39, DateTimeKind.Unspecified).AddTicks(2281));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 39, DateTimeKind.Unspecified).AddTicks(2283));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 39, DateTimeKind.Unspecified).AddTicks(2284));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 39, DateTimeKind.Unspecified).AddTicks(3764));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 39, DateTimeKind.Unspecified).AddTicks(3776));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 39, DateTimeKind.Unspecified).AddTicks(3777));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 39, DateTimeKind.Unspecified).AddTicks(3779));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 39, DateTimeKind.Unspecified).AddTicks(3780));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 39, DateTimeKind.Unspecified).AddTicks(3781));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 39, DateTimeKind.Unspecified).AddTicks(3782));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 39, DateTimeKind.Unspecified).AddTicks(3783));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 39, DateTimeKind.Unspecified).AddTicks(3784));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 39, DateTimeKind.Unspecified).AddTicks(3785));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 39, DateTimeKind.Unspecified).AddTicks(3786));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 40, DateTimeKind.Unspecified).AddTicks(1992));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 40, DateTimeKind.Unspecified).AddTicks(2015));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 40, DateTimeKind.Unspecified).AddTicks(2017));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 40, DateTimeKind.Unspecified).AddTicks(2018));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 40, DateTimeKind.Unspecified).AddTicks(2020));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 40, DateTimeKind.Unspecified).AddTicks(3689));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 40, DateTimeKind.Unspecified).AddTicks(3702));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 40, DateTimeKind.Unspecified).AddTicks(3703));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 40, DateTimeKind.Unspecified).AddTicks(3704));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 40, DateTimeKind.Unspecified).AddTicks(3705));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 40, DateTimeKind.Unspecified).AddTicks(5134));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 40, DateTimeKind.Unspecified).AddTicks(5144));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 40, DateTimeKind.Unspecified).AddTicks(5146));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 40, DateTimeKind.Unspecified).AddTicks(5147));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 40, DateTimeKind.Unspecified).AddTicks(5148));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 40, DateTimeKind.Unspecified).AddTicks(6494));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 40, DateTimeKind.Unspecified).AddTicks(6504));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 40, DateTimeKind.Unspecified).AddTicks(6505));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 40, DateTimeKind.Unspecified).AddTicks(6507));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 40, DateTimeKind.Unspecified).AddTicks(6508));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 40, DateTimeKind.Unspecified).AddTicks(6509));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 40, DateTimeKind.Unspecified).AddTicks(6511));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 40, DateTimeKind.Unspecified).AddTicks(6512));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 40, DateTimeKind.Unspecified).AddTicks(6513));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 40, DateTimeKind.Unspecified).AddTicks(6514));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 41, DateTimeKind.Unspecified).AddTicks(94));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 41, DateTimeKind.Unspecified).AddTicks(113));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 41, DateTimeKind.Unspecified).AddTicks(115));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 41, DateTimeKind.Unspecified).AddTicks(116));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 41, DateTimeKind.Unspecified).AddTicks(119));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 41, DateTimeKind.Unspecified).AddTicks(120));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 41, DateTimeKind.Unspecified).AddTicks(121));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 41, DateTimeKind.Unspecified).AddTicks(123));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 41, DateTimeKind.Unspecified).AddTicks(124));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 41, DateTimeKind.Unspecified).AddTicks(125));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 41, DateTimeKind.Unspecified).AddTicks(126));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 41, DateTimeKind.Unspecified).AddTicks(118));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 41, DateTimeKind.Unspecified).AddTicks(1949));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 41, DateTimeKind.Unspecified).AddTicks(1961));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 41, DateTimeKind.Unspecified).AddTicks(1963));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 41, DateTimeKind.Unspecified).AddTicks(1964));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 41, DateTimeKind.Unspecified).AddTicks(1966));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 41, DateTimeKind.Unspecified).AddTicks(3291));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 41, DateTimeKind.Unspecified).AddTicks(3301));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 41, DateTimeKind.Unspecified).AddTicks(3303));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 41, DateTimeKind.Unspecified).AddTicks(3304));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 41, DateTimeKind.Unspecified).AddTicks(3305));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 41, DateTimeKind.Unspecified).AddTicks(8956));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 41, DateTimeKind.Unspecified).AddTicks(8976));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 41, DateTimeKind.Unspecified).AddTicks(8978));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 43, DateTimeKind.Unspecified).AddTicks(5197));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 43, DateTimeKind.Unspecified).AddTicks(5218));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 15, 52, 22, 43, DateTimeKind.Unspecified).AddTicks(5219));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DelegationStatus",
                table: "UserDelegations");

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 982, DateTimeKind.Unspecified).AddTicks(6122));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 982, DateTimeKind.Unspecified).AddTicks(6147));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 982, DateTimeKind.Unspecified).AddTicks(6150));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 982, DateTimeKind.Unspecified).AddTicks(6152));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 982, DateTimeKind.Unspecified).AddTicks(6154));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 983, DateTimeKind.Unspecified).AddTicks(579));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 983, DateTimeKind.Unspecified).AddTicks(606));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 983, DateTimeKind.Unspecified).AddTicks(610));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 983, DateTimeKind.Unspecified).AddTicks(612));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 983, DateTimeKind.Unspecified).AddTicks(614));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 983, DateTimeKind.Unspecified).AddTicks(3951));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 983, DateTimeKind.Unspecified).AddTicks(3972));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 983, DateTimeKind.Unspecified).AddTicks(3975));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 983, DateTimeKind.Unspecified).AddTicks(3977));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 983, DateTimeKind.Unspecified).AddTicks(3979));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 983, DateTimeKind.Unspecified).AddTicks(3980));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 983, DateTimeKind.Unspecified).AddTicks(3982));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 983, DateTimeKind.Unspecified).AddTicks(3984));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 983, DateTimeKind.Unspecified).AddTicks(3986));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 983, DateTimeKind.Unspecified).AddTicks(3987));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 983, DateTimeKind.Unspecified).AddTicks(3995));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 983, DateTimeKind.Unspecified).AddTicks(4020));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 983, DateTimeKind.Unspecified).AddTicks(4024));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 983, DateTimeKind.Unspecified).AddTicks(4026));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 15L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 983, DateTimeKind.Unspecified).AddTicks(4027));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 16L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 983, DateTimeKind.Unspecified).AddTicks(4029));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 17L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 983, DateTimeKind.Unspecified).AddTicks(4031));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 18L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 983, DateTimeKind.Unspecified).AddTicks(4032));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 983, DateTimeKind.Unspecified).AddTicks(5739));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 983, DateTimeKind.Unspecified).AddTicks(5759));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 983, DateTimeKind.Unspecified).AddTicks(5764));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 983, DateTimeKind.Unspecified).AddTicks(5766));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 984, DateTimeKind.Unspecified).AddTicks(5119));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 984, DateTimeKind.Unspecified).AddTicks(5143));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 984, DateTimeKind.Unspecified).AddTicks(5146));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 984, DateTimeKind.Unspecified).AddTicks(5148));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 984, DateTimeKind.Unspecified).AddTicks(5150));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 985, DateTimeKind.Unspecified).AddTicks(7934));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 985, DateTimeKind.Unspecified).AddTicks(7958));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 985, DateTimeKind.Unspecified).AddTicks(7961));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 985, DateTimeKind.Unspecified).AddTicks(7963));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 985, DateTimeKind.Unspecified).AddTicks(7965));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 985, DateTimeKind.Unspecified).AddTicks(9553));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 985, DateTimeKind.Unspecified).AddTicks(9569));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 985, DateTimeKind.Unspecified).AddTicks(9572));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 985, DateTimeKind.Unspecified).AddTicks(9573));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 985, DateTimeKind.Unspecified).AddTicks(9575));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 985, DateTimeKind.Unspecified).AddTicks(9577));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 985, DateTimeKind.Unspecified).AddTicks(9578));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 985, DateTimeKind.Unspecified).AddTicks(9580));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 985, DateTimeKind.Unspecified).AddTicks(9581));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 985, DateTimeKind.Unspecified).AddTicks(9583));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 985, DateTimeKind.Unspecified).AddTicks(9584));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 986, DateTimeKind.Unspecified).AddTicks(9086));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 986, DateTimeKind.Unspecified).AddTicks(9112));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 986, DateTimeKind.Unspecified).AddTicks(9115));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 986, DateTimeKind.Unspecified).AddTicks(9117));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 986, DateTimeKind.Unspecified).AddTicks(9118));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(781));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(799));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(801));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(803));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(805));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(2263));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(2277));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(2279));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(2280));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(2282));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(3739));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(3754));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(3756));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(3758));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(3761));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(3762));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(3764));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(3765));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(3767));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(3768));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(7813));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(7837));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(7839));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(7841));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(7844));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(7846));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(7847));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(7849));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(7851));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(7852));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(7854));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(7842));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(9873));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(9896));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(9898));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(9900));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 987, DateTimeKind.Unspecified).AddTicks(9901));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 988, DateTimeKind.Unspecified).AddTicks(1310));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 988, DateTimeKind.Unspecified).AddTicks(1324));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 988, DateTimeKind.Unspecified).AddTicks(1349));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 988, DateTimeKind.Unspecified).AddTicks(1351));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 988, DateTimeKind.Unspecified).AddTicks(1353));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 988, DateTimeKind.Unspecified).AddTicks(7744));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 988, DateTimeKind.Unspecified).AddTicks(7768));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 988, DateTimeKind.Unspecified).AddTicks(7771));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 990, DateTimeKind.Unspecified).AddTicks(5633));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 990, DateTimeKind.Unspecified).AddTicks(5657));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 12, 14, 46, 35, 990, DateTimeKind.Unspecified).AddTicks(5660));
        }
    }
}
