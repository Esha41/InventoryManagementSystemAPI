using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class SetIsActiveInUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 403, DateTimeKind.Unspecified).AddTicks(9816));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 403, DateTimeKind.Unspecified).AddTicks(9852));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 403, DateTimeKind.Unspecified).AddTicks(9854));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 403, DateTimeKind.Unspecified).AddTicks(9856));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 403, DateTimeKind.Unspecified).AddTicks(9857));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 404, DateTimeKind.Unspecified).AddTicks(4390));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 404, DateTimeKind.Unspecified).AddTicks(4411));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 404, DateTimeKind.Unspecified).AddTicks(4413));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 404, DateTimeKind.Unspecified).AddTicks(4414));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 404, DateTimeKind.Unspecified).AddTicks(4416));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 404, DateTimeKind.Unspecified).AddTicks(7648));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 404, DateTimeKind.Unspecified).AddTicks(7666));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 404, DateTimeKind.Unspecified).AddTicks(7668));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 404, DateTimeKind.Unspecified).AddTicks(7669));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 404, DateTimeKind.Unspecified).AddTicks(7671));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 404, DateTimeKind.Unspecified).AddTicks(7672));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 404, DateTimeKind.Unspecified).AddTicks(7674));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 404, DateTimeKind.Unspecified).AddTicks(7703));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 404, DateTimeKind.Unspecified).AddTicks(7705));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 404, DateTimeKind.Unspecified).AddTicks(7707));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 404, DateTimeKind.Unspecified).AddTicks(7708));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 404, DateTimeKind.Unspecified).AddTicks(7709));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 404, DateTimeKind.Unspecified).AddTicks(7711));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 404, DateTimeKind.Unspecified).AddTicks(7712));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 15L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 404, DateTimeKind.Unspecified).AddTicks(7713));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 16L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 404, DateTimeKind.Unspecified).AddTicks(7714));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 17L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 404, DateTimeKind.Unspecified).AddTicks(7716));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 18L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 404, DateTimeKind.Unspecified).AddTicks(7717));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 404, DateTimeKind.Unspecified).AddTicks(9360));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 404, DateTimeKind.Unspecified).AddTicks(9376));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 404, DateTimeKind.Unspecified).AddTicks(9378));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 404, DateTimeKind.Unspecified).AddTicks(9380));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 405, DateTimeKind.Unspecified).AddTicks(8433));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 405, DateTimeKind.Unspecified).AddTicks(8456));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 405, DateTimeKind.Unspecified).AddTicks(8458));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 405, DateTimeKind.Unspecified).AddTicks(8459));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 405, DateTimeKind.Unspecified).AddTicks(8461));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 407, DateTimeKind.Unspecified).AddTicks(1563));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 407, DateTimeKind.Unspecified).AddTicks(1587));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 407, DateTimeKind.Unspecified).AddTicks(1589));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 407, DateTimeKind.Unspecified).AddTicks(1590));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 407, DateTimeKind.Unspecified).AddTicks(1591));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 407, DateTimeKind.Unspecified).AddTicks(3150));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 407, DateTimeKind.Unspecified).AddTicks(3162));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 407, DateTimeKind.Unspecified).AddTicks(3164));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 407, DateTimeKind.Unspecified).AddTicks(3166));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 407, DateTimeKind.Unspecified).AddTicks(3167));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 407, DateTimeKind.Unspecified).AddTicks(3168));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 407, DateTimeKind.Unspecified).AddTicks(3169));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 407, DateTimeKind.Unspecified).AddTicks(3170));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 407, DateTimeKind.Unspecified).AddTicks(3171));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 407, DateTimeKind.Unspecified).AddTicks(3173));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 407, DateTimeKind.Unspecified).AddTicks(3174));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(1494));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(1516));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(1518));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(1519));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(1521));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(3034));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(3046));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(3047));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(3049));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(3050));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(4499));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(4509));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(4511));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(4512));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(4514));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(5837));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(5847));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(5848));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(5850));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(5851));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(5852));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(5853));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(5855));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(5856));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(5857));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(9541));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(9564));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(9565));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(9567));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(9569));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(9570));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(9571));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(9573));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(9574));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(9575));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(9576));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 408, DateTimeKind.Unspecified).AddTicks(9568));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 409, DateTimeKind.Unspecified).AddTicks(1509));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 409, DateTimeKind.Unspecified).AddTicks(1524));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 409, DateTimeKind.Unspecified).AddTicks(1526));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 409, DateTimeKind.Unspecified).AddTicks(1528));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 409, DateTimeKind.Unspecified).AddTicks(1529));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 409, DateTimeKind.Unspecified).AddTicks(2869));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 409, DateTimeKind.Unspecified).AddTicks(2880));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 409, DateTimeKind.Unspecified).AddTicks(2881));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 409, DateTimeKind.Unspecified).AddTicks(2883));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 409, DateTimeKind.Unspecified).AddTicks(2884));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 409, DateTimeKind.Unspecified).AddTicks(8827));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 409, DateTimeKind.Unspecified).AddTicks(8847));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 409, DateTimeKind.Unspecified).AddTicks(8849));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 411, DateTimeKind.Unspecified).AddTicks(4198));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 411, DateTimeKind.Unspecified).AddTicks(4220));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 11, 16, 37, 22, 411, DateTimeKind.Unspecified).AddTicks(4222));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 622, DateTimeKind.Unspecified).AddTicks(5336));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 622, DateTimeKind.Unspecified).AddTicks(5487));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 622, DateTimeKind.Unspecified).AddTicks(5490));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 622, DateTimeKind.Unspecified).AddTicks(5492));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 622, DateTimeKind.Unspecified).AddTicks(5493));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 623, DateTimeKind.Unspecified).AddTicks(103));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 623, DateTimeKind.Unspecified).AddTicks(152));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 623, DateTimeKind.Unspecified).AddTicks(155));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 623, DateTimeKind.Unspecified).AddTicks(157));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 623, DateTimeKind.Unspecified).AddTicks(158));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 623, DateTimeKind.Unspecified).AddTicks(3628));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 623, DateTimeKind.Unspecified).AddTicks(3651));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 623, DateTimeKind.Unspecified).AddTicks(3653));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 623, DateTimeKind.Unspecified).AddTicks(3654));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 623, DateTimeKind.Unspecified).AddTicks(3656));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 623, DateTimeKind.Unspecified).AddTicks(3658));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 623, DateTimeKind.Unspecified).AddTicks(3660));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 623, DateTimeKind.Unspecified).AddTicks(3661));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 623, DateTimeKind.Unspecified).AddTicks(3663));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 623, DateTimeKind.Unspecified).AddTicks(3665));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 623, DateTimeKind.Unspecified).AddTicks(3680));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 623, DateTimeKind.Unspecified).AddTicks(3719));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 623, DateTimeKind.Unspecified).AddTicks(3722));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 623, DateTimeKind.Unspecified).AddTicks(3724));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 15L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 623, DateTimeKind.Unspecified).AddTicks(3726));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 16L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 623, DateTimeKind.Unspecified).AddTicks(3727));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 17L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 623, DateTimeKind.Unspecified).AddTicks(3729));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 18L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 623, DateTimeKind.Unspecified).AddTicks(3731));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 623, DateTimeKind.Unspecified).AddTicks(5483));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 623, DateTimeKind.Unspecified).AddTicks(5498));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 623, DateTimeKind.Unspecified).AddTicks(5505));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 623, DateTimeKind.Unspecified).AddTicks(5507));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 625, DateTimeKind.Unspecified).AddTicks(804));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 625, DateTimeKind.Unspecified).AddTicks(831));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 625, DateTimeKind.Unspecified).AddTicks(833));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 625, DateTimeKind.Unspecified).AddTicks(834));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 625, DateTimeKind.Unspecified).AddTicks(836));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 626, DateTimeKind.Unspecified).AddTicks(4778));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 626, DateTimeKind.Unspecified).AddTicks(4804));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 626, DateTimeKind.Unspecified).AddTicks(4806));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 626, DateTimeKind.Unspecified).AddTicks(4808));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 626, DateTimeKind.Unspecified).AddTicks(4809));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 626, DateTimeKind.Unspecified).AddTicks(8196));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 626, DateTimeKind.Unspecified).AddTicks(8220));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 626, DateTimeKind.Unspecified).AddTicks(8222));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 626, DateTimeKind.Unspecified).AddTicks(8223));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 626, DateTimeKind.Unspecified).AddTicks(8225));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 626, DateTimeKind.Unspecified).AddTicks(8227));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 626, DateTimeKind.Unspecified).AddTicks(8228));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 626, DateTimeKind.Unspecified).AddTicks(8229));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 626, DateTimeKind.Unspecified).AddTicks(8231));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 626, DateTimeKind.Unspecified).AddTicks(8232));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 626, DateTimeKind.Unspecified).AddTicks(8233));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 627, DateTimeKind.Unspecified).AddTicks(8078));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 627, DateTimeKind.Unspecified).AddTicks(8103));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 627, DateTimeKind.Unspecified).AddTicks(8105));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 627, DateTimeKind.Unspecified).AddTicks(8106));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 627, DateTimeKind.Unspecified).AddTicks(8108));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 627, DateTimeKind.Unspecified).AddTicks(9582));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 627, DateTimeKind.Unspecified).AddTicks(9593));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 627, DateTimeKind.Unspecified).AddTicks(9594));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 627, DateTimeKind.Unspecified).AddTicks(9596));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 627, DateTimeKind.Unspecified).AddTicks(9597));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 628, DateTimeKind.Unspecified).AddTicks(1037));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 628, DateTimeKind.Unspecified).AddTicks(1046));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 628, DateTimeKind.Unspecified).AddTicks(1048));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 628, DateTimeKind.Unspecified).AddTicks(1050));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 628, DateTimeKind.Unspecified).AddTicks(1051));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 628, DateTimeKind.Unspecified).AddTicks(2404));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 628, DateTimeKind.Unspecified).AddTicks(2413));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 628, DateTimeKind.Unspecified).AddTicks(2415));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 628, DateTimeKind.Unspecified).AddTicks(2417));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 628, DateTimeKind.Unspecified).AddTicks(2418));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 628, DateTimeKind.Unspecified).AddTicks(2419));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 628, DateTimeKind.Unspecified).AddTicks(2421));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 628, DateTimeKind.Unspecified).AddTicks(2422));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 628, DateTimeKind.Unspecified).AddTicks(2424));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 628, DateTimeKind.Unspecified).AddTicks(2425));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 628, DateTimeKind.Unspecified).AddTicks(6341));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 628, DateTimeKind.Unspecified).AddTicks(6362));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 628, DateTimeKind.Unspecified).AddTicks(6364));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 628, DateTimeKind.Unspecified).AddTicks(6366));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 628, DateTimeKind.Unspecified).AddTicks(6368));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 628, DateTimeKind.Unspecified).AddTicks(6370));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 628, DateTimeKind.Unspecified).AddTicks(6371));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 628, DateTimeKind.Unspecified).AddTicks(6373));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 628, DateTimeKind.Unspecified).AddTicks(6374));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 628, DateTimeKind.Unspecified).AddTicks(6375));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 628, DateTimeKind.Unspecified).AddTicks(6377));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 628, DateTimeKind.Unspecified).AddTicks(6367));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 628, DateTimeKind.Unspecified).AddTicks(9401));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 628, DateTimeKind.Unspecified).AddTicks(9419));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 628, DateTimeKind.Unspecified).AddTicks(9421));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 628, DateTimeKind.Unspecified).AddTicks(9423));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 628, DateTimeKind.Unspecified).AddTicks(9424));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 629, DateTimeKind.Unspecified).AddTicks(857));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 629, DateTimeKind.Unspecified).AddTicks(865));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 629, DateTimeKind.Unspecified).AddTicks(867));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 629, DateTimeKind.Unspecified).AddTicks(869));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 629, DateTimeKind.Unspecified).AddTicks(870));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 629, DateTimeKind.Unspecified).AddTicks(7067));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 629, DateTimeKind.Unspecified).AddTicks(7089));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 629, DateTimeKind.Unspecified).AddTicks(7090));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 631, DateTimeKind.Unspecified).AddTicks(3651));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 631, DateTimeKind.Unspecified).AddTicks(3679));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 8, 13, 14, 43, 631, DateTimeKind.Unspecified).AddTicks(3681));
        }
    }
}
