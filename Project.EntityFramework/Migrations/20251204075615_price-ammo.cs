using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class priceammo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "BaseItems",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "MinimumQuantity", "Price" },
                values: new object[] { 200L, 0.65m });

            migrationBuilder.UpdateData(
                table: "BaseItems",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "MinimumQuantity", "Price" },
                values: new object[] { 150L, 1.25m });

            migrationBuilder.UpdateData(
                table: "BaseItems",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "MinimumQuantity", "Price" },
                values: new object[] { 200L, 0.70m });

            migrationBuilder.UpdateData(
                table: "BaseItems",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "MinimumQuantity", "Price" },
                values: new object[] { 50L, 3.50m });

            migrationBuilder.UpdateData(
                table: "BaseItems",
                keyColumn: "Id",
                keyValue: 5L,
                columns: new[] { "MinimumQuantity", "Price" },
                values: new object[] { 100L, 1.50m });

            migrationBuilder.UpdateData(
                table: "BaseItems",
                keyColumn: "Id",
                keyValue: 6L,
                columns: new[] { "MinimumQuantity", "Price" },
                values: new object[] { 150L, 0.75m });

            migrationBuilder.UpdateData(
                table: "BaseItems",
                keyColumn: "Id",
                keyValue: 7L,
                columns: new[] { "MinimumQuantity", "Price" },
                values: new object[] { 50L, 2.50m });

            migrationBuilder.UpdateData(
                table: "BaseItems",
                keyColumn: "Id",
                keyValue: 8L,
                columns: new[] { "MinimumQuantity", "Price" },
                values: new object[] { 200L, 0.60m });

            migrationBuilder.UpdateData(
                table: "BaseItems",
                keyColumn: "Id",
                keyValue: 9L,
                columns: new[] { "MinimumQuantity", "Price" },
                values: new object[] { 150L, 0.80m });

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 806, DateTimeKind.Unspecified).AddTicks(6194));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 806, DateTimeKind.Unspecified).AddTicks(6217));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 806, DateTimeKind.Unspecified).AddTicks(6219));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 806, DateTimeKind.Unspecified).AddTicks(6220));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 806, DateTimeKind.Unspecified).AddTicks(6221));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 806, DateTimeKind.Unspecified).AddTicks(9040));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 806, DateTimeKind.Unspecified).AddTicks(9056));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 806, DateTimeKind.Unspecified).AddTicks(9058));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 806, DateTimeKind.Unspecified).AddTicks(9059));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 806, DateTimeKind.Unspecified).AddTicks(9060));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 807, DateTimeKind.Unspecified).AddTicks(2242));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 807, DateTimeKind.Unspecified).AddTicks(2259));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 807, DateTimeKind.Unspecified).AddTicks(2260));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 807, DateTimeKind.Unspecified).AddTicks(2262));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 807, DateTimeKind.Unspecified).AddTicks(2263));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 807, DateTimeKind.Unspecified).AddTicks(2264));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 807, DateTimeKind.Unspecified).AddTicks(2266));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 807, DateTimeKind.Unspecified).AddTicks(2267));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 807, DateTimeKind.Unspecified).AddTicks(2268));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 807, DateTimeKind.Unspecified).AddTicks(2270));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 807, DateTimeKind.Unspecified).AddTicks(2271));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 807, DateTimeKind.Unspecified).AddTicks(2272));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 807, DateTimeKind.Unspecified).AddTicks(2273));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 807, DateTimeKind.Unspecified).AddTicks(2275));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 15L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 807, DateTimeKind.Unspecified).AddTicks(2276));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 16L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 807, DateTimeKind.Unspecified).AddTicks(2277));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 17L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 807, DateTimeKind.Unspecified).AddTicks(2278));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 18L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 807, DateTimeKind.Unspecified).AddTicks(2279));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 807, DateTimeKind.Unspecified).AddTicks(3842));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 807, DateTimeKind.Unspecified).AddTicks(3856));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 807, DateTimeKind.Unspecified).AddTicks(3858));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 807, DateTimeKind.Unspecified).AddTicks(3860));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 807, DateTimeKind.Unspecified).AddTicks(9627));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 807, DateTimeKind.Unspecified).AddTicks(9647));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 807, DateTimeKind.Unspecified).AddTicks(9649));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 807, DateTimeKind.Unspecified).AddTicks(9650));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 807, DateTimeKind.Unspecified).AddTicks(9651));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 808, DateTimeKind.Unspecified).AddTicks(1005));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 808, DateTimeKind.Unspecified).AddTicks(1016));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 808, DateTimeKind.Unspecified).AddTicks(1017));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 808, DateTimeKind.Unspecified).AddTicks(1019));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 808, DateTimeKind.Unspecified).AddTicks(1020));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 808, DateTimeKind.Unspecified).AddTicks(8306));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 808, DateTimeKind.Unspecified).AddTicks(8327));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 808, DateTimeKind.Unspecified).AddTicks(8328));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 808, DateTimeKind.Unspecified).AddTicks(8330));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 808, DateTimeKind.Unspecified).AddTicks(8331));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 808, DateTimeKind.Unspecified).AddTicks(9718));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 808, DateTimeKind.Unspecified).AddTicks(9727));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 808, DateTimeKind.Unspecified).AddTicks(9728));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 808, DateTimeKind.Unspecified).AddTicks(9730));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 808, DateTimeKind.Unspecified).AddTicks(9731));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 808, DateTimeKind.Unspecified).AddTicks(9732));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 808, DateTimeKind.Unspecified).AddTicks(9733));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 808, DateTimeKind.Unspecified).AddTicks(9734));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 808, DateTimeKind.Unspecified).AddTicks(9735));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 808, DateTimeKind.Unspecified).AddTicks(9736));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 808, DateTimeKind.Unspecified).AddTicks(9737));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 809, DateTimeKind.Unspecified).AddTicks(7344));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 809, DateTimeKind.Unspecified).AddTicks(7365));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 809, DateTimeKind.Unspecified).AddTicks(7366));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 809, DateTimeKind.Unspecified).AddTicks(7368));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 809, DateTimeKind.Unspecified).AddTicks(7372));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 809, DateTimeKind.Unspecified).AddTicks(9082));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 809, DateTimeKind.Unspecified).AddTicks(9095));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 809, DateTimeKind.Unspecified).AddTicks(9097));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 809, DateTimeKind.Unspecified).AddTicks(9098));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 809, DateTimeKind.Unspecified).AddTicks(9099));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 810, DateTimeKind.Unspecified).AddTicks(631));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 810, DateTimeKind.Unspecified).AddTicks(642));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 810, DateTimeKind.Unspecified).AddTicks(644));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 810, DateTimeKind.Unspecified).AddTicks(645));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 810, DateTimeKind.Unspecified).AddTicks(647));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 810, DateTimeKind.Unspecified).AddTicks(1962));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 810, DateTimeKind.Unspecified).AddTicks(1971));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 810, DateTimeKind.Unspecified).AddTicks(1973));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 810, DateTimeKind.Unspecified).AddTicks(1974));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 810, DateTimeKind.Unspecified).AddTicks(1975));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 810, DateTimeKind.Unspecified).AddTicks(1976));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 810, DateTimeKind.Unspecified).AddTicks(1978));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 810, DateTimeKind.Unspecified).AddTicks(1979));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 810, DateTimeKind.Unspecified).AddTicks(1980));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 810, DateTimeKind.Unspecified).AddTicks(1981));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 810, DateTimeKind.Unspecified).AddTicks(5945));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 810, DateTimeKind.Unspecified).AddTicks(5965));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 810, DateTimeKind.Unspecified).AddTicks(5967));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 810, DateTimeKind.Unspecified).AddTicks(5969));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 810, DateTimeKind.Unspecified).AddTicks(5970));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 810, DateTimeKind.Unspecified).AddTicks(5972));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 810, DateTimeKind.Unspecified).AddTicks(5973));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 810, DateTimeKind.Unspecified).AddTicks(5974));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 810, DateTimeKind.Unspecified).AddTicks(5975));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 810, DateTimeKind.Unspecified).AddTicks(5977));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 810, DateTimeKind.Unspecified).AddTicks(5978));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 810, DateTimeKind.Unspecified).AddTicks(7916));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 810, DateTimeKind.Unspecified).AddTicks(7932));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 810, DateTimeKind.Unspecified).AddTicks(7934));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 810, DateTimeKind.Unspecified).AddTicks(7936));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 810, DateTimeKind.Unspecified).AddTicks(7937));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 810, DateTimeKind.Unspecified).AddTicks(9276));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 810, DateTimeKind.Unspecified).AddTicks(9285));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 810, DateTimeKind.Unspecified).AddTicks(9287));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 810, DateTimeKind.Unspecified).AddTicks(9289));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 810, DateTimeKind.Unspecified).AddTicks(9290));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 811, DateTimeKind.Unspecified).AddTicks(5210));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 811, DateTimeKind.Unspecified).AddTicks(5230));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 811, DateTimeKind.Unspecified).AddTicks(5232));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 811, DateTimeKind.Unspecified).AddTicks(5233));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 811, DateTimeKind.Unspecified).AddTicks(5235));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 812, DateTimeKind.Unspecified).AddTicks(9951));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 812, DateTimeKind.Unspecified).AddTicks(9974));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 56, 14, 812, DateTimeKind.Unspecified).AddTicks(9976));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "BaseItems",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "MinimumQuantity", "Price" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "BaseItems",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "MinimumQuantity", "Price" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "BaseItems",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "MinimumQuantity", "Price" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "BaseItems",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "MinimumQuantity", "Price" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "BaseItems",
                keyColumn: "Id",
                keyValue: 5L,
                columns: new[] { "MinimumQuantity", "Price" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "BaseItems",
                keyColumn: "Id",
                keyValue: 6L,
                columns: new[] { "MinimumQuantity", "Price" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "BaseItems",
                keyColumn: "Id",
                keyValue: 7L,
                columns: new[] { "MinimumQuantity", "Price" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "BaseItems",
                keyColumn: "Id",
                keyValue: 8L,
                columns: new[] { "MinimumQuantity", "Price" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "BaseItems",
                keyColumn: "Id",
                keyValue: 9L,
                columns: new[] { "MinimumQuantity", "Price" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 986, DateTimeKind.Unspecified).AddTicks(6529));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 986, DateTimeKind.Unspecified).AddTicks(6550));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 986, DateTimeKind.Unspecified).AddTicks(6551));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 986, DateTimeKind.Unspecified).AddTicks(6553));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 986, DateTimeKind.Unspecified).AddTicks(6554));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 986, DateTimeKind.Unspecified).AddTicks(9266));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 986, DateTimeKind.Unspecified).AddTicks(9281));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 986, DateTimeKind.Unspecified).AddTicks(9282));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 986, DateTimeKind.Unspecified).AddTicks(9283));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 986, DateTimeKind.Unspecified).AddTicks(9285));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 987, DateTimeKind.Unspecified).AddTicks(2299));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 987, DateTimeKind.Unspecified).AddTicks(2314));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 987, DateTimeKind.Unspecified).AddTicks(2315));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 987, DateTimeKind.Unspecified).AddTicks(2317));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 987, DateTimeKind.Unspecified).AddTicks(2318));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 987, DateTimeKind.Unspecified).AddTicks(2319));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 987, DateTimeKind.Unspecified).AddTicks(2321));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 987, DateTimeKind.Unspecified).AddTicks(2322));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 987, DateTimeKind.Unspecified).AddTicks(2323));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 987, DateTimeKind.Unspecified).AddTicks(2324));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 987, DateTimeKind.Unspecified).AddTicks(2326));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 987, DateTimeKind.Unspecified).AddTicks(2327));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 987, DateTimeKind.Unspecified).AddTicks(2328));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 987, DateTimeKind.Unspecified).AddTicks(2330));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 15L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 987, DateTimeKind.Unspecified).AddTicks(2331));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 16L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 987, DateTimeKind.Unspecified).AddTicks(2332));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 17L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 987, DateTimeKind.Unspecified).AddTicks(2333));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 18L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 987, DateTimeKind.Unspecified).AddTicks(2334));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 987, DateTimeKind.Unspecified).AddTicks(3836));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 987, DateTimeKind.Unspecified).AddTicks(3848));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 987, DateTimeKind.Unspecified).AddTicks(3850));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 987, DateTimeKind.Unspecified).AddTicks(3870));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 987, DateTimeKind.Unspecified).AddTicks(9292));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 987, DateTimeKind.Unspecified).AddTicks(9311));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 987, DateTimeKind.Unspecified).AddTicks(9312));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 987, DateTimeKind.Unspecified).AddTicks(9314));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 987, DateTimeKind.Unspecified).AddTicks(9315));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 988, DateTimeKind.Unspecified).AddTicks(541));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 988, DateTimeKind.Unspecified).AddTicks(549));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 988, DateTimeKind.Unspecified).AddTicks(551));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 988, DateTimeKind.Unspecified).AddTicks(552));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 988, DateTimeKind.Unspecified).AddTicks(554));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 988, DateTimeKind.Unspecified).AddTicks(7930));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 988, DateTimeKind.Unspecified).AddTicks(7950));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 988, DateTimeKind.Unspecified).AddTicks(7951));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 988, DateTimeKind.Unspecified).AddTicks(7953));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 988, DateTimeKind.Unspecified).AddTicks(7954));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 988, DateTimeKind.Unspecified).AddTicks(9332));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 988, DateTimeKind.Unspecified).AddTicks(9341));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 988, DateTimeKind.Unspecified).AddTicks(9343));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 988, DateTimeKind.Unspecified).AddTicks(9345));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 988, DateTimeKind.Unspecified).AddTicks(9346));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 988, DateTimeKind.Unspecified).AddTicks(9347));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 988, DateTimeKind.Unspecified).AddTicks(9348));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 988, DateTimeKind.Unspecified).AddTicks(9349));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 988, DateTimeKind.Unspecified).AddTicks(9351));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 988, DateTimeKind.Unspecified).AddTicks(9352));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 988, DateTimeKind.Unspecified).AddTicks(9353));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 989, DateTimeKind.Unspecified).AddTicks(7045));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 989, DateTimeKind.Unspecified).AddTicks(7064));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 989, DateTimeKind.Unspecified).AddTicks(7066));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 989, DateTimeKind.Unspecified).AddTicks(7067));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 989, DateTimeKind.Unspecified).AddTicks(7069));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 989, DateTimeKind.Unspecified).AddTicks(8251));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 989, DateTimeKind.Unspecified).AddTicks(8259));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 989, DateTimeKind.Unspecified).AddTicks(8261));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 989, DateTimeKind.Unspecified).AddTicks(8262));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 989, DateTimeKind.Unspecified).AddTicks(8263));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 989, DateTimeKind.Unspecified).AddTicks(9412));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 989, DateTimeKind.Unspecified).AddTicks(9420));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 989, DateTimeKind.Unspecified).AddTicks(9421));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 989, DateTimeKind.Unspecified).AddTicks(9422));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 989, DateTimeKind.Unspecified).AddTicks(9424));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 990, DateTimeKind.Unspecified).AddTicks(571));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 990, DateTimeKind.Unspecified).AddTicks(580));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 990, DateTimeKind.Unspecified).AddTicks(582));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 990, DateTimeKind.Unspecified).AddTicks(583));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 990, DateTimeKind.Unspecified).AddTicks(584));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 990, DateTimeKind.Unspecified).AddTicks(586));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 990, DateTimeKind.Unspecified).AddTicks(587));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 990, DateTimeKind.Unspecified).AddTicks(588));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 990, DateTimeKind.Unspecified).AddTicks(589));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 990, DateTimeKind.Unspecified).AddTicks(590));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 990, DateTimeKind.Unspecified).AddTicks(3774));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 990, DateTimeKind.Unspecified).AddTicks(3793));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 990, DateTimeKind.Unspecified).AddTicks(3795));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 990, DateTimeKind.Unspecified).AddTicks(3796));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 990, DateTimeKind.Unspecified).AddTicks(3798));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 990, DateTimeKind.Unspecified).AddTicks(3799));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 990, DateTimeKind.Unspecified).AddTicks(3801));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 990, DateTimeKind.Unspecified).AddTicks(3802));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 990, DateTimeKind.Unspecified).AddTicks(3803));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 990, DateTimeKind.Unspecified).AddTicks(3804));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 990, DateTimeKind.Unspecified).AddTicks(3805));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 990, DateTimeKind.Unspecified).AddTicks(5605));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 990, DateTimeKind.Unspecified).AddTicks(5621));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 990, DateTimeKind.Unspecified).AddTicks(5622));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 990, DateTimeKind.Unspecified).AddTicks(5623));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 990, DateTimeKind.Unspecified).AddTicks(5624));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 990, DateTimeKind.Unspecified).AddTicks(6851));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 990, DateTimeKind.Unspecified).AddTicks(6860));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 990, DateTimeKind.Unspecified).AddTicks(6861));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 990, DateTimeKind.Unspecified).AddTicks(6862));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 990, DateTimeKind.Unspecified).AddTicks(6864));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 991, DateTimeKind.Unspecified).AddTicks(1998));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 991, DateTimeKind.Unspecified).AddTicks(2016));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 991, DateTimeKind.Unspecified).AddTicks(2017));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 991, DateTimeKind.Unspecified).AddTicks(2019));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 991, DateTimeKind.Unspecified).AddTicks(2020));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 992, DateTimeKind.Unspecified).AddTicks(4992));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 992, DateTimeKind.Unspecified).AddTicks(5013));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 4, 10, 5, 46, 992, DateTimeKind.Unspecified).AddTicks(5014));
        }
    }
}
