using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class WeaponColums : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Weapons_Units_BarrelLengthUnitId",
                table: "Weapons");

            migrationBuilder.DropForeignKey(
                name: "FK_Weapons_Units_OverallLengthUnitId",
                table: "Weapons");

            migrationBuilder.DropForeignKey(
                name: "FK_Weapons_Units_WeightUnitId",
                table: "Weapons");

            migrationBuilder.DropIndex(
                name: "IX_Weapons_BarrelLengthUnitId",
                table: "Weapons");

            migrationBuilder.DropColumn(
                name: "ActionType",
                table: "Weapons");

            migrationBuilder.DropColumn(
                name: "BarrelLength",
                table: "Weapons");

            migrationBuilder.DropColumn(
                name: "BarrelLengthUnitId",
                table: "Weapons");

            migrationBuilder.DropColumn(
                name: "OverallLength",
                table: "Weapons");

            migrationBuilder.DropColumn(
                name: "WeaponType",
                table: "Weapons");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Weapons");

            migrationBuilder.RenameColumn(
                name: "WeightUnitId",
                table: "Weapons",
                newName: "CountryOfManufactureId");

            migrationBuilder.RenameColumn(
                name: "OverallLengthUnitId",
                table: "Weapons",
                newName: "CaliberUnitId");

            migrationBuilder.RenameColumn(
                name: "Capacity",
                table: "Weapons",
                newName: "YearOfManufacture");

            migrationBuilder.RenameIndex(
                name: "IX_Weapons_WeightUnitId",
                table: "Weapons",
                newName: "IX_Weapons_CountryOfManufactureId");

            migrationBuilder.RenameIndex(
                name: "IX_Weapons_OverallLengthUnitId",
                table: "Weapons",
                newName: "IX_Weapons_CaliberUnitId");

            migrationBuilder.AddColumn<string>(
                name: "Model",
                table: "Weapons",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 136, DateTimeKind.Unspecified).AddTicks(3761));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 136, DateTimeKind.Unspecified).AddTicks(3791));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 136, DateTimeKind.Unspecified).AddTicks(3792));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 136, DateTimeKind.Unspecified).AddTicks(3794));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 136, DateTimeKind.Unspecified).AddTicks(3795));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 136, DateTimeKind.Unspecified).AddTicks(8340));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 136, DateTimeKind.Unspecified).AddTicks(8361));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 136, DateTimeKind.Unspecified).AddTicks(8362));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 136, DateTimeKind.Unspecified).AddTicks(8364));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 136, DateTimeKind.Unspecified).AddTicks(8365));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 137, DateTimeKind.Unspecified).AddTicks(1544));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 137, DateTimeKind.Unspecified).AddTicks(1562));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 137, DateTimeKind.Unspecified).AddTicks(1564));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 137, DateTimeKind.Unspecified).AddTicks(1566));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 137, DateTimeKind.Unspecified).AddTicks(1568));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 137, DateTimeKind.Unspecified).AddTicks(1569));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 137, DateTimeKind.Unspecified).AddTicks(1570));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 137, DateTimeKind.Unspecified).AddTicks(1572));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 137, DateTimeKind.Unspecified).AddTicks(1573));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 137, DateTimeKind.Unspecified).AddTicks(1575));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 137, DateTimeKind.Unspecified).AddTicks(1576));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 137, DateTimeKind.Unspecified).AddTicks(1577));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 137, DateTimeKind.Unspecified).AddTicks(1579));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 137, DateTimeKind.Unspecified).AddTicks(1580));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 15L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 137, DateTimeKind.Unspecified).AddTicks(1581));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 16L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 137, DateTimeKind.Unspecified).AddTicks(1583));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 17L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 137, DateTimeKind.Unspecified).AddTicks(1584));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 18L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 137, DateTimeKind.Unspecified).AddTicks(1585));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 137, DateTimeKind.Unspecified).AddTicks(3586));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 137, DateTimeKind.Unspecified).AddTicks(3606));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 137, DateTimeKind.Unspecified).AddTicks(3609));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 137, DateTimeKind.Unspecified).AddTicks(3610));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 138, DateTimeKind.Unspecified).AddTicks(603));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 138, DateTimeKind.Unspecified).AddTicks(623));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 138, DateTimeKind.Unspecified).AddTicks(625));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 138, DateTimeKind.Unspecified).AddTicks(626));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 138, DateTimeKind.Unspecified).AddTicks(627));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 139, DateTimeKind.Unspecified).AddTicks(12));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 139, DateTimeKind.Unspecified).AddTicks(34));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 139, DateTimeKind.Unspecified).AddTicks(36));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 139, DateTimeKind.Unspecified).AddTicks(37));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 139, DateTimeKind.Unspecified).AddTicks(38));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 139, DateTimeKind.Unspecified).AddTicks(1426));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 139, DateTimeKind.Unspecified).AddTicks(1436));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 139, DateTimeKind.Unspecified).AddTicks(1437));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 139, DateTimeKind.Unspecified).AddTicks(1438));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 139, DateTimeKind.Unspecified).AddTicks(1440));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 139, DateTimeKind.Unspecified).AddTicks(1441));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 139, DateTimeKind.Unspecified).AddTicks(1442));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 139, DateTimeKind.Unspecified).AddTicks(1443));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 139, DateTimeKind.Unspecified).AddTicks(1444));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 139, DateTimeKind.Unspecified).AddTicks(1445));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 139, DateTimeKind.Unspecified).AddTicks(1447));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 139, DateTimeKind.Unspecified).AddTicks(9576));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 139, DateTimeKind.Unspecified).AddTicks(9598));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 139, DateTimeKind.Unspecified).AddTicks(9599));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 139, DateTimeKind.Unspecified).AddTicks(9601));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 139, DateTimeKind.Unspecified).AddTicks(9602));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 140, DateTimeKind.Unspecified).AddTicks(946));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 140, DateTimeKind.Unspecified).AddTicks(956));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 140, DateTimeKind.Unspecified).AddTicks(957));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 140, DateTimeKind.Unspecified).AddTicks(959));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 140, DateTimeKind.Unspecified).AddTicks(960));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 140, DateTimeKind.Unspecified).AddTicks(2382));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 140, DateTimeKind.Unspecified).AddTicks(2391));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 140, DateTimeKind.Unspecified).AddTicks(2393));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 140, DateTimeKind.Unspecified).AddTicks(2394));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 140, DateTimeKind.Unspecified).AddTicks(2396));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 140, DateTimeKind.Unspecified).AddTicks(3684));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 140, DateTimeKind.Unspecified).AddTicks(3693));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 140, DateTimeKind.Unspecified).AddTicks(3695));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 140, DateTimeKind.Unspecified).AddTicks(3696));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 140, DateTimeKind.Unspecified).AddTicks(3698));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 140, DateTimeKind.Unspecified).AddTicks(3699));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 140, DateTimeKind.Unspecified).AddTicks(3700));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 140, DateTimeKind.Unspecified).AddTicks(3701));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 140, DateTimeKind.Unspecified).AddTicks(3703));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 140, DateTimeKind.Unspecified).AddTicks(3704));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 140, DateTimeKind.Unspecified).AddTicks(7118));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 140, DateTimeKind.Unspecified).AddTicks(7138));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 140, DateTimeKind.Unspecified).AddTicks(7140));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 140, DateTimeKind.Unspecified).AddTicks(7165));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 140, DateTimeKind.Unspecified).AddTicks(7168));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 140, DateTimeKind.Unspecified).AddTicks(7169));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 140, DateTimeKind.Unspecified).AddTicks(7170));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 140, DateTimeKind.Unspecified).AddTicks(7171));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 140, DateTimeKind.Unspecified).AddTicks(7172));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 140, DateTimeKind.Unspecified).AddTicks(7174));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 140, DateTimeKind.Unspecified).AddTicks(7175));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 140, DateTimeKind.Unspecified).AddTicks(7167));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 140, DateTimeKind.Unspecified).AddTicks(9228));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 140, DateTimeKind.Unspecified).AddTicks(9248));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 140, DateTimeKind.Unspecified).AddTicks(9250));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 140, DateTimeKind.Unspecified).AddTicks(9251));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 140, DateTimeKind.Unspecified).AddTicks(9252));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 141, DateTimeKind.Unspecified).AddTicks(519));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 141, DateTimeKind.Unspecified).AddTicks(529));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 141, DateTimeKind.Unspecified).AddTicks(531));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 141, DateTimeKind.Unspecified).AddTicks(532));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 141, DateTimeKind.Unspecified).AddTicks(534));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 141, DateTimeKind.Unspecified).AddTicks(6346));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 141, DateTimeKind.Unspecified).AddTicks(6382));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 141, DateTimeKind.Unspecified).AddTicks(6384));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 143, DateTimeKind.Unspecified).AddTicks(545));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 143, DateTimeKind.Unspecified).AddTicks(566));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 11, 27, 59, 143, DateTimeKind.Unspecified).AddTicks(568));

            migrationBuilder.AddForeignKey(
                name: "FK_Weapons_Countries_CountryOfManufactureId",
                table: "Weapons",
                column: "CountryOfManufactureId",
                principalTable: "Countries",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Weapons_Units_CaliberUnitId",
                table: "Weapons",
                column: "CaliberUnitId",
                principalTable: "Units",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Weapons_Countries_CountryOfManufactureId",
                table: "Weapons");

            migrationBuilder.DropForeignKey(
                name: "FK_Weapons_Units_CaliberUnitId",
                table: "Weapons");

            migrationBuilder.DropColumn(
                name: "Model",
                table: "Weapons");

            migrationBuilder.RenameColumn(
                name: "YearOfManufacture",
                table: "Weapons",
                newName: "Capacity");

            migrationBuilder.RenameColumn(
                name: "CountryOfManufactureId",
                table: "Weapons",
                newName: "WeightUnitId");

            migrationBuilder.RenameColumn(
                name: "CaliberUnitId",
                table: "Weapons",
                newName: "OverallLengthUnitId");

            migrationBuilder.RenameIndex(
                name: "IX_Weapons_CountryOfManufactureId",
                table: "Weapons",
                newName: "IX_Weapons_WeightUnitId");

            migrationBuilder.RenameIndex(
                name: "IX_Weapons_CaliberUnitId",
                table: "Weapons",
                newName: "IX_Weapons_OverallLengthUnitId");

            migrationBuilder.AddColumn<int>(
                name: "ActionType",
                table: "Weapons",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "BarrelLength",
                table: "Weapons",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "BarrelLengthUnitId",
                table: "Weapons",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "OverallLength",
                table: "Weapons",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WeaponType",
                table: "Weapons",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Weight",
                table: "Weapons",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 129, DateTimeKind.Unspecified).AddTicks(1387));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 129, DateTimeKind.Unspecified).AddTicks(1411));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 129, DateTimeKind.Unspecified).AddTicks(1412));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 129, DateTimeKind.Unspecified).AddTicks(1414));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 129, DateTimeKind.Unspecified).AddTicks(1415));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 129, DateTimeKind.Unspecified).AddTicks(5530));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 129, DateTimeKind.Unspecified).AddTicks(5550));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 129, DateTimeKind.Unspecified).AddTicks(5552));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 129, DateTimeKind.Unspecified).AddTicks(5553));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 129, DateTimeKind.Unspecified).AddTicks(5555));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 129, DateTimeKind.Unspecified).AddTicks(8730));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 129, DateTimeKind.Unspecified).AddTicks(8749));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 129, DateTimeKind.Unspecified).AddTicks(8751));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 129, DateTimeKind.Unspecified).AddTicks(8753));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 129, DateTimeKind.Unspecified).AddTicks(8754));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 129, DateTimeKind.Unspecified).AddTicks(8755));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 129, DateTimeKind.Unspecified).AddTicks(8756));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 129, DateTimeKind.Unspecified).AddTicks(8758));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 129, DateTimeKind.Unspecified).AddTicks(8759));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 129, DateTimeKind.Unspecified).AddTicks(8760));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 129, DateTimeKind.Unspecified).AddTicks(8761));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 129, DateTimeKind.Unspecified).AddTicks(8763));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 129, DateTimeKind.Unspecified).AddTicks(8764));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 129, DateTimeKind.Unspecified).AddTicks(8765));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 15L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 129, DateTimeKind.Unspecified).AddTicks(8766));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 16L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 129, DateTimeKind.Unspecified).AddTicks(8768));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 17L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 129, DateTimeKind.Unspecified).AddTicks(8769));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 18L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 129, DateTimeKind.Unspecified).AddTicks(8770));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 130, DateTimeKind.Unspecified).AddTicks(497));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 130, DateTimeKind.Unspecified).AddTicks(513));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 130, DateTimeKind.Unspecified).AddTicks(515));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 130, DateTimeKind.Unspecified).AddTicks(517));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 130, DateTimeKind.Unspecified).AddTicks(7747));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 130, DateTimeKind.Unspecified).AddTicks(7769));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 130, DateTimeKind.Unspecified).AddTicks(7771));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 130, DateTimeKind.Unspecified).AddTicks(7772));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 130, DateTimeKind.Unspecified).AddTicks(7773));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 131, DateTimeKind.Unspecified).AddTicks(7099));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 131, DateTimeKind.Unspecified).AddTicks(7124));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 131, DateTimeKind.Unspecified).AddTicks(7127));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 131, DateTimeKind.Unspecified).AddTicks(7128));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 131, DateTimeKind.Unspecified).AddTicks(7130));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 131, DateTimeKind.Unspecified).AddTicks(8605));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 131, DateTimeKind.Unspecified).AddTicks(8616));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 131, DateTimeKind.Unspecified).AddTicks(8617));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 131, DateTimeKind.Unspecified).AddTicks(8619));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 131, DateTimeKind.Unspecified).AddTicks(8620));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 131, DateTimeKind.Unspecified).AddTicks(8621));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 131, DateTimeKind.Unspecified).AddTicks(8623));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 131, DateTimeKind.Unspecified).AddTicks(8624));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 131, DateTimeKind.Unspecified).AddTicks(8625));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 131, DateTimeKind.Unspecified).AddTicks(8626));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 131, DateTimeKind.Unspecified).AddTicks(8627));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 132, DateTimeKind.Unspecified).AddTicks(6284));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 132, DateTimeKind.Unspecified).AddTicks(6309));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 132, DateTimeKind.Unspecified).AddTicks(6311));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 132, DateTimeKind.Unspecified).AddTicks(6312));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 132, DateTimeKind.Unspecified).AddTicks(6313));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 132, DateTimeKind.Unspecified).AddTicks(7737));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 132, DateTimeKind.Unspecified).AddTicks(7747));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 132, DateTimeKind.Unspecified).AddTicks(7749));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 132, DateTimeKind.Unspecified).AddTicks(7751));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 132, DateTimeKind.Unspecified).AddTicks(7752));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 132, DateTimeKind.Unspecified).AddTicks(9034));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 132, DateTimeKind.Unspecified).AddTicks(9044));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 132, DateTimeKind.Unspecified).AddTicks(9046));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 132, DateTimeKind.Unspecified).AddTicks(9047));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 132, DateTimeKind.Unspecified).AddTicks(9049));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 133, DateTimeKind.Unspecified).AddTicks(354));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 133, DateTimeKind.Unspecified).AddTicks(364));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 133, DateTimeKind.Unspecified).AddTicks(366));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 133, DateTimeKind.Unspecified).AddTicks(367));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 133, DateTimeKind.Unspecified).AddTicks(368));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 133, DateTimeKind.Unspecified).AddTicks(370));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 133, DateTimeKind.Unspecified).AddTicks(371));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 133, DateTimeKind.Unspecified).AddTicks(372));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 133, DateTimeKind.Unspecified).AddTicks(373));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 133, DateTimeKind.Unspecified).AddTicks(374));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 133, DateTimeKind.Unspecified).AddTicks(3907));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 133, DateTimeKind.Unspecified).AddTicks(3930));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 133, DateTimeKind.Unspecified).AddTicks(3931));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 133, DateTimeKind.Unspecified).AddTicks(3933));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 133, DateTimeKind.Unspecified).AddTicks(3936));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 133, DateTimeKind.Unspecified).AddTicks(3937));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 133, DateTimeKind.Unspecified).AddTicks(3938));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 133, DateTimeKind.Unspecified).AddTicks(3939));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 133, DateTimeKind.Unspecified).AddTicks(3940));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 133, DateTimeKind.Unspecified).AddTicks(3941));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 133, DateTimeKind.Unspecified).AddTicks(3942));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 133, DateTimeKind.Unspecified).AddTicks(3934));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 133, DateTimeKind.Unspecified).AddTicks(5908));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 133, DateTimeKind.Unspecified).AddTicks(5925));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 133, DateTimeKind.Unspecified).AddTicks(5927));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 133, DateTimeKind.Unspecified).AddTicks(5928));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 133, DateTimeKind.Unspecified).AddTicks(5929));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 133, DateTimeKind.Unspecified).AddTicks(7188));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 133, DateTimeKind.Unspecified).AddTicks(7200));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 133, DateTimeKind.Unspecified).AddTicks(7202));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 133, DateTimeKind.Unspecified).AddTicks(7203));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 133, DateTimeKind.Unspecified).AddTicks(7205));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 134, DateTimeKind.Unspecified).AddTicks(2906));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 134, DateTimeKind.Unspecified).AddTicks(2928));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 134, DateTimeKind.Unspecified).AddTicks(2930));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 135, DateTimeKind.Unspecified).AddTicks(8411));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 135, DateTimeKind.Unspecified).AddTicks(8514));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 23, 9, 58, 7, 135, DateTimeKind.Unspecified).AddTicks(8517));

            migrationBuilder.CreateIndex(
                name: "IX_Weapons_BarrelLengthUnitId",
                table: "Weapons",
                column: "BarrelLengthUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Weapons_Units_BarrelLengthUnitId",
                table: "Weapons",
                column: "BarrelLengthUnitId",
                principalTable: "Units",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Weapons_Units_OverallLengthUnitId",
                table: "Weapons",
                column: "OverallLengthUnitId",
                principalTable: "Units",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Weapons_Units_WeightUnitId",
                table: "Weapons",
                column: "WeightUnitId",
                principalTable: "Units",
                principalColumn: "Id");
        }
    }
}
