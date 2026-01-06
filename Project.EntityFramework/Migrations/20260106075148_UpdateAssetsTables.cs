using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAssetsTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssetAssignments_Employees_CustodianId",
                table: "AssetAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_AssetHistory_Employees_NewCustodianId",
                table: "AssetHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_AssetHistory_Employees_PreviousCustodianId",
                table: "AssetHistory");

            migrationBuilder.AddColumn<string>(
                name: "CustodianId",
                table: "AssetSupplyDetails",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "CustodianId",
                table: "AssetSupplies",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PreviousCustodianId",
                table: "AssetHistory",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NewCustodianId",
                table: "AssetHistory",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CustodianId",
                table: "AssetAssignments",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 944, DateTimeKind.Unspecified).AddTicks(2361));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 944, DateTimeKind.Unspecified).AddTicks(2389));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 944, DateTimeKind.Unspecified).AddTicks(2391));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 944, DateTimeKind.Unspecified).AddTicks(2393));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 944, DateTimeKind.Unspecified).AddTicks(2394));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 944, DateTimeKind.Unspecified).AddTicks(7153));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 944, DateTimeKind.Unspecified).AddTicks(7175));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 944, DateTimeKind.Unspecified).AddTicks(7177));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 944, DateTimeKind.Unspecified).AddTicks(7178));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 944, DateTimeKind.Unspecified).AddTicks(7180));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 945, DateTimeKind.Unspecified).AddTicks(910));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 945, DateTimeKind.Unspecified).AddTicks(931));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 945, DateTimeKind.Unspecified).AddTicks(932));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 945, DateTimeKind.Unspecified).AddTicks(934));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 945, DateTimeKind.Unspecified).AddTicks(935));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 945, DateTimeKind.Unspecified).AddTicks(936));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 945, DateTimeKind.Unspecified).AddTicks(938));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 945, DateTimeKind.Unspecified).AddTicks(939));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 945, DateTimeKind.Unspecified).AddTicks(940));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 945, DateTimeKind.Unspecified).AddTicks(942));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 945, DateTimeKind.Unspecified).AddTicks(943));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 945, DateTimeKind.Unspecified).AddTicks(944));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 945, DateTimeKind.Unspecified).AddTicks(945));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 945, DateTimeKind.Unspecified).AddTicks(947));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 15L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 945, DateTimeKind.Unspecified).AddTicks(948));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 16L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 945, DateTimeKind.Unspecified).AddTicks(949));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 17L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 945, DateTimeKind.Unspecified).AddTicks(950));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 18L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 945, DateTimeKind.Unspecified).AddTicks(952));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 945, DateTimeKind.Unspecified).AddTicks(2911));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 945, DateTimeKind.Unspecified).AddTicks(2929));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 945, DateTimeKind.Unspecified).AddTicks(2932));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 945, DateTimeKind.Unspecified).AddTicks(2934));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 946, DateTimeKind.Unspecified).AddTicks(3584));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 946, DateTimeKind.Unspecified).AddTicks(3607));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 946, DateTimeKind.Unspecified).AddTicks(3608));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 946, DateTimeKind.Unspecified).AddTicks(3609));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 946, DateTimeKind.Unspecified).AddTicks(3611));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 947, DateTimeKind.Unspecified).AddTicks(6963));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 947, DateTimeKind.Unspecified).AddTicks(6986));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 947, DateTimeKind.Unspecified).AddTicks(6988));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 947, DateTimeKind.Unspecified).AddTicks(6990));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 947, DateTimeKind.Unspecified).AddTicks(6991));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 947, DateTimeKind.Unspecified).AddTicks(8597));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 947, DateTimeKind.Unspecified).AddTicks(8610));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 947, DateTimeKind.Unspecified).AddTicks(8612));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 947, DateTimeKind.Unspecified).AddTicks(8613));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 947, DateTimeKind.Unspecified).AddTicks(8614));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 947, DateTimeKind.Unspecified).AddTicks(8616));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 947, DateTimeKind.Unspecified).AddTicks(8617));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 947, DateTimeKind.Unspecified).AddTicks(8618));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 947, DateTimeKind.Unspecified).AddTicks(8619));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 947, DateTimeKind.Unspecified).AddTicks(8620));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 947, DateTimeKind.Unspecified).AddTicks(8621));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 948, DateTimeKind.Unspecified).AddTicks(7528));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 948, DateTimeKind.Unspecified).AddTicks(7552));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 948, DateTimeKind.Unspecified).AddTicks(7554));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 948, DateTimeKind.Unspecified).AddTicks(7555));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 948, DateTimeKind.Unspecified).AddTicks(7557));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 948, DateTimeKind.Unspecified).AddTicks(9155));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 948, DateTimeKind.Unspecified).AddTicks(9169));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 948, DateTimeKind.Unspecified).AddTicks(9170));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 948, DateTimeKind.Unspecified).AddTicks(9172));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 948, DateTimeKind.Unspecified).AddTicks(9173));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(758));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(771));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(773));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(774));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(776));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(2368));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(2379));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(2380));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(2382));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(2383));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(2384));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(2385));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(2387));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(2388));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(2389));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(6494));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(6518));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(6520));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(6521));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(6580));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(6582));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(6583));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(6584));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(6585));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(6586));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(6588));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(6523));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(8842));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(8859));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(8861));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(8863));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 949, DateTimeKind.Unspecified).AddTicks(8864));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 950, DateTimeKind.Unspecified).AddTicks(380));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 950, DateTimeKind.Unspecified).AddTicks(393));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 950, DateTimeKind.Unspecified).AddTicks(394));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 950, DateTimeKind.Unspecified).AddTicks(396));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 950, DateTimeKind.Unspecified).AddTicks(397));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 950, DateTimeKind.Unspecified).AddTicks(7050));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 950, DateTimeKind.Unspecified).AddTicks(7072));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 950, DateTimeKind.Unspecified).AddTicks(7074));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 952, DateTimeKind.Unspecified).AddTicks(3635));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 952, DateTimeKind.Unspecified).AddTicks(3658));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 6, 10, 51, 47, 952, DateTimeKind.Unspecified).AddTicks(3659));

            migrationBuilder.CreateIndex(
                name: "IX_AssetSupplyDetails_CustodianId",
                table: "AssetSupplyDetails",
                column: "CustodianId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssetAssignments_AspNetUsers_CustodianId",
                table: "AssetAssignments",
                column: "CustodianId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetHistory_AspNetUsers_NewCustodianId",
                table: "AssetHistory",
                column: "NewCustodianId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetHistory_AspNetUsers_PreviousCustodianId",
                table: "AssetHistory",
                column: "PreviousCustodianId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetSupplyDetails_AspNetUsers_CustodianId",
                table: "AssetSupplyDetails",
                column: "CustodianId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssetAssignments_AspNetUsers_CustodianId",
                table: "AssetAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_AssetHistory_AspNetUsers_NewCustodianId",
                table: "AssetHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_AssetHistory_AspNetUsers_PreviousCustodianId",
                table: "AssetHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_AssetSupplyDetails_AspNetUsers_CustodianId",
                table: "AssetSupplyDetails");

            migrationBuilder.DropIndex(
                name: "IX_AssetSupplyDetails_CustodianId",
                table: "AssetSupplyDetails");

            migrationBuilder.DropColumn(
                name: "CustodianId",
                table: "AssetSupplyDetails");

            migrationBuilder.AlterColumn<string>(
                name: "CustodianId",
                table: "AssetSupplies",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450);

            migrationBuilder.AlterColumn<long>(
                name: "PreviousCustodianId",
                table: "AssetHistory",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "NewCustodianId",
                table: "AssetHistory",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CustodianId",
                table: "AssetAssignments",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 588, DateTimeKind.Unspecified).AddTicks(5835));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 588, DateTimeKind.Unspecified).AddTicks(5885));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 588, DateTimeKind.Unspecified).AddTicks(5887));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 588, DateTimeKind.Unspecified).AddTicks(5888));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 588, DateTimeKind.Unspecified).AddTicks(5890));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 589, DateTimeKind.Unspecified).AddTicks(1329));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 589, DateTimeKind.Unspecified).AddTicks(1373));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 589, DateTimeKind.Unspecified).AddTicks(1375));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 589, DateTimeKind.Unspecified).AddTicks(1378));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 589, DateTimeKind.Unspecified).AddTicks(1379));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 589, DateTimeKind.Unspecified).AddTicks(5637));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 589, DateTimeKind.Unspecified).AddTicks(5662));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 589, DateTimeKind.Unspecified).AddTicks(5664));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 589, DateTimeKind.Unspecified).AddTicks(5666));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 589, DateTimeKind.Unspecified).AddTicks(5668));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 589, DateTimeKind.Unspecified).AddTicks(5669));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 589, DateTimeKind.Unspecified).AddTicks(5671));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 589, DateTimeKind.Unspecified).AddTicks(5723));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 589, DateTimeKind.Unspecified).AddTicks(5725));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 589, DateTimeKind.Unspecified).AddTicks(5727));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 589, DateTimeKind.Unspecified).AddTicks(5748));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 589, DateTimeKind.Unspecified).AddTicks(5803));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 589, DateTimeKind.Unspecified).AddTicks(5807));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 589, DateTimeKind.Unspecified).AddTicks(5808));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 15L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 589, DateTimeKind.Unspecified).AddTicks(5810));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 16L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 589, DateTimeKind.Unspecified).AddTicks(5811));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 17L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 589, DateTimeKind.Unspecified).AddTicks(5813));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 18L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 589, DateTimeKind.Unspecified).AddTicks(5815));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 589, DateTimeKind.Unspecified).AddTicks(7798));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 589, DateTimeKind.Unspecified).AddTicks(7816));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 589, DateTimeKind.Unspecified).AddTicks(7825));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 589, DateTimeKind.Unspecified).AddTicks(7827));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 590, DateTimeKind.Unspecified).AddTicks(8869));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 590, DateTimeKind.Unspecified).AddTicks(8896));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 590, DateTimeKind.Unspecified).AddTicks(8898));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 590, DateTimeKind.Unspecified).AddTicks(8900));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 590, DateTimeKind.Unspecified).AddTicks(8902));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 592, DateTimeKind.Unspecified).AddTicks(2865));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 592, DateTimeKind.Unspecified).AddTicks(2892));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 592, DateTimeKind.Unspecified).AddTicks(2895));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 592, DateTimeKind.Unspecified).AddTicks(2896));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 592, DateTimeKind.Unspecified).AddTicks(2898));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 592, DateTimeKind.Unspecified).AddTicks(4615));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 592, DateTimeKind.Unspecified).AddTicks(4627));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 592, DateTimeKind.Unspecified).AddTicks(4629));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 592, DateTimeKind.Unspecified).AddTicks(4631));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 592, DateTimeKind.Unspecified).AddTicks(4632));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 592, DateTimeKind.Unspecified).AddTicks(4634));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 592, DateTimeKind.Unspecified).AddTicks(4635));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 592, DateTimeKind.Unspecified).AddTicks(4637));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 592, DateTimeKind.Unspecified).AddTicks(4638));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 592, DateTimeKind.Unspecified).AddTicks(4640));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 592, DateTimeKind.Unspecified).AddTicks(4641));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 593, DateTimeKind.Unspecified).AddTicks(4457));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 593, DateTimeKind.Unspecified).AddTicks(4488));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 593, DateTimeKind.Unspecified).AddTicks(4490));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 593, DateTimeKind.Unspecified).AddTicks(4492));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 593, DateTimeKind.Unspecified).AddTicks(4494));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 593, DateTimeKind.Unspecified).AddTicks(6222));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 593, DateTimeKind.Unspecified).AddTicks(6236));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 593, DateTimeKind.Unspecified).AddTicks(6239));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 593, DateTimeKind.Unspecified).AddTicks(6241));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 593, DateTimeKind.Unspecified).AddTicks(6242));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 593, DateTimeKind.Unspecified).AddTicks(8019));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 593, DateTimeKind.Unspecified).AddTicks(8035));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 593, DateTimeKind.Unspecified).AddTicks(8037));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 593, DateTimeKind.Unspecified).AddTicks(8039));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 593, DateTimeKind.Unspecified).AddTicks(8041));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 593, DateTimeKind.Unspecified).AddTicks(9615));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 593, DateTimeKind.Unspecified).AddTicks(9627));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 593, DateTimeKind.Unspecified).AddTicks(9629));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 593, DateTimeKind.Unspecified).AddTicks(9631));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 593, DateTimeKind.Unspecified).AddTicks(9632));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 593, DateTimeKind.Unspecified).AddTicks(9634));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 593, DateTimeKind.Unspecified).AddTicks(9635));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 593, DateTimeKind.Unspecified).AddTicks(9637));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 593, DateTimeKind.Unspecified).AddTicks(9638));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 593, DateTimeKind.Unspecified).AddTicks(9640));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 594, DateTimeKind.Unspecified).AddTicks(3960));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 594, DateTimeKind.Unspecified).AddTicks(3985));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 594, DateTimeKind.Unspecified).AddTicks(3986));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 594, DateTimeKind.Unspecified).AddTicks(3988));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 594, DateTimeKind.Unspecified).AddTicks(3991));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 594, DateTimeKind.Unspecified).AddTicks(3993));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 594, DateTimeKind.Unspecified).AddTicks(3994));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 594, DateTimeKind.Unspecified).AddTicks(3996));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 594, DateTimeKind.Unspecified).AddTicks(3997));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 594, DateTimeKind.Unspecified).AddTicks(3999));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 594, DateTimeKind.Unspecified).AddTicks(4000));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 594, DateTimeKind.Unspecified).AddTicks(3990));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 594, DateTimeKind.Unspecified).AddTicks(6401));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 594, DateTimeKind.Unspecified).AddTicks(6420));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 594, DateTimeKind.Unspecified).AddTicks(6423));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 594, DateTimeKind.Unspecified).AddTicks(6425));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 594, DateTimeKind.Unspecified).AddTicks(6426));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 594, DateTimeKind.Unspecified).AddTicks(8208));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 594, DateTimeKind.Unspecified).AddTicks(8225));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 594, DateTimeKind.Unspecified).AddTicks(8227));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 594, DateTimeKind.Unspecified).AddTicks(8228));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 594, DateTimeKind.Unspecified).AddTicks(8230));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 595, DateTimeKind.Unspecified).AddTicks(5221));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 595, DateTimeKind.Unspecified).AddTicks(5246));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 595, DateTimeKind.Unspecified).AddTicks(5248));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 597, DateTimeKind.Unspecified).AddTicks(3930));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 597, DateTimeKind.Unspecified).AddTicks(3960));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2026, 1, 5, 16, 49, 31, 597, DateTimeKind.Unspecified).AddTicks(3962));

            migrationBuilder.AddForeignKey(
                name: "FK_AssetAssignments_Employees_CustodianId",
                table: "AssetAssignments",
                column: "CustodianId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetHistory_Employees_NewCustodianId",
                table: "AssetHistory",
                column: "NewCustodianId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetHistory_Employees_PreviousCustodianId",
                table: "AssetHistory",
                column: "PreviousCustodianId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
