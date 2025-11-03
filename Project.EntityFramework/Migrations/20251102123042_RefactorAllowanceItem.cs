using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class RefactorAllowanceItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ItemType",
                table: "AllowanceItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 790, DateTimeKind.Unspecified).AddTicks(5157));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 790, DateTimeKind.Unspecified).AddTicks(5227));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 790, DateTimeKind.Unspecified).AddTicks(5229));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 790, DateTimeKind.Unspecified).AddTicks(5231));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 790, DateTimeKind.Unspecified).AddTicks(5233));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 790, DateTimeKind.Unspecified).AddTicks(8209));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 790, DateTimeKind.Unspecified).AddTicks(8232));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 790, DateTimeKind.Unspecified).AddTicks(8234));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 790, DateTimeKind.Unspecified).AddTicks(8236));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 790, DateTimeKind.Unspecified).AddTicks(8238));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 791, DateTimeKind.Unspecified).AddTicks(3507));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 791, DateTimeKind.Unspecified).AddTicks(3566));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 791, DateTimeKind.Unspecified).AddTicks(3568));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 791, DateTimeKind.Unspecified).AddTicks(3570));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 791, DateTimeKind.Unspecified).AddTicks(3572));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 791, DateTimeKind.Unspecified).AddTicks(6756));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 791, DateTimeKind.Unspecified).AddTicks(6775));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 791, DateTimeKind.Unspecified).AddTicks(6778));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 791, DateTimeKind.Unspecified).AddTicks(6780));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 791, DateTimeKind.Unspecified).AddTicks(8340));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 791, DateTimeKind.Unspecified).AddTicks(8350));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 791, DateTimeKind.Unspecified).AddTicks(8351));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 791, DateTimeKind.Unspecified).AddTicks(8353));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 791, DateTimeKind.Unspecified).AddTicks(8355));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 791, DateTimeKind.Unspecified).AddTicks(9873));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 791, DateTimeKind.Unspecified).AddTicks(9883));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 791, DateTimeKind.Unspecified).AddTicks(9885));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 791, DateTimeKind.Unspecified).AddTicks(9887));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 791, DateTimeKind.Unspecified).AddTicks(9889));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 792, DateTimeKind.Unspecified).AddTicks(7420));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 792, DateTimeKind.Unspecified).AddTicks(7445));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 792, DateTimeKind.Unspecified).AddTicks(7447));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 792, DateTimeKind.Unspecified).AddTicks(7449));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 792, DateTimeKind.Unspecified).AddTicks(7451));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 792, DateTimeKind.Unspecified).AddTicks(8974));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 792, DateTimeKind.Unspecified).AddTicks(8984));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 792, DateTimeKind.Unspecified).AddTicks(8986));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 792, DateTimeKind.Unspecified).AddTicks(8987));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 792, DateTimeKind.Unspecified).AddTicks(8989));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 793, DateTimeKind.Unspecified).AddTicks(437));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 793, DateTimeKind.Unspecified).AddTicks(447));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 793, DateTimeKind.Unspecified).AddTicks(449));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 793, DateTimeKind.Unspecified).AddTicks(451));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 793, DateTimeKind.Unspecified).AddTicks(452));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 793, DateTimeKind.Unspecified).AddTicks(1853));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 793, DateTimeKind.Unspecified).AddTicks(1863));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 793, DateTimeKind.Unspecified).AddTicks(1865));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 793, DateTimeKind.Unspecified).AddTicks(1866));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 793, DateTimeKind.Unspecified).AddTicks(1868));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 793, DateTimeKind.Unspecified).AddTicks(3309));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 793, DateTimeKind.Unspecified).AddTicks(3323));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 793, DateTimeKind.Unspecified).AddTicks(3325));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 793, DateTimeKind.Unspecified).AddTicks(3327));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 793, DateTimeKind.Unspecified).AddTicks(3328));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 793, DateTimeKind.Unspecified).AddTicks(4800));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 793, DateTimeKind.Unspecified).AddTicks(4810));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 793, DateTimeKind.Unspecified).AddTicks(4812));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 793, DateTimeKind.Unspecified).AddTicks(4814));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 793, DateTimeKind.Unspecified).AddTicks(4816));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 793, DateTimeKind.Unspecified).AddTicks(6259));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 793, DateTimeKind.Unspecified).AddTicks(6270));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 793, DateTimeKind.Unspecified).AddTicks(6272));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 793, DateTimeKind.Unspecified).AddTicks(6274));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 793, DateTimeKind.Unspecified).AddTicks(6276));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 793, DateTimeKind.Unspecified).AddTicks(7668));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 793, DateTimeKind.Unspecified).AddTicks(7678));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 793, DateTimeKind.Unspecified).AddTicks(7680));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 793, DateTimeKind.Unspecified).AddTicks(7682));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 15, 30, 41, 793, DateTimeKind.Unspecified).AddTicks(7683));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemType",
                table: "AllowanceItems");

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
        }
    }
}
