using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class RefactorDatabaseTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BaseItems_Countries_CountryId",
                table: "BaseItems");

            migrationBuilder.DropForeignKey(
                name: "FK_BaseItems_Manufacturers_ManufacturerId",
                table: "BaseItems");

            migrationBuilder.DropForeignKey(
                name: "FK_BaseItems_Suppliers_SupplierId",
                table: "BaseItems");

            migrationBuilder.DropIndex(
                name: "IX_BaseItems_CountryId",
                table: "BaseItems");

            migrationBuilder.DropIndex(
                name: "IX_BaseItems_ManufacturerId",
                table: "BaseItems");

            migrationBuilder.DropIndex(
                name: "IX_BaseItems_SupplierId",
                table: "BaseItems");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "BaseItems");

            migrationBuilder.DropColumn(
                name: "Lot",
                table: "BaseItems");

            migrationBuilder.DropColumn(
                name: "ManufacturerId",
                table: "BaseItems");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "BaseItems");

            migrationBuilder.AddColumn<long>(
                name: "CountryId",
                table: "InventoryDetails",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Lot",
                table: "InventoryDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "ManufacturerId",
                table: "InventoryDetails",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SupplierId",
                table: "InventoryDetails",
                type: "bigint",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 720, DateTimeKind.Unspecified).AddTicks(1504));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 720, DateTimeKind.Unspecified).AddTicks(1582));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 720, DateTimeKind.Unspecified).AddTicks(1584));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 720, DateTimeKind.Unspecified).AddTicks(1586));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 720, DateTimeKind.Unspecified).AddTicks(1587));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 720, DateTimeKind.Unspecified).AddTicks(4487));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 720, DateTimeKind.Unspecified).AddTicks(4507));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 720, DateTimeKind.Unspecified).AddTicks(4509));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 720, DateTimeKind.Unspecified).AddTicks(4510));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 720, DateTimeKind.Unspecified).AddTicks(4512));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 720, DateTimeKind.Unspecified).AddTicks(7739));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 720, DateTimeKind.Unspecified).AddTicks(7763));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 720, DateTimeKind.Unspecified).AddTicks(7765));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 720, DateTimeKind.Unspecified).AddTicks(7766));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 720, DateTimeKind.Unspecified).AddTicks(7768));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 720, DateTimeKind.Unspecified).AddTicks(9175));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 720, DateTimeKind.Unspecified).AddTicks(9193));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 720, DateTimeKind.Unspecified).AddTicks(9196));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 720, DateTimeKind.Unspecified).AddTicks(9198));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 721, DateTimeKind.Unspecified).AddTicks(501));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 721, DateTimeKind.Unspecified).AddTicks(511));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 721, DateTimeKind.Unspecified).AddTicks(513));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 721, DateTimeKind.Unspecified).AddTicks(515));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 721, DateTimeKind.Unspecified).AddTicks(516));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 721, DateTimeKind.Unspecified).AddTicks(1853));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 721, DateTimeKind.Unspecified).AddTicks(1865));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 721, DateTimeKind.Unspecified).AddTicks(1867));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 721, DateTimeKind.Unspecified).AddTicks(1868));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 721, DateTimeKind.Unspecified).AddTicks(1870));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 721, DateTimeKind.Unspecified).AddTicks(8742));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 721, DateTimeKind.Unspecified).AddTicks(8765));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 721, DateTimeKind.Unspecified).AddTicks(8767));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 721, DateTimeKind.Unspecified).AddTicks(8769));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 721, DateTimeKind.Unspecified).AddTicks(8770));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 722, DateTimeKind.Unspecified).AddTicks(197));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 722, DateTimeKind.Unspecified).AddTicks(209));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 722, DateTimeKind.Unspecified).AddTicks(211));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 722, DateTimeKind.Unspecified).AddTicks(213));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 722, DateTimeKind.Unspecified).AddTicks(214));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 722, DateTimeKind.Unspecified).AddTicks(1581));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 722, DateTimeKind.Unspecified).AddTicks(1593));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 722, DateTimeKind.Unspecified).AddTicks(1595));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 722, DateTimeKind.Unspecified).AddTicks(1596));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 722, DateTimeKind.Unspecified).AddTicks(1598));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 722, DateTimeKind.Unspecified).AddTicks(3088));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 722, DateTimeKind.Unspecified).AddTicks(3101));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 722, DateTimeKind.Unspecified).AddTicks(3103));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 722, DateTimeKind.Unspecified).AddTicks(3104));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 722, DateTimeKind.Unspecified).AddTicks(3106));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 722, DateTimeKind.Unspecified).AddTicks(4497));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 722, DateTimeKind.Unspecified).AddTicks(4507));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 722, DateTimeKind.Unspecified).AddTicks(4509));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 722, DateTimeKind.Unspecified).AddTicks(4511));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 722, DateTimeKind.Unspecified).AddTicks(4513));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 722, DateTimeKind.Unspecified).AddTicks(5921));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 722, DateTimeKind.Unspecified).AddTicks(5933));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 722, DateTimeKind.Unspecified).AddTicks(5935));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 722, DateTimeKind.Unspecified).AddTicks(5937));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 722, DateTimeKind.Unspecified).AddTicks(5939));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 722, DateTimeKind.Unspecified).AddTicks(7524));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 722, DateTimeKind.Unspecified).AddTicks(7537));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 722, DateTimeKind.Unspecified).AddTicks(7539));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 722, DateTimeKind.Unspecified).AddTicks(7541));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 722, DateTimeKind.Unspecified).AddTicks(7543));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 722, DateTimeKind.Unspecified).AddTicks(8924));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 722, DateTimeKind.Unspecified).AddTicks(8934));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 722, DateTimeKind.Unspecified).AddTicks(8936));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 722, DateTimeKind.Unspecified).AddTicks(8938));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 9, 16, 7, 722, DateTimeKind.Unspecified).AddTicks(8939));

            migrationBuilder.CreateIndex(
                name: "IX_InventoryDetails_CountryId",
                table: "InventoryDetails",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryDetails_ManufacturerId",
                table: "InventoryDetails",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryDetails_SupplierId",
                table: "InventoryDetails",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryDetails_Countries_CountryId",
                table: "InventoryDetails",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryDetails_Manufacturers_ManufacturerId",
                table: "InventoryDetails",
                column: "ManufacturerId",
                principalTable: "Manufacturers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryDetails_Suppliers_SupplierId",
                table: "InventoryDetails",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryDetails_Countries_CountryId",
                table: "InventoryDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryDetails_Manufacturers_ManufacturerId",
                table: "InventoryDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryDetails_Suppliers_SupplierId",
                table: "InventoryDetails");

            migrationBuilder.DropIndex(
                name: "IX_InventoryDetails_CountryId",
                table: "InventoryDetails");

            migrationBuilder.DropIndex(
                name: "IX_InventoryDetails_ManufacturerId",
                table: "InventoryDetails");

            migrationBuilder.DropIndex(
                name: "IX_InventoryDetails_SupplierId",
                table: "InventoryDetails");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "InventoryDetails");

            migrationBuilder.DropColumn(
                name: "Lot",
                table: "InventoryDetails");

            migrationBuilder.DropColumn(
                name: "ManufacturerId",
                table: "InventoryDetails");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "InventoryDetails");

            migrationBuilder.AddColumn<long>(
                name: "CountryId",
                table: "BaseItems",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Lot",
                table: "BaseItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "ManufacturerId",
                table: "BaseItems",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SupplierId",
                table: "BaseItems",
                type: "bigint",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 290, DateTimeKind.Unspecified).AddTicks(7106));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 290, DateTimeKind.Unspecified).AddTicks(7170));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 290, DateTimeKind.Unspecified).AddTicks(7173));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 290, DateTimeKind.Unspecified).AddTicks(7175));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 290, DateTimeKind.Unspecified).AddTicks(7177));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 291, DateTimeKind.Unspecified).AddTicks(187));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 291, DateTimeKind.Unspecified).AddTicks(208));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 291, DateTimeKind.Unspecified).AddTicks(211));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 291, DateTimeKind.Unspecified).AddTicks(212));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 291, DateTimeKind.Unspecified).AddTicks(214));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 291, DateTimeKind.Unspecified).AddTicks(3401));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 291, DateTimeKind.Unspecified).AddTicks(3420));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 291, DateTimeKind.Unspecified).AddTicks(3423));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 291, DateTimeKind.Unspecified).AddTicks(3424));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 291, DateTimeKind.Unspecified).AddTicks(3426));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 291, DateTimeKind.Unspecified).AddTicks(4850));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 291, DateTimeKind.Unspecified).AddTicks(4864));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 291, DateTimeKind.Unspecified).AddTicks(4867));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 291, DateTimeKind.Unspecified).AddTicks(4869));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 291, DateTimeKind.Unspecified).AddTicks(6358));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 291, DateTimeKind.Unspecified).AddTicks(6369));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 291, DateTimeKind.Unspecified).AddTicks(6371));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 291, DateTimeKind.Unspecified).AddTicks(6373));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 291, DateTimeKind.Unspecified).AddTicks(6375));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 291, DateTimeKind.Unspecified).AddTicks(7743));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 291, DateTimeKind.Unspecified).AddTicks(7754));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 291, DateTimeKind.Unspecified).AddTicks(7756));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 291, DateTimeKind.Unspecified).AddTicks(7758));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 291, DateTimeKind.Unspecified).AddTicks(7759));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 292, DateTimeKind.Unspecified).AddTicks(2804));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 292, DateTimeKind.Unspecified).AddTicks(2826));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 292, DateTimeKind.Unspecified).AddTicks(2828));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 292, DateTimeKind.Unspecified).AddTicks(2830));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 292, DateTimeKind.Unspecified).AddTicks(2832));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 292, DateTimeKind.Unspecified).AddTicks(4195));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 292, DateTimeKind.Unspecified).AddTicks(4205));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 292, DateTimeKind.Unspecified).AddTicks(4206));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 292, DateTimeKind.Unspecified).AddTicks(4208));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 292, DateTimeKind.Unspecified).AddTicks(4210));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 292, DateTimeKind.Unspecified).AddTicks(5544));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 292, DateTimeKind.Unspecified).AddTicks(5554));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 292, DateTimeKind.Unspecified).AddTicks(5556));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 292, DateTimeKind.Unspecified).AddTicks(5558));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 292, DateTimeKind.Unspecified).AddTicks(5559));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 292, DateTimeKind.Unspecified).AddTicks(6940));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 292, DateTimeKind.Unspecified).AddTicks(6950));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 292, DateTimeKind.Unspecified).AddTicks(6977));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 292, DateTimeKind.Unspecified).AddTicks(6979));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 292, DateTimeKind.Unspecified).AddTicks(6981));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 292, DateTimeKind.Unspecified).AddTicks(8248));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 292, DateTimeKind.Unspecified).AddTicks(8257));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 292, DateTimeKind.Unspecified).AddTicks(8259));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 292, DateTimeKind.Unspecified).AddTicks(8260));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 292, DateTimeKind.Unspecified).AddTicks(8262));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 292, DateTimeKind.Unspecified).AddTicks(9759));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 292, DateTimeKind.Unspecified).AddTicks(9770));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 292, DateTimeKind.Unspecified).AddTicks(9772));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 292, DateTimeKind.Unspecified).AddTicks(9774));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 292, DateTimeKind.Unspecified).AddTicks(9776));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 293, DateTimeKind.Unspecified).AddTicks(1195));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 293, DateTimeKind.Unspecified).AddTicks(1207));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 293, DateTimeKind.Unspecified).AddTicks(1209));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 293, DateTimeKind.Unspecified).AddTicks(1211));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 293, DateTimeKind.Unspecified).AddTicks(1213));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 293, DateTimeKind.Unspecified).AddTicks(2551));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 293, DateTimeKind.Unspecified).AddTicks(2561));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 293, DateTimeKind.Unspecified).AddTicks(2562));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 293, DateTimeKind.Unspecified).AddTicks(2564));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 26, 25, 293, DateTimeKind.Unspecified).AddTicks(2566));

            migrationBuilder.CreateIndex(
                name: "IX_BaseItems_CountryId",
                table: "BaseItems",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseItems_ManufacturerId",
                table: "BaseItems",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseItems_SupplierId",
                table: "BaseItems",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_BaseItems_Countries_CountryId",
                table: "BaseItems",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BaseItems_Manufacturers_ManufacturerId",
                table: "BaseItems",
                column: "ManufacturerId",
                principalTable: "Manufacturers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BaseItems_Suppliers_SupplierId",
                table: "BaseItems",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
