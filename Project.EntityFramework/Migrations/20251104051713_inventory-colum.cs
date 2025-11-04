using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class inventorycolum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Inventories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionDate",
                table: "Inventories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Inventories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 757, DateTimeKind.Unspecified).AddTicks(2344));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 757, DateTimeKind.Unspecified).AddTicks(2406));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 757, DateTimeKind.Unspecified).AddTicks(2408));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 757, DateTimeKind.Unspecified).AddTicks(2409));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 757, DateTimeKind.Unspecified).AddTicks(2410));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 757, DateTimeKind.Unspecified).AddTicks(5366));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 757, DateTimeKind.Unspecified).AddTicks(5384));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 757, DateTimeKind.Unspecified).AddTicks(5386));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 757, DateTimeKind.Unspecified).AddTicks(5388));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 757, DateTimeKind.Unspecified).AddTicks(5389));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 757, DateTimeKind.Unspecified).AddTicks(8619));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 757, DateTimeKind.Unspecified).AddTicks(8636));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 757, DateTimeKind.Unspecified).AddTicks(8638));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 757, DateTimeKind.Unspecified).AddTicks(8640));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 757, DateTimeKind.Unspecified).AddTicks(8641));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 758, DateTimeKind.Unspecified).AddTicks(182));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 758, DateTimeKind.Unspecified).AddTicks(200));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 758, DateTimeKind.Unspecified).AddTicks(202));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 758, DateTimeKind.Unspecified).AddTicks(204));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 758, DateTimeKind.Unspecified).AddTicks(1764));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 758, DateTimeKind.Unspecified).AddTicks(1775));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 758, DateTimeKind.Unspecified).AddTicks(1776));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 758, DateTimeKind.Unspecified).AddTicks(1778));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 758, DateTimeKind.Unspecified).AddTicks(1779));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 758, DateTimeKind.Unspecified).AddTicks(3354));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 758, DateTimeKind.Unspecified).AddTicks(3365));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 758, DateTimeKind.Unspecified).AddTicks(3367));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 758, DateTimeKind.Unspecified).AddTicks(3368));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 758, DateTimeKind.Unspecified).AddTicks(3369));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 759, DateTimeKind.Unspecified).AddTicks(896));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 759, DateTimeKind.Unspecified).AddTicks(919));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 759, DateTimeKind.Unspecified).AddTicks(920));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 759, DateTimeKind.Unspecified).AddTicks(922));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 759, DateTimeKind.Unspecified).AddTicks(923));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 759, DateTimeKind.Unspecified).AddTicks(2424));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 759, DateTimeKind.Unspecified).AddTicks(2434));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 759, DateTimeKind.Unspecified).AddTicks(2505));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 759, DateTimeKind.Unspecified).AddTicks(2507));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 759, DateTimeKind.Unspecified).AddTicks(2508));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 759, DateTimeKind.Unspecified).AddTicks(3950));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 759, DateTimeKind.Unspecified).AddTicks(3960));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 759, DateTimeKind.Unspecified).AddTicks(3962));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 759, DateTimeKind.Unspecified).AddTicks(3963));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 759, DateTimeKind.Unspecified).AddTicks(3965));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 759, DateTimeKind.Unspecified).AddTicks(5523));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 759, DateTimeKind.Unspecified).AddTicks(5533));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 759, DateTimeKind.Unspecified).AddTicks(5535));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 759, DateTimeKind.Unspecified).AddTicks(5536));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 759, DateTimeKind.Unspecified).AddTicks(5537));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 759, DateTimeKind.Unspecified).AddTicks(6989));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 759, DateTimeKind.Unspecified).AddTicks(7000));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 759, DateTimeKind.Unspecified).AddTicks(7001));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 759, DateTimeKind.Unspecified).AddTicks(7003));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 759, DateTimeKind.Unspecified).AddTicks(7004));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 759, DateTimeKind.Unspecified).AddTicks(8498));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 759, DateTimeKind.Unspecified).AddTicks(8507));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 759, DateTimeKind.Unspecified).AddTicks(8509));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 759, DateTimeKind.Unspecified).AddTicks(8510));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 759, DateTimeKind.Unspecified).AddTicks(8512));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 759, DateTimeKind.Unspecified).AddTicks(9986));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 759, DateTimeKind.Unspecified).AddTicks(9997));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 759, DateTimeKind.Unspecified).AddTicks(9998));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 759, DateTimeKind.Unspecified).AddTicks(9999));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 760, DateTimeKind.Unspecified).AddTicks(1));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 760, DateTimeKind.Unspecified).AddTicks(1547));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 760, DateTimeKind.Unspecified).AddTicks(1558));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 760, DateTimeKind.Unspecified).AddTicks(1560));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 760, DateTimeKind.Unspecified).AddTicks(1562));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 8, 17, 12, 760, DateTimeKind.Unspecified).AddTicks(1563));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "DeletionDate",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Inventories");

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 132, DateTimeKind.Unspecified).AddTicks(2457));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 132, DateTimeKind.Unspecified).AddTicks(2576));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 132, DateTimeKind.Unspecified).AddTicks(2578));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 132, DateTimeKind.Unspecified).AddTicks(2580));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 132, DateTimeKind.Unspecified).AddTicks(2605));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 132, DateTimeKind.Unspecified).AddTicks(6022));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 132, DateTimeKind.Unspecified).AddTicks(6041));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 132, DateTimeKind.Unspecified).AddTicks(6075));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 132, DateTimeKind.Unspecified).AddTicks(6077));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 132, DateTimeKind.Unspecified).AddTicks(6079));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 132, DateTimeKind.Unspecified).AddTicks(9290));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 132, DateTimeKind.Unspecified).AddTicks(9311));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 132, DateTimeKind.Unspecified).AddTicks(9313));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 132, DateTimeKind.Unspecified).AddTicks(9315));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 132, DateTimeKind.Unspecified).AddTicks(9317));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 133, DateTimeKind.Unspecified).AddTicks(858));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 133, DateTimeKind.Unspecified).AddTicks(876));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 133, DateTimeKind.Unspecified).AddTicks(878));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 133, DateTimeKind.Unspecified).AddTicks(881));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 133, DateTimeKind.Unspecified).AddTicks(2333));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 133, DateTimeKind.Unspecified).AddTicks(2344));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 133, DateTimeKind.Unspecified).AddTicks(2346));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 133, DateTimeKind.Unspecified).AddTicks(2348));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 133, DateTimeKind.Unspecified).AddTicks(2350));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 133, DateTimeKind.Unspecified).AddTicks(3797));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 133, DateTimeKind.Unspecified).AddTicks(3808));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 133, DateTimeKind.Unspecified).AddTicks(3810));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 133, DateTimeKind.Unspecified).AddTicks(3812));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 133, DateTimeKind.Unspecified).AddTicks(3813));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(715));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(760));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(765));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(767));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(769));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(2283));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(2294));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(2296));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(2298));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(2299));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(3676));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(3685));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(3687));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(3689));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(3690));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(5029));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(5040));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(5042));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(5044));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(5046));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(6426));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(6437));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(6439));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(6440));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(6442));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(7775));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(7786));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(7788));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(7789));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(7791));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(9106));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(9116));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(9118));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(9120));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(9121));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 135, DateTimeKind.Unspecified).AddTicks(383));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 135, DateTimeKind.Unspecified).AddTicks(393));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 135, DateTimeKind.Unspecified).AddTicks(395));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 135, DateTimeKind.Unspecified).AddTicks(396));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 135, DateTimeKind.Unspecified).AddTicks(398));
        }
    }
}
