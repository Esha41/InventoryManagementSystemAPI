using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class returnStep : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReturnToStepId",
                table: "WorkflowApprovalSteps",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 159, DateTimeKind.Unspecified).AddTicks(8641));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 159, DateTimeKind.Unspecified).AddTicks(8665));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 159, DateTimeKind.Unspecified).AddTicks(8667));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 159, DateTimeKind.Unspecified).AddTicks(8669));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 159, DateTimeKind.Unspecified).AddTicks(8671));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 160, DateTimeKind.Unspecified).AddTicks(1788));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 160, DateTimeKind.Unspecified).AddTicks(1807));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 160, DateTimeKind.Unspecified).AddTicks(1809));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 160, DateTimeKind.Unspecified).AddTicks(1811));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 160, DateTimeKind.Unspecified).AddTicks(1813));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 160, DateTimeKind.Unspecified).AddTicks(5404));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 160, DateTimeKind.Unspecified).AddTicks(5424));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 160, DateTimeKind.Unspecified).AddTicks(5426));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 160, DateTimeKind.Unspecified).AddTicks(5428));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 160, DateTimeKind.Unspecified).AddTicks(5430));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 160, DateTimeKind.Unspecified).AddTicks(5432));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 160, DateTimeKind.Unspecified).AddTicks(5433));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 160, DateTimeKind.Unspecified).AddTicks(5435));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 160, DateTimeKind.Unspecified).AddTicks(5437));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 160, DateTimeKind.Unspecified).AddTicks(5438));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 160, DateTimeKind.Unspecified).AddTicks(5440));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 160, DateTimeKind.Unspecified).AddTicks(5441));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 160, DateTimeKind.Unspecified).AddTicks(5443));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 160, DateTimeKind.Unspecified).AddTicks(5445));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 15L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 160, DateTimeKind.Unspecified).AddTicks(5446));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 16L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 160, DateTimeKind.Unspecified).AddTicks(5825));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 17L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 160, DateTimeKind.Unspecified).AddTicks(5827));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 18L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 160, DateTimeKind.Unspecified).AddTicks(5829));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 160, DateTimeKind.Unspecified).AddTicks(7863));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 160, DateTimeKind.Unspecified).AddTicks(7879));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 160, DateTimeKind.Unspecified).AddTicks(7881));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 160, DateTimeKind.Unspecified).AddTicks(7884));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 161, DateTimeKind.Unspecified).AddTicks(9940));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 161, DateTimeKind.Unspecified).AddTicks(9963));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 161, DateTimeKind.Unspecified).AddTicks(9965));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 161, DateTimeKind.Unspecified).AddTicks(9967));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 161, DateTimeKind.Unspecified).AddTicks(9969));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 162, DateTimeKind.Unspecified).AddTicks(8478));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 162, DateTimeKind.Unspecified).AddTicks(8502));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 162, DateTimeKind.Unspecified).AddTicks(8504));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 162, DateTimeKind.Unspecified).AddTicks(8506));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 162, DateTimeKind.Unspecified).AddTicks(8507));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 163, DateTimeKind.Unspecified).AddTicks(119));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 163, DateTimeKind.Unspecified).AddTicks(130));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 163, DateTimeKind.Unspecified).AddTicks(132));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 163, DateTimeKind.Unspecified).AddTicks(134));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 163, DateTimeKind.Unspecified).AddTicks(135));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 163, DateTimeKind.Unspecified).AddTicks(137));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 163, DateTimeKind.Unspecified).AddTicks(138));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 163, DateTimeKind.Unspecified).AddTicks(140));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 163, DateTimeKind.Unspecified).AddTicks(141));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 163, DateTimeKind.Unspecified).AddTicks(143));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 163, DateTimeKind.Unspecified).AddTicks(144));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 163, DateTimeKind.Unspecified).AddTicks(8963));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 163, DateTimeKind.Unspecified).AddTicks(8987));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 163, DateTimeKind.Unspecified).AddTicks(8989));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 163, DateTimeKind.Unspecified).AddTicks(8991));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 163, DateTimeKind.Unspecified).AddTicks(8993));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 164, DateTimeKind.Unspecified).AddTicks(772));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 164, DateTimeKind.Unspecified).AddTicks(792));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 164, DateTimeKind.Unspecified).AddTicks(794));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 164, DateTimeKind.Unspecified).AddTicks(796));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 164, DateTimeKind.Unspecified).AddTicks(798));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 164, DateTimeKind.Unspecified).AddTicks(2573));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 164, DateTimeKind.Unspecified).AddTicks(2585));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 164, DateTimeKind.Unspecified).AddTicks(2587));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 164, DateTimeKind.Unspecified).AddTicks(2588));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 164, DateTimeKind.Unspecified).AddTicks(2590));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 164, DateTimeKind.Unspecified).AddTicks(4131));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 164, DateTimeKind.Unspecified).AddTicks(4142));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 164, DateTimeKind.Unspecified).AddTicks(4144));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 164, DateTimeKind.Unspecified).AddTicks(4146));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 164, DateTimeKind.Unspecified).AddTicks(4148));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 164, DateTimeKind.Unspecified).AddTicks(4149));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 164, DateTimeKind.Unspecified).AddTicks(4151));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 164, DateTimeKind.Unspecified).AddTicks(4152));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 164, DateTimeKind.Unspecified).AddTicks(4154));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 164, DateTimeKind.Unspecified).AddTicks(4155));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 164, DateTimeKind.Unspecified).AddTicks(8649));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 164, DateTimeKind.Unspecified).AddTicks(8672));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 164, DateTimeKind.Unspecified).AddTicks(8675));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 164, DateTimeKind.Unspecified).AddTicks(8676));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 164, DateTimeKind.Unspecified).AddTicks(8679));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 164, DateTimeKind.Unspecified).AddTicks(8681));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 164, DateTimeKind.Unspecified).AddTicks(8682));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 164, DateTimeKind.Unspecified).AddTicks(8684));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 164, DateTimeKind.Unspecified).AddTicks(8685));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 164, DateTimeKind.Unspecified).AddTicks(8687));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 164, DateTimeKind.Unspecified).AddTicks(8688));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 164, DateTimeKind.Unspecified).AddTicks(8678));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 165, DateTimeKind.Unspecified).AddTicks(816));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 165, DateTimeKind.Unspecified).AddTicks(834));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 165, DateTimeKind.Unspecified).AddTicks(836));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 165, DateTimeKind.Unspecified).AddTicks(837));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 165, DateTimeKind.Unspecified).AddTicks(839));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 165, DateTimeKind.Unspecified).AddTicks(2261));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 165, DateTimeKind.Unspecified).AddTicks(2271));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 165, DateTimeKind.Unspecified).AddTicks(2273));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 165, DateTimeKind.Unspecified).AddTicks(2275));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 165, DateTimeKind.Unspecified).AddTicks(2277));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 165, DateTimeKind.Unspecified).AddTicks(8564));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 165, DateTimeKind.Unspecified).AddTicks(8586));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 165, DateTimeKind.Unspecified).AddTicks(8588));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 167, DateTimeKind.Unspecified).AddTicks(4537));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 167, DateTimeKind.Unspecified).AddTicks(4559));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 16, 23, 28, 167, DateTimeKind.Unspecified).AddTicks(4561));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReturnToStepId",
                table: "WorkflowApprovalSteps");

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 773, DateTimeKind.Unspecified).AddTicks(680));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 773, DateTimeKind.Unspecified).AddTicks(701));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 773, DateTimeKind.Unspecified).AddTicks(703));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 773, DateTimeKind.Unspecified).AddTicks(704));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 773, DateTimeKind.Unspecified).AddTicks(705));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 773, DateTimeKind.Unspecified).AddTicks(3523));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 773, DateTimeKind.Unspecified).AddTicks(3538));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 773, DateTimeKind.Unspecified).AddTicks(3539));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 773, DateTimeKind.Unspecified).AddTicks(3541));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 773, DateTimeKind.Unspecified).AddTicks(3542));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 773, DateTimeKind.Unspecified).AddTicks(6738));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 773, DateTimeKind.Unspecified).AddTicks(6754));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 773, DateTimeKind.Unspecified).AddTicks(6756));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 773, DateTimeKind.Unspecified).AddTicks(6758));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 773, DateTimeKind.Unspecified).AddTicks(6759));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 773, DateTimeKind.Unspecified).AddTicks(6761));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 773, DateTimeKind.Unspecified).AddTicks(6762));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 773, DateTimeKind.Unspecified).AddTicks(6763));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 773, DateTimeKind.Unspecified).AddTicks(6765));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 773, DateTimeKind.Unspecified).AddTicks(6766));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 773, DateTimeKind.Unspecified).AddTicks(6767));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 773, DateTimeKind.Unspecified).AddTicks(6769));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 773, DateTimeKind.Unspecified).AddTicks(6770));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 773, DateTimeKind.Unspecified).AddTicks(6771));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 15L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 773, DateTimeKind.Unspecified).AddTicks(6772));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 16L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 773, DateTimeKind.Unspecified).AddTicks(6774));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 17L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 773, DateTimeKind.Unspecified).AddTicks(6775));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 18L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 773, DateTimeKind.Unspecified).AddTicks(6776));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 773, DateTimeKind.Unspecified).AddTicks(8409));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 773, DateTimeKind.Unspecified).AddTicks(8422));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 773, DateTimeKind.Unspecified).AddTicks(8424));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 773, DateTimeKind.Unspecified).AddTicks(8426));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 774, DateTimeKind.Unspecified).AddTicks(4479));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 774, DateTimeKind.Unspecified).AddTicks(4499));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 774, DateTimeKind.Unspecified).AddTicks(4500));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 774, DateTimeKind.Unspecified).AddTicks(4502));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 774, DateTimeKind.Unspecified).AddTicks(4503));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 775, DateTimeKind.Unspecified).AddTicks(2145));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 775, DateTimeKind.Unspecified).AddTicks(2164));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 775, DateTimeKind.Unspecified).AddTicks(2166));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 775, DateTimeKind.Unspecified).AddTicks(2167));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 775, DateTimeKind.Unspecified).AddTicks(2169));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 775, DateTimeKind.Unspecified).AddTicks(3585));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 775, DateTimeKind.Unspecified).AddTicks(3594));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 775, DateTimeKind.Unspecified).AddTicks(3595));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 775, DateTimeKind.Unspecified).AddTicks(3597));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 775, DateTimeKind.Unspecified).AddTicks(3598));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 775, DateTimeKind.Unspecified).AddTicks(3599));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 775, DateTimeKind.Unspecified).AddTicks(3600));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 775, DateTimeKind.Unspecified).AddTicks(3602));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 775, DateTimeKind.Unspecified).AddTicks(3603));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 775, DateTimeKind.Unspecified).AddTicks(3604));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 775, DateTimeKind.Unspecified).AddTicks(3605));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 776, DateTimeKind.Unspecified).AddTicks(1314));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 776, DateTimeKind.Unspecified).AddTicks(1334));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 776, DateTimeKind.Unspecified).AddTicks(1336));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 776, DateTimeKind.Unspecified).AddTicks(1337));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 776, DateTimeKind.Unspecified).AddTicks(1338));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 776, DateTimeKind.Unspecified).AddTicks(2673));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 776, DateTimeKind.Unspecified).AddTicks(2683));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 776, DateTimeKind.Unspecified).AddTicks(2684));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 776, DateTimeKind.Unspecified).AddTicks(2686));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 776, DateTimeKind.Unspecified).AddTicks(2687));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 776, DateTimeKind.Unspecified).AddTicks(3943));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 776, DateTimeKind.Unspecified).AddTicks(3952));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 776, DateTimeKind.Unspecified).AddTicks(3974));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 776, DateTimeKind.Unspecified).AddTicks(3977));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 776, DateTimeKind.Unspecified).AddTicks(3978));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 776, DateTimeKind.Unspecified).AddTicks(5357));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 776, DateTimeKind.Unspecified).AddTicks(5367));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 776, DateTimeKind.Unspecified).AddTicks(5369));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 776, DateTimeKind.Unspecified).AddTicks(5370));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 776, DateTimeKind.Unspecified).AddTicks(5372));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 776, DateTimeKind.Unspecified).AddTicks(5373));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 776, DateTimeKind.Unspecified).AddTicks(5374));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 776, DateTimeKind.Unspecified).AddTicks(5375));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 776, DateTimeKind.Unspecified).AddTicks(5377));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 776, DateTimeKind.Unspecified).AddTicks(5378));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 776, DateTimeKind.Unspecified).AddTicks(8881));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 776, DateTimeKind.Unspecified).AddTicks(8901));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 776, DateTimeKind.Unspecified).AddTicks(8903));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 776, DateTimeKind.Unspecified).AddTicks(8905));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 776, DateTimeKind.Unspecified).AddTicks(8907));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 776, DateTimeKind.Unspecified).AddTicks(8909));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 776, DateTimeKind.Unspecified).AddTicks(8910));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 776, DateTimeKind.Unspecified).AddTicks(8911));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 776, DateTimeKind.Unspecified).AddTicks(8912));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 776, DateTimeKind.Unspecified).AddTicks(8913));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 776, DateTimeKind.Unspecified).AddTicks(8914));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 776, DateTimeKind.Unspecified).AddTicks(8906));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 777, DateTimeKind.Unspecified).AddTicks(925));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 777, DateTimeKind.Unspecified).AddTicks(941));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 777, DateTimeKind.Unspecified).AddTicks(942));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 777, DateTimeKind.Unspecified).AddTicks(944));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 777, DateTimeKind.Unspecified).AddTicks(945));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 777, DateTimeKind.Unspecified).AddTicks(2240));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 777, DateTimeKind.Unspecified).AddTicks(2250));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 777, DateTimeKind.Unspecified).AddTicks(2251));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 777, DateTimeKind.Unspecified).AddTicks(2253));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 777, DateTimeKind.Unspecified).AddTicks(2254));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 777, DateTimeKind.Unspecified).AddTicks(7825));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 777, DateTimeKind.Unspecified).AddTicks(7844));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 777, DateTimeKind.Unspecified).AddTicks(7845));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 779, DateTimeKind.Unspecified).AddTicks(2023));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 779, DateTimeKind.Unspecified).AddTicks(2044));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 9, 7, 39, 34, 779, DateTimeKind.Unspecified).AddTicks(2046));
        }
    }
}
