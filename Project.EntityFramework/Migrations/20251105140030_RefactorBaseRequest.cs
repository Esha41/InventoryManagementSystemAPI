using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class RefactorBaseRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderNo",
                table: "BaseRequests",
                newName: "RequestNo");

            migrationBuilder.RenameIndex(
                name: "IX_BaseRequests_OrderNo",
                table: "BaseRequests",
                newName: "IX_BaseRequests_RequestNo");

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 900, DateTimeKind.Unspecified).AddTicks(4857));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 900, DateTimeKind.Unspecified).AddTicks(4897));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 900, DateTimeKind.Unspecified).AddTicks(4899));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 900, DateTimeKind.Unspecified).AddTicks(4900));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 900, DateTimeKind.Unspecified).AddTicks(4901));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 900, DateTimeKind.Unspecified).AddTicks(7781));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 900, DateTimeKind.Unspecified).AddTicks(7799));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 900, DateTimeKind.Unspecified).AddTicks(7801));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 900, DateTimeKind.Unspecified).AddTicks(7802));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 900, DateTimeKind.Unspecified).AddTicks(7804));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 901, DateTimeKind.Unspecified).AddTicks(1583));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 901, DateTimeKind.Unspecified).AddTicks(1607));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 901, DateTimeKind.Unspecified).AddTicks(1609));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 901, DateTimeKind.Unspecified).AddTicks(1610));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 901, DateTimeKind.Unspecified).AddTicks(1612));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 901, DateTimeKind.Unspecified).AddTicks(3062));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 901, DateTimeKind.Unspecified).AddTicks(3073));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 901, DateTimeKind.Unspecified).AddTicks(3075));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 901, DateTimeKind.Unspecified).AddTicks(3077));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 901, DateTimeKind.Unspecified).AddTicks(5816));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 901, DateTimeKind.Unspecified).AddTicks(5838));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 901, DateTimeKind.Unspecified).AddTicks(5840));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 901, DateTimeKind.Unspecified).AddTicks(5841));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 901, DateTimeKind.Unspecified).AddTicks(5843));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 901, DateTimeKind.Unspecified).AddTicks(5845));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 901, DateTimeKind.Unspecified).AddTicks(5847));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 901, DateTimeKind.Unspecified).AddTicks(5848));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 901, DateTimeKind.Unspecified).AddTicks(5850));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 901, DateTimeKind.Unspecified).AddTicks(5890));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 901, DateTimeKind.Unspecified).AddTicks(7394));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 901, DateTimeKind.Unspecified).AddTicks(7406));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 901, DateTimeKind.Unspecified).AddTicks(7408));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 901, DateTimeKind.Unspecified).AddTicks(7409));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 901, DateTimeKind.Unspecified).AddTicks(7411));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 901, DateTimeKind.Unspecified).AddTicks(8849));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 901, DateTimeKind.Unspecified).AddTicks(8862));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 901, DateTimeKind.Unspecified).AddTicks(8863));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 901, DateTimeKind.Unspecified).AddTicks(8865));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 901, DateTimeKind.Unspecified).AddTicks(8866));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 902, DateTimeKind.Unspecified).AddTicks(5802));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 902, DateTimeKind.Unspecified).AddTicks(5822));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 902, DateTimeKind.Unspecified).AddTicks(5824));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 902, DateTimeKind.Unspecified).AddTicks(5825));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 902, DateTimeKind.Unspecified).AddTicks(5827));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 902, DateTimeKind.Unspecified).AddTicks(7251));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 902, DateTimeKind.Unspecified).AddTicks(7260));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 902, DateTimeKind.Unspecified).AddTicks(7261));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 902, DateTimeKind.Unspecified).AddTicks(7263));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 902, DateTimeKind.Unspecified).AddTicks(7264));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 902, DateTimeKind.Unspecified).AddTicks(8658));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 902, DateTimeKind.Unspecified).AddTicks(8672));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 902, DateTimeKind.Unspecified).AddTicks(8674));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 902, DateTimeKind.Unspecified).AddTicks(8675));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 902, DateTimeKind.Unspecified).AddTicks(8676));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 903, DateTimeKind.Unspecified).AddTicks(1396));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 903, DateTimeKind.Unspecified).AddTicks(1411));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 903, DateTimeKind.Unspecified).AddTicks(1412));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 903, DateTimeKind.Unspecified).AddTicks(1414));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 903, DateTimeKind.Unspecified).AddTicks(1415));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 903, DateTimeKind.Unspecified).AddTicks(2755));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 903, DateTimeKind.Unspecified).AddTicks(2763));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 903, DateTimeKind.Unspecified).AddTicks(2764));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 903, DateTimeKind.Unspecified).AddTicks(2766));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 903, DateTimeKind.Unspecified).AddTicks(2767));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 903, DateTimeKind.Unspecified).AddTicks(4121));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 903, DateTimeKind.Unspecified).AddTicks(4129));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 903, DateTimeKind.Unspecified).AddTicks(4130));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 903, DateTimeKind.Unspecified).AddTicks(4131));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 903, DateTimeKind.Unspecified).AddTicks(4132));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 903, DateTimeKind.Unspecified).AddTicks(5508));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 903, DateTimeKind.Unspecified).AddTicks(5516));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 903, DateTimeKind.Unspecified).AddTicks(5518));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 903, DateTimeKind.Unspecified).AddTicks(5519));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 903, DateTimeKind.Unspecified).AddTicks(5521));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 903, DateTimeKind.Unspecified).AddTicks(5522));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 903, DateTimeKind.Unspecified).AddTicks(5523));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 903, DateTimeKind.Unspecified).AddTicks(5524));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 903, DateTimeKind.Unspecified).AddTicks(5525));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 903, DateTimeKind.Unspecified).AddTicks(5526));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 903, DateTimeKind.Unspecified).AddTicks(9172));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 903, DateTimeKind.Unspecified).AddTicks(9197));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 903, DateTimeKind.Unspecified).AddTicks(9229));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 903, DateTimeKind.Unspecified).AddTicks(9231));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 903, DateTimeKind.Unspecified).AddTicks(9232));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 903, DateTimeKind.Unspecified).AddTicks(9233));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 903, DateTimeKind.Unspecified).AddTicks(9234));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 903, DateTimeKind.Unspecified).AddTicks(9236));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 903, DateTimeKind.Unspecified).AddTicks(9237));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 903, DateTimeKind.Unspecified).AddTicks(9238));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 903, DateTimeKind.Unspecified).AddTicks(9240));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 904, DateTimeKind.Unspecified).AddTicks(1550));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 904, DateTimeKind.Unspecified).AddTicks(1564));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 904, DateTimeKind.Unspecified).AddTicks(1565));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 904, DateTimeKind.Unspecified).AddTicks(1567));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 904, DateTimeKind.Unspecified).AddTicks(1568));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 904, DateTimeKind.Unspecified).AddTicks(3052));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 904, DateTimeKind.Unspecified).AddTicks(3062));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 904, DateTimeKind.Unspecified).AddTicks(3064));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 904, DateTimeKind.Unspecified).AddTicks(3065));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 17, 0, 29, 904, DateTimeKind.Unspecified).AddTicks(3066));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RequestNo",
                table: "BaseRequests",
                newName: "OrderNo");

            migrationBuilder.RenameIndex(
                name: "IX_BaseRequests_RequestNo",
                table: "BaseRequests",
                newName: "IX_BaseRequests_OrderNo");

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 819, DateTimeKind.Unspecified).AddTicks(9064));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 819, DateTimeKind.Unspecified).AddTicks(9086));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 819, DateTimeKind.Unspecified).AddTicks(9088));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 819, DateTimeKind.Unspecified).AddTicks(9090));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 819, DateTimeKind.Unspecified).AddTicks(9091));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 820, DateTimeKind.Unspecified).AddTicks(2261));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 820, DateTimeKind.Unspecified).AddTicks(2278));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 820, DateTimeKind.Unspecified).AddTicks(2280));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 820, DateTimeKind.Unspecified).AddTicks(2281));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 820, DateTimeKind.Unspecified).AddTicks(2283));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 820, DateTimeKind.Unspecified).AddTicks(5794));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 820, DateTimeKind.Unspecified).AddTicks(5812));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 820, DateTimeKind.Unspecified).AddTicks(5814));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 820, DateTimeKind.Unspecified).AddTicks(5816));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 820, DateTimeKind.Unspecified).AddTicks(5817));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 820, DateTimeKind.Unspecified).AddTicks(7333));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 820, DateTimeKind.Unspecified).AddTicks(7348));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 820, DateTimeKind.Unspecified).AddTicks(7350));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 820, DateTimeKind.Unspecified).AddTicks(7353));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 821, DateTimeKind.Unspecified).AddTicks(430));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 821, DateTimeKind.Unspecified).AddTicks(454));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 821, DateTimeKind.Unspecified).AddTicks(457));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 821, DateTimeKind.Unspecified).AddTicks(460));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 821, DateTimeKind.Unspecified).AddTicks(462));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 821, DateTimeKind.Unspecified).AddTicks(465));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 821, DateTimeKind.Unspecified).AddTicks(467));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 821, DateTimeKind.Unspecified).AddTicks(527));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 821, DateTimeKind.Unspecified).AddTicks(530));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 821, DateTimeKind.Unspecified).AddTicks(533));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 821, DateTimeKind.Unspecified).AddTicks(2455));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 821, DateTimeKind.Unspecified).AddTicks(2468));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 821, DateTimeKind.Unspecified).AddTicks(2470));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 821, DateTimeKind.Unspecified).AddTicks(2472));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 821, DateTimeKind.Unspecified).AddTicks(2473));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 821, DateTimeKind.Unspecified).AddTicks(4341));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 821, DateTimeKind.Unspecified).AddTicks(4353));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 821, DateTimeKind.Unspecified).AddTicks(4355));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 821, DateTimeKind.Unspecified).AddTicks(4357));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 821, DateTimeKind.Unspecified).AddTicks(4359));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 822, DateTimeKind.Unspecified).AddTicks(2326));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 822, DateTimeKind.Unspecified).AddTicks(2346));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 822, DateTimeKind.Unspecified).AddTicks(2348));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 822, DateTimeKind.Unspecified).AddTicks(2350));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 822, DateTimeKind.Unspecified).AddTicks(2352));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 822, DateTimeKind.Unspecified).AddTicks(3887));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 822, DateTimeKind.Unspecified).AddTicks(3898));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 822, DateTimeKind.Unspecified).AddTicks(3900));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 822, DateTimeKind.Unspecified).AddTicks(3901));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 822, DateTimeKind.Unspecified).AddTicks(3902));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 822, DateTimeKind.Unspecified).AddTicks(5357));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 822, DateTimeKind.Unspecified).AddTicks(5367));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 822, DateTimeKind.Unspecified).AddTicks(5368));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 822, DateTimeKind.Unspecified).AddTicks(5370));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 822, DateTimeKind.Unspecified).AddTicks(5371));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 822, DateTimeKind.Unspecified).AddTicks(8343));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 822, DateTimeKind.Unspecified).AddTicks(8358));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 822, DateTimeKind.Unspecified).AddTicks(8360));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 822, DateTimeKind.Unspecified).AddTicks(8361));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 822, DateTimeKind.Unspecified).AddTicks(8363));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 822, DateTimeKind.Unspecified).AddTicks(9809));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 822, DateTimeKind.Unspecified).AddTicks(9819));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 822, DateTimeKind.Unspecified).AddTicks(9820));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 822, DateTimeKind.Unspecified).AddTicks(9822));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 822, DateTimeKind.Unspecified).AddTicks(9823));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 823, DateTimeKind.Unspecified).AddTicks(1224));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 823, DateTimeKind.Unspecified).AddTicks(1234));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 823, DateTimeKind.Unspecified).AddTicks(1236));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 823, DateTimeKind.Unspecified).AddTicks(1237));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 823, DateTimeKind.Unspecified).AddTicks(1238));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 823, DateTimeKind.Unspecified).AddTicks(2630));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 823, DateTimeKind.Unspecified).AddTicks(2641));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 823, DateTimeKind.Unspecified).AddTicks(2642));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 823, DateTimeKind.Unspecified).AddTicks(2644));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 823, DateTimeKind.Unspecified).AddTicks(2645));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 823, DateTimeKind.Unspecified).AddTicks(2647));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 823, DateTimeKind.Unspecified).AddTicks(2648));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 823, DateTimeKind.Unspecified).AddTicks(2649));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 823, DateTimeKind.Unspecified).AddTicks(2650));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 823, DateTimeKind.Unspecified).AddTicks(2652));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 823, DateTimeKind.Unspecified).AddTicks(6668));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 823, DateTimeKind.Unspecified).AddTicks(6692));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 823, DateTimeKind.Unspecified).AddTicks(6694));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 823, DateTimeKind.Unspecified).AddTicks(6695));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 823, DateTimeKind.Unspecified).AddTicks(6696));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 823, DateTimeKind.Unspecified).AddTicks(6698));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 823, DateTimeKind.Unspecified).AddTicks(6699));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 823, DateTimeKind.Unspecified).AddTicks(6700));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 823, DateTimeKind.Unspecified).AddTicks(6701));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 823, DateTimeKind.Unspecified).AddTicks(6702));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 823, DateTimeKind.Unspecified).AddTicks(6703));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 823, DateTimeKind.Unspecified).AddTicks(9058));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 823, DateTimeKind.Unspecified).AddTicks(9073));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 823, DateTimeKind.Unspecified).AddTicks(9074));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 823, DateTimeKind.Unspecified).AddTicks(9076));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 823, DateTimeKind.Unspecified).AddTicks(9077));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 824, DateTimeKind.Unspecified).AddTicks(542));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 824, DateTimeKind.Unspecified).AddTicks(553));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 824, DateTimeKind.Unspecified).AddTicks(555));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 824, DateTimeKind.Unspecified).AddTicks(556));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 15, 17, 58, 824, DateTimeKind.Unspecified).AddTicks(558));
        }
    }
}
