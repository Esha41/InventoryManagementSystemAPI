using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class SeedAmmunistions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "BaseItems",
                columns: new[] { "Id", "BatchNo", "CreatedBy", "CreationDate", "DeletedBy", "DeletionDate", "ExpiryDate", "HccId", "IsDeleted", "ItemNo", "ItemType", "ModificationDate", "ModifiedBy", "Name", "PartNo", "ReadyForIssue" },
                values: new object[,]
                {
                    { 1L, "BATCH-2024-001", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2029, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1L, false, "AMM-001", 1, null, null, "5.56x45mm NATO", "PN-556-001", true },
                    { 2L, "BATCH-2024-002", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2029, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2L, false, "AMM-002", 1, null, null, "7.62x51mm NATO", "PN-762-001", true },
                    { 3L, "BATCH-2024-003", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2028, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3L, false, "AMM-003", 1, null, null, "9x19mm Parabellum", "PN-9MM-001", true },
                    { 4L, "BATCH-2024-004", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2030, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1L, false, "AMM-004", 1, null, null, ".50 BMG", "PN-50BMG-001", true },
                    { 5L, "BATCH-2024-005", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2029, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2L, false, "AMM-005", 1, null, null, ".308 Winchester", "PN-308-001", true },
                    { 6L, "BATCH-2024-006", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2028, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3L, false, "AMM-006", 1, null, null, ".45 ACP", "PN-45ACP-001", true },
                    { 7L, "BATCH-2024-007", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2030, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1L, false, "AMM-007", 1, null, null, "12.7x108mm", "PN-127-001", true },
                    { 8L, "BATCH-2024-008", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2029, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2L, false, "AMM-008", 1, null, null, "5.45x39mm", "PN-545-001", true },
                    { 9L, "BATCH-2024-009", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2028, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3L, false, "AMM-009", 1, null, null, ".40 S&W", "PN-40SW-001", true }
                });

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

            migrationBuilder.InsertData(
                table: "Ammunitions",
                columns: new[] { "Id", "BulletDiameter", "BulletDiameterUnitId", "CaseLength", "CaseLengthUnitId", "CaseTypeId", "CompatibilityId", "HazardDivisionId", "IsLinked", "NatureOptionId", "NsnId", "PrimaryPurposId", "Primer", "ProjectailMaterialId", "ProjectileColorId", "PropellantId", "TotalWeight" },
                values: new object[,]
                {
                    { 1L, 5.56m, 1L, 45.0m, 1L, 1L, 1L, 1L, false, 1L, 1L, 1L, "Boxer", 1L, 1L, 1L, 12.0m },
                    { 2L, 7.62m, 2L, 51.0m, 2L, 2L, 2L, 2L, false, 2L, 2L, 2L, "Berdan", 2L, 2L, 2L, 24.0m },
                    { 3L, 9.0m, 3L, 19.0m, 3L, 3L, 3L, 3L, false, 3L, 3L, 3L, "Boxer", 3L, 3L, 3L, 7.5m },
                    { 4L, 12.7m, 1L, 99.0m, 1L, 1L, 1L, 1L, false, 1L, 1L, 1L, "Berdan", 1L, 1L, 1L, 115.0m },
                    { 5L, 7.62m, 2L, 51.0m, 2L, 2L, 2L, 2L, false, 2L, 2L, 2L, "Boxer", 2L, 2L, 2L, 23.0m },
                    { 6L, 11.43m, 3L, 23.0m, 3L, 3L, 3L, 3L, false, 3L, 3L, 3L, "Boxer", 3L, 3L, 3L, 15.0m },
                    { 7L, 12.7m, 1L, 108.0m, 1L, 1L, 1L, 1L, false, 1L, 1L, 1L, "Berdan", 1L, 1L, 1L, 130.0m },
                    { 8L, 5.45m, 2L, 39.0m, 2L, 2L, 2L, 2L, false, 2L, 2L, 2L, "Berdan", 2L, 2L, 2L, 10.5m },
                    { 9L, 10.16m, 3L, 21.6m, 3L, 3L, 3L, 3L, false, 3L, 3L, 3L, "Boxer", 3L, 3L, 3L, 11.0m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Ammunitions",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Ammunitions",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Ammunitions",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "Ammunitions",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "Ammunitions",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "Ammunitions",
                keyColumn: "Id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "Ammunitions",
                keyColumn: "Id",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                table: "Ammunitions",
                keyColumn: "Id",
                keyValue: 8L);

            migrationBuilder.DeleteData(
                table: "Ammunitions",
                keyColumn: "Id",
                keyValue: 9L);

            migrationBuilder.DeleteData(
                table: "BaseItems",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "BaseItems",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "BaseItems",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "BaseItems",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "BaseItems",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "BaseItems",
                keyColumn: "Id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "BaseItems",
                keyColumn: "Id",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                table: "BaseItems",
                keyColumn: "Id",
                keyValue: 8L);

            migrationBuilder.DeleteData(
                table: "BaseItems",
                keyColumn: "Id",
                keyValue: 9L);

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 806, DateTimeKind.Unspecified).AddTicks(1149));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 806, DateTimeKind.Unspecified).AddTicks(1209));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 806, DateTimeKind.Unspecified).AddTicks(1211));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 806, DateTimeKind.Unspecified).AddTicks(1212));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 806, DateTimeKind.Unspecified).AddTicks(1214));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 806, DateTimeKind.Unspecified).AddTicks(4329));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 806, DateTimeKind.Unspecified).AddTicks(4348));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 806, DateTimeKind.Unspecified).AddTicks(4349));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 806, DateTimeKind.Unspecified).AddTicks(4351));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 806, DateTimeKind.Unspecified).AddTicks(4352));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 806, DateTimeKind.Unspecified).AddTicks(7678));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 806, DateTimeKind.Unspecified).AddTicks(7695));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 806, DateTimeKind.Unspecified).AddTicks(7697));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 806, DateTimeKind.Unspecified).AddTicks(7698));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 806, DateTimeKind.Unspecified).AddTicks(7700));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 806, DateTimeKind.Unspecified).AddTicks(9148));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 806, DateTimeKind.Unspecified).AddTicks(9164));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 806, DateTimeKind.Unspecified).AddTicks(9166));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 806, DateTimeKind.Unspecified).AddTicks(9168));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 807, DateTimeKind.Unspecified).AddTicks(1813));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 807, DateTimeKind.Unspecified).AddTicks(1833));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 807, DateTimeKind.Unspecified).AddTicks(1835));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 807, DateTimeKind.Unspecified).AddTicks(1837));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 807, DateTimeKind.Unspecified).AddTicks(1838));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 807, DateTimeKind.Unspecified).AddTicks(1840));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 807, DateTimeKind.Unspecified).AddTicks(1842));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 807, DateTimeKind.Unspecified).AddTicks(1844));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 807, DateTimeKind.Unspecified).AddTicks(1845));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 807, DateTimeKind.Unspecified).AddTicks(1847));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 807, DateTimeKind.Unspecified).AddTicks(3435));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 807, DateTimeKind.Unspecified).AddTicks(3445));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 807, DateTimeKind.Unspecified).AddTicks(3447));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 807, DateTimeKind.Unspecified).AddTicks(3448));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 807, DateTimeKind.Unspecified).AddTicks(3449));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 807, DateTimeKind.Unspecified).AddTicks(4936));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 807, DateTimeKind.Unspecified).AddTicks(4945));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 807, DateTimeKind.Unspecified).AddTicks(4947));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 807, DateTimeKind.Unspecified).AddTicks(4948));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 807, DateTimeKind.Unspecified).AddTicks(4950));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 808, DateTimeKind.Unspecified).AddTicks(2323));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 808, DateTimeKind.Unspecified).AddTicks(2345));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 808, DateTimeKind.Unspecified).AddTicks(2346));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 808, DateTimeKind.Unspecified).AddTicks(2348));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 808, DateTimeKind.Unspecified).AddTicks(2349));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 808, DateTimeKind.Unspecified).AddTicks(3895));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 808, DateTimeKind.Unspecified).AddTicks(3907));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 808, DateTimeKind.Unspecified).AddTicks(3908));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 808, DateTimeKind.Unspecified).AddTicks(3910));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 808, DateTimeKind.Unspecified).AddTicks(3911));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 808, DateTimeKind.Unspecified).AddTicks(5323));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 808, DateTimeKind.Unspecified).AddTicks(5333));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 808, DateTimeKind.Unspecified).AddTicks(5334));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 808, DateTimeKind.Unspecified).AddTicks(5336));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 808, DateTimeKind.Unspecified).AddTicks(5337));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 808, DateTimeKind.Unspecified).AddTicks(8152));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 808, DateTimeKind.Unspecified).AddTicks(8169));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 808, DateTimeKind.Unspecified).AddTicks(8170));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 808, DateTimeKind.Unspecified).AddTicks(8172));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 808, DateTimeKind.Unspecified).AddTicks(8173));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 808, DateTimeKind.Unspecified).AddTicks(9600));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 808, DateTimeKind.Unspecified).AddTicks(9609));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 808, DateTimeKind.Unspecified).AddTicks(9611));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 808, DateTimeKind.Unspecified).AddTicks(9612));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 808, DateTimeKind.Unspecified).AddTicks(9613));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 809, DateTimeKind.Unspecified).AddTicks(1091));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 809, DateTimeKind.Unspecified).AddTicks(1101));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 809, DateTimeKind.Unspecified).AddTicks(1103));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 809, DateTimeKind.Unspecified).AddTicks(1104));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 809, DateTimeKind.Unspecified).AddTicks(1105));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 809, DateTimeKind.Unspecified).AddTicks(2490));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 809, DateTimeKind.Unspecified).AddTicks(2499));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 809, DateTimeKind.Unspecified).AddTicks(2501));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 809, DateTimeKind.Unspecified).AddTicks(2502));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 809, DateTimeKind.Unspecified).AddTicks(2503));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 809, DateTimeKind.Unspecified).AddTicks(2505));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 809, DateTimeKind.Unspecified).AddTicks(2506));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 809, DateTimeKind.Unspecified).AddTicks(2507));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 809, DateTimeKind.Unspecified).AddTicks(2508));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 809, DateTimeKind.Unspecified).AddTicks(2509));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 809, DateTimeKind.Unspecified).AddTicks(6860));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 809, DateTimeKind.Unspecified).AddTicks(6882));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 809, DateTimeKind.Unspecified).AddTicks(6884));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 809, DateTimeKind.Unspecified).AddTicks(6885));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 809, DateTimeKind.Unspecified).AddTicks(6887));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 809, DateTimeKind.Unspecified).AddTicks(6888));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 809, DateTimeKind.Unspecified).AddTicks(6889));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 809, DateTimeKind.Unspecified).AddTicks(6890));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 809, DateTimeKind.Unspecified).AddTicks(6892));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 809, DateTimeKind.Unspecified).AddTicks(6893));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 809, DateTimeKind.Unspecified).AddTicks(6894));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 809, DateTimeKind.Unspecified).AddTicks(9427));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 809, DateTimeKind.Unspecified).AddTicks(9442));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 809, DateTimeKind.Unspecified).AddTicks(9443));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 809, DateTimeKind.Unspecified).AddTicks(9445));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 809, DateTimeKind.Unspecified).AddTicks(9446));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 810, DateTimeKind.Unspecified).AddTicks(1086));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 810, DateTimeKind.Unspecified).AddTicks(1098));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 810, DateTimeKind.Unspecified).AddTicks(1099));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 810, DateTimeKind.Unspecified).AddTicks(1101));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 13, 2, 7, 810, DateTimeKind.Unspecified).AddTicks(1102));
        }
    }
}
