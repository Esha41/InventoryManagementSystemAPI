using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class ImplementSoftDaelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AllowanceItems_ItemId",
                table: "AllowanceItems");

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "BaseItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionDate",
                table: "BaseItems",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "AllowanceItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionDate",
                table: "AllowanceItems",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AllowanceItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 607, DateTimeKind.Unspecified).AddTicks(4350));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 607, DateTimeKind.Unspecified).AddTicks(4426));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 607, DateTimeKind.Unspecified).AddTicks(4428));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 607, DateTimeKind.Unspecified).AddTicks(4430));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 607, DateTimeKind.Unspecified).AddTicks(4432));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 607, DateTimeKind.Unspecified).AddTicks(7396));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 607, DateTimeKind.Unspecified).AddTicks(7420));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 607, DateTimeKind.Unspecified).AddTicks(7422));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 607, DateTimeKind.Unspecified).AddTicks(7424));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 607, DateTimeKind.Unspecified).AddTicks(7426));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 608, DateTimeKind.Unspecified).AddTicks(672));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 608, DateTimeKind.Unspecified).AddTicks(692));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 608, DateTimeKind.Unspecified).AddTicks(694));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 608, DateTimeKind.Unspecified).AddTicks(696));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 608, DateTimeKind.Unspecified).AddTicks(697));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 608, DateTimeKind.Unspecified).AddTicks(2253));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 608, DateTimeKind.Unspecified).AddTicks(2268));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 608, DateTimeKind.Unspecified).AddTicks(2270));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 608, DateTimeKind.Unspecified).AddTicks(2273));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 608, DateTimeKind.Unspecified).AddTicks(3632));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 608, DateTimeKind.Unspecified).AddTicks(3644));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 608, DateTimeKind.Unspecified).AddTicks(3646));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 608, DateTimeKind.Unspecified).AddTicks(3648));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 608, DateTimeKind.Unspecified).AddTicks(3650));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 608, DateTimeKind.Unspecified).AddTicks(5036));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 608, DateTimeKind.Unspecified).AddTicks(5047));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 608, DateTimeKind.Unspecified).AddTicks(5049));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 608, DateTimeKind.Unspecified).AddTicks(5050));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 608, DateTimeKind.Unspecified).AddTicks(5052));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 609, DateTimeKind.Unspecified).AddTicks(2044));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 609, DateTimeKind.Unspecified).AddTicks(2072));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 609, DateTimeKind.Unspecified).AddTicks(2074));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 609, DateTimeKind.Unspecified).AddTicks(2076));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 609, DateTimeKind.Unspecified).AddTicks(2078));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 609, DateTimeKind.Unspecified).AddTicks(3546));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 609, DateTimeKind.Unspecified).AddTicks(3555));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 609, DateTimeKind.Unspecified).AddTicks(3557));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 609, DateTimeKind.Unspecified).AddTicks(3559));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 609, DateTimeKind.Unspecified).AddTicks(3560));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 609, DateTimeKind.Unspecified).AddTicks(4925));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 609, DateTimeKind.Unspecified).AddTicks(4936));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 609, DateTimeKind.Unspecified).AddTicks(4938));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 609, DateTimeKind.Unspecified).AddTicks(4940));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 609, DateTimeKind.Unspecified).AddTicks(4965));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 609, DateTimeKind.Unspecified).AddTicks(6250));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 609, DateTimeKind.Unspecified).AddTicks(6259));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 609, DateTimeKind.Unspecified).AddTicks(6261));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 609, DateTimeKind.Unspecified).AddTicks(6263));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 609, DateTimeKind.Unspecified).AddTicks(6265));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 609, DateTimeKind.Unspecified).AddTicks(7572));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 609, DateTimeKind.Unspecified).AddTicks(7582));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 609, DateTimeKind.Unspecified).AddTicks(7584));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 609, DateTimeKind.Unspecified).AddTicks(7586));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 609, DateTimeKind.Unspecified).AddTicks(7588));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 609, DateTimeKind.Unspecified).AddTicks(8894));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 609, DateTimeKind.Unspecified).AddTicks(8905));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 609, DateTimeKind.Unspecified).AddTicks(8906));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 609, DateTimeKind.Unspecified).AddTicks(8908));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 609, DateTimeKind.Unspecified).AddTicks(8910));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 610, DateTimeKind.Unspecified).AddTicks(355));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 610, DateTimeKind.Unspecified).AddTicks(367));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 610, DateTimeKind.Unspecified).AddTicks(369));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 610, DateTimeKind.Unspecified).AddTicks(371));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 610, DateTimeKind.Unspecified).AddTicks(372));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 610, DateTimeKind.Unspecified).AddTicks(1712));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 610, DateTimeKind.Unspecified).AddTicks(1722));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 610, DateTimeKind.Unspecified).AddTicks(1723));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 610, DateTimeKind.Unspecified).AddTicks(1725));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 14, 32, 37, 610, DateTimeKind.Unspecified).AddTicks(1727));

            migrationBuilder.CreateIndex(
                name: "IX_AllowanceItems_ItemId_DepartmentId_Year",
                table: "AllowanceItems",
                columns: new[] { "ItemId", "DepartmentId", "Year" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AllowanceItems_ItemId_DepartmentId_Year",
                table: "AllowanceItems");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "BaseItems");

            migrationBuilder.DropColumn(
                name: "DeletionDate",
                table: "BaseItems");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "AllowanceItems");

            migrationBuilder.DropColumn(
                name: "DeletionDate",
                table: "AllowanceItems");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AllowanceItems");

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 754, DateTimeKind.Unspecified).AddTicks(2855));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 754, DateTimeKind.Unspecified).AddTicks(2928));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 754, DateTimeKind.Unspecified).AddTicks(2930));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 754, DateTimeKind.Unspecified).AddTicks(2932));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 754, DateTimeKind.Unspecified).AddTicks(2933));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 754, DateTimeKind.Unspecified).AddTicks(5940));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 754, DateTimeKind.Unspecified).AddTicks(5959));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 754, DateTimeKind.Unspecified).AddTicks(5961));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 754, DateTimeKind.Unspecified).AddTicks(5962));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 754, DateTimeKind.Unspecified).AddTicks(5964));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 754, DateTimeKind.Unspecified).AddTicks(9260));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 754, DateTimeKind.Unspecified).AddTicks(9277));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 754, DateTimeKind.Unspecified).AddTicks(9280));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 754, DateTimeKind.Unspecified).AddTicks(9281));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 754, DateTimeKind.Unspecified).AddTicks(9283));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 755, DateTimeKind.Unspecified).AddTicks(735));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 755, DateTimeKind.Unspecified).AddTicks(750));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 755, DateTimeKind.Unspecified).AddTicks(753));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 755, DateTimeKind.Unspecified).AddTicks(755));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 755, DateTimeKind.Unspecified).AddTicks(2141));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 755, DateTimeKind.Unspecified).AddTicks(2149));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 755, DateTimeKind.Unspecified).AddTicks(2150));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 755, DateTimeKind.Unspecified).AddTicks(2152));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 755, DateTimeKind.Unspecified).AddTicks(2181));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 755, DateTimeKind.Unspecified).AddTicks(3635));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 755, DateTimeKind.Unspecified).AddTicks(3644));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 755, DateTimeKind.Unspecified).AddTicks(3646));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 755, DateTimeKind.Unspecified).AddTicks(3647));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 755, DateTimeKind.Unspecified).AddTicks(3649));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 756, DateTimeKind.Unspecified).AddTicks(866));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 756, DateTimeKind.Unspecified).AddTicks(891));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 756, DateTimeKind.Unspecified).AddTicks(892));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 756, DateTimeKind.Unspecified).AddTicks(894));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 756, DateTimeKind.Unspecified).AddTicks(896));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 756, DateTimeKind.Unspecified).AddTicks(2398));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 756, DateTimeKind.Unspecified).AddTicks(2409));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 756, DateTimeKind.Unspecified).AddTicks(2411));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 756, DateTimeKind.Unspecified).AddTicks(2413));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 756, DateTimeKind.Unspecified).AddTicks(2415));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 756, DateTimeKind.Unspecified).AddTicks(3898));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 756, DateTimeKind.Unspecified).AddTicks(3907));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 756, DateTimeKind.Unspecified).AddTicks(3909));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 756, DateTimeKind.Unspecified).AddTicks(3910));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 756, DateTimeKind.Unspecified).AddTicks(3912));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 756, DateTimeKind.Unspecified).AddTicks(5277));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 756, DateTimeKind.Unspecified).AddTicks(5286));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 756, DateTimeKind.Unspecified).AddTicks(5287));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 756, DateTimeKind.Unspecified).AddTicks(5289));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 756, DateTimeKind.Unspecified).AddTicks(5291));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 756, DateTimeKind.Unspecified).AddTicks(6687));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 756, DateTimeKind.Unspecified).AddTicks(6697));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 756, DateTimeKind.Unspecified).AddTicks(6699));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 756, DateTimeKind.Unspecified).AddTicks(6701));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 756, DateTimeKind.Unspecified).AddTicks(6702));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 756, DateTimeKind.Unspecified).AddTicks(8053));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 756, DateTimeKind.Unspecified).AddTicks(8062));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 756, DateTimeKind.Unspecified).AddTicks(8065));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 756, DateTimeKind.Unspecified).AddTicks(8066));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 756, DateTimeKind.Unspecified).AddTicks(8068));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 756, DateTimeKind.Unspecified).AddTicks(9391));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 756, DateTimeKind.Unspecified).AddTicks(9399));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 756, DateTimeKind.Unspecified).AddTicks(9400));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 756, DateTimeKind.Unspecified).AddTicks(9402));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 756, DateTimeKind.Unspecified).AddTicks(9404));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 757, DateTimeKind.Unspecified).AddTicks(693));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 757, DateTimeKind.Unspecified).AddTicks(702));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 757, DateTimeKind.Unspecified).AddTicks(704));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 757, DateTimeKind.Unspecified).AddTicks(705));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 11, 20, 38, 757, DateTimeKind.Unspecified).AddTicks(707));

            migrationBuilder.CreateIndex(
                name: "IX_AllowanceItems_ItemId",
                table: "AllowanceItems",
                column: "ItemId");
        }
    }
}
