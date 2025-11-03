using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class SoftDeleteProblem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Units_NameAr",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Units_NameEn",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Suppliers_NameAr",
                table: "Suppliers");

            migrationBuilder.DropIndex(
                name: "IX_Suppliers_NameEn",
                table: "Suppliers");

            migrationBuilder.DropIndex(
                name: "IX_Propellants_NameAr",
                table: "Propellants");

            migrationBuilder.DropIndex(
                name: "IX_Propellants_NameEn",
                table: "Propellants");

            migrationBuilder.DropIndex(
                name: "IX_ProjectailMaterials_NameAr",
                table: "ProjectailMaterials");

            migrationBuilder.DropIndex(
                name: "IX_ProjectailMaterials_NameEn",
                table: "ProjectailMaterials");

            migrationBuilder.DropIndex(
                name: "IX_PrimaryPurposes_NameAr",
                table: "PrimaryPurposes");

            migrationBuilder.DropIndex(
                name: "IX_PrimaryPurposes_NameEn",
                table: "PrimaryPurposes");

            migrationBuilder.DropIndex(
                name: "IX_Nsn_NameAr",
                table: "Nsn");

            migrationBuilder.DropIndex(
                name: "IX_Nsn_NameEn",
                table: "Nsn");

            migrationBuilder.DropIndex(
                name: "IX_NatureOptions_NameAr",
                table: "NatureOptions");

            migrationBuilder.DropIndex(
                name: "IX_NatureOptions_NameEn",
                table: "NatureOptions");

            migrationBuilder.DropIndex(
                name: "IX_Manufacturers_NameAr",
                table: "Manufacturers");

            migrationBuilder.DropIndex(
                name: "IX_Manufacturers_NameEn",
                table: "Manufacturers");

            migrationBuilder.DropIndex(
                name: "IX_Hcc_NameAr",
                table: "Hcc");

            migrationBuilder.DropIndex(
                name: "IX_Hcc_NameEn",
                table: "Hcc");

            migrationBuilder.DropIndex(
                name: "IX_HazardDivisions_NameAr",
                table: "HazardDivisions");

            migrationBuilder.DropIndex(
                name: "IX_HazardDivisions_NameEn",
                table: "HazardDivisions");

            migrationBuilder.DropIndex(
                name: "IX_Depots_NameAr",
                table: "Depots");

            migrationBuilder.DropIndex(
                name: "IX_Depots_NameEn",
                table: "Depots");

            migrationBuilder.DropIndex(
                name: "IX_Departments_Code",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Departments_NameAr",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Departments_NameEn",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Countries_Code",
                table: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_Countries_NameAr",
                table: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_Countries_NameEn",
                table: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_Compatibilities_NameAr",
                table: "Compatibilities");

            migrationBuilder.DropIndex(
                name: "IX_Compatibilities_NameEn",
                table: "Compatibilities");

            migrationBuilder.DropIndex(
                name: "IX_Colors_NameAr",
                table: "Colors");

            migrationBuilder.DropIndex(
                name: "IX_Colors_NameEn",
                table: "Colors");

            migrationBuilder.DropIndex(
                name: "IX_CaseTypes_NameAr",
                table: "CaseTypes");

            migrationBuilder.DropIndex(
                name: "IX_CaseTypes_NameEn",
                table: "CaseTypes");

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
                name: "IX_Units_NameAr",
                table: "Units",
                column: "NameAr",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Units_NameEn",
                table: "Units",
                column: "NameEn",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_NameAr",
                table: "Suppliers",
                column: "NameAr",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_NameEn",
                table: "Suppliers",
                column: "NameEn",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Propellants_NameAr",
                table: "Propellants",
                column: "NameAr",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Propellants_NameEn",
                table: "Propellants",
                column: "NameEn",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectailMaterials_NameAr",
                table: "ProjectailMaterials",
                column: "NameAr",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectailMaterials_NameEn",
                table: "ProjectailMaterials",
                column: "NameEn",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_PrimaryPurposes_NameAr",
                table: "PrimaryPurposes",
                column: "NameAr",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_PrimaryPurposes_NameEn",
                table: "PrimaryPurposes",
                column: "NameEn",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Nsn_NameAr",
                table: "Nsn",
                column: "NameAr",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Nsn_NameEn",
                table: "Nsn",
                column: "NameEn",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_NatureOptions_NameAr",
                table: "NatureOptions",
                column: "NameAr",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_NatureOptions_NameEn",
                table: "NatureOptions",
                column: "NameEn",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Manufacturers_NameAr",
                table: "Manufacturers",
                column: "NameAr",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Manufacturers_NameEn",
                table: "Manufacturers",
                column: "NameEn",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Hcc_NameAr",
                table: "Hcc",
                column: "NameAr",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Hcc_NameEn",
                table: "Hcc",
                column: "NameEn",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_HazardDivisions_NameAr",
                table: "HazardDivisions",
                column: "NameAr",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_HazardDivisions_NameEn",
                table: "HazardDivisions",
                column: "NameEn",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Depots_NameAr",
                table: "Depots",
                column: "NameAr",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Depots_NameEn",
                table: "Depots",
                column: "NameEn",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_Code",
                table: "Departments",
                column: "Code",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_NameAr",
                table: "Departments",
                column: "NameAr",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_NameEn",
                table: "Departments",
                column: "NameEn",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_Code",
                table: "Countries",
                column: "Code",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_NameAr",
                table: "Countries",
                column: "NameAr",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_NameEn",
                table: "Countries",
                column: "NameEn",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Compatibilities_NameAr",
                table: "Compatibilities",
                column: "NameAr",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Compatibilities_NameEn",
                table: "Compatibilities",
                column: "NameEn",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Colors_NameAr",
                table: "Colors",
                column: "NameAr",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Colors_NameEn",
                table: "Colors",
                column: "NameEn",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_CaseTypes_NameAr",
                table: "CaseTypes",
                column: "NameAr",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_CaseTypes_NameEn",
                table: "CaseTypes",
                column: "NameEn",
                unique: true,
                filter: "[IsDeleted] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Units_NameAr",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Units_NameEn",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Suppliers_NameAr",
                table: "Suppliers");

            migrationBuilder.DropIndex(
                name: "IX_Suppliers_NameEn",
                table: "Suppliers");

            migrationBuilder.DropIndex(
                name: "IX_Propellants_NameAr",
                table: "Propellants");

            migrationBuilder.DropIndex(
                name: "IX_Propellants_NameEn",
                table: "Propellants");

            migrationBuilder.DropIndex(
                name: "IX_ProjectailMaterials_NameAr",
                table: "ProjectailMaterials");

            migrationBuilder.DropIndex(
                name: "IX_ProjectailMaterials_NameEn",
                table: "ProjectailMaterials");

            migrationBuilder.DropIndex(
                name: "IX_PrimaryPurposes_NameAr",
                table: "PrimaryPurposes");

            migrationBuilder.DropIndex(
                name: "IX_PrimaryPurposes_NameEn",
                table: "PrimaryPurposes");

            migrationBuilder.DropIndex(
                name: "IX_Nsn_NameAr",
                table: "Nsn");

            migrationBuilder.DropIndex(
                name: "IX_Nsn_NameEn",
                table: "Nsn");

            migrationBuilder.DropIndex(
                name: "IX_NatureOptions_NameAr",
                table: "NatureOptions");

            migrationBuilder.DropIndex(
                name: "IX_NatureOptions_NameEn",
                table: "NatureOptions");

            migrationBuilder.DropIndex(
                name: "IX_Manufacturers_NameAr",
                table: "Manufacturers");

            migrationBuilder.DropIndex(
                name: "IX_Manufacturers_NameEn",
                table: "Manufacturers");

            migrationBuilder.DropIndex(
                name: "IX_Hcc_NameAr",
                table: "Hcc");

            migrationBuilder.DropIndex(
                name: "IX_Hcc_NameEn",
                table: "Hcc");

            migrationBuilder.DropIndex(
                name: "IX_HazardDivisions_NameAr",
                table: "HazardDivisions");

            migrationBuilder.DropIndex(
                name: "IX_HazardDivisions_NameEn",
                table: "HazardDivisions");

            migrationBuilder.DropIndex(
                name: "IX_Depots_NameAr",
                table: "Depots");

            migrationBuilder.DropIndex(
                name: "IX_Depots_NameEn",
                table: "Depots");

            migrationBuilder.DropIndex(
                name: "IX_Departments_Code",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Departments_NameAr",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Departments_NameEn",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Countries_Code",
                table: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_Countries_NameAr",
                table: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_Countries_NameEn",
                table: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_Compatibilities_NameAr",
                table: "Compatibilities");

            migrationBuilder.DropIndex(
                name: "IX_Compatibilities_NameEn",
                table: "Compatibilities");

            migrationBuilder.DropIndex(
                name: "IX_Colors_NameAr",
                table: "Colors");

            migrationBuilder.DropIndex(
                name: "IX_Colors_NameEn",
                table: "Colors");

            migrationBuilder.DropIndex(
                name: "IX_CaseTypes_NameAr",
                table: "CaseTypes");

            migrationBuilder.DropIndex(
                name: "IX_CaseTypes_NameEn",
                table: "CaseTypes");

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 275, DateTimeKind.Unspecified).AddTicks(980));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 275, DateTimeKind.Unspecified).AddTicks(1043));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 275, DateTimeKind.Unspecified).AddTicks(1045));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 275, DateTimeKind.Unspecified).AddTicks(1047));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 275, DateTimeKind.Unspecified).AddTicks(1048));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 275, DateTimeKind.Unspecified).AddTicks(3747));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 275, DateTimeKind.Unspecified).AddTicks(3764));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 275, DateTimeKind.Unspecified).AddTicks(3766));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 275, DateTimeKind.Unspecified).AddTicks(3767));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 275, DateTimeKind.Unspecified).AddTicks(3769));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 275, DateTimeKind.Unspecified).AddTicks(6732));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 275, DateTimeKind.Unspecified).AddTicks(6752));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 275, DateTimeKind.Unspecified).AddTicks(6754));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 275, DateTimeKind.Unspecified).AddTicks(6755));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 275, DateTimeKind.Unspecified).AddTicks(6757));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 275, DateTimeKind.Unspecified).AddTicks(8068));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 275, DateTimeKind.Unspecified).AddTicks(8083));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 275, DateTimeKind.Unspecified).AddTicks(8085));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 275, DateTimeKind.Unspecified).AddTicks(8087));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 275, DateTimeKind.Unspecified).AddTicks(9282));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 275, DateTimeKind.Unspecified).AddTicks(9290));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 275, DateTimeKind.Unspecified).AddTicks(9292));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 275, DateTimeKind.Unspecified).AddTicks(9293));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 275, DateTimeKind.Unspecified).AddTicks(9295));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 276, DateTimeKind.Unspecified).AddTicks(492));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 276, DateTimeKind.Unspecified).AddTicks(501));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 276, DateTimeKind.Unspecified).AddTicks(502));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 276, DateTimeKind.Unspecified).AddTicks(503));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 276, DateTimeKind.Unspecified).AddTicks(505));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 276, DateTimeKind.Unspecified).AddTicks(5007));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 276, DateTimeKind.Unspecified).AddTicks(5026));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 276, DateTimeKind.Unspecified).AddTicks(5028));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 276, DateTimeKind.Unspecified).AddTicks(5054));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 276, DateTimeKind.Unspecified).AddTicks(5055));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 276, DateTimeKind.Unspecified).AddTicks(6301));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 276, DateTimeKind.Unspecified).AddTicks(6312));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 276, DateTimeKind.Unspecified).AddTicks(6313));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 276, DateTimeKind.Unspecified).AddTicks(6315));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 276, DateTimeKind.Unspecified).AddTicks(6316));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 276, DateTimeKind.Unspecified).AddTicks(7565));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 276, DateTimeKind.Unspecified).AddTicks(7574));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 276, DateTimeKind.Unspecified).AddTicks(7576));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 276, DateTimeKind.Unspecified).AddTicks(7577));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 276, DateTimeKind.Unspecified).AddTicks(7578));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 276, DateTimeKind.Unspecified).AddTicks(8902));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 276, DateTimeKind.Unspecified).AddTicks(8910));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 276, DateTimeKind.Unspecified).AddTicks(8912));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 276, DateTimeKind.Unspecified).AddTicks(8913));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 276, DateTimeKind.Unspecified).AddTicks(8914));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 277, DateTimeKind.Unspecified).AddTicks(125));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 277, DateTimeKind.Unspecified).AddTicks(133));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 277, DateTimeKind.Unspecified).AddTicks(135));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 277, DateTimeKind.Unspecified).AddTicks(136));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 277, DateTimeKind.Unspecified).AddTicks(138));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 277, DateTimeKind.Unspecified).AddTicks(1328));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 277, DateTimeKind.Unspecified).AddTicks(1338));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 277, DateTimeKind.Unspecified).AddTicks(1339));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 277, DateTimeKind.Unspecified).AddTicks(1340));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 277, DateTimeKind.Unspecified).AddTicks(1341));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 277, DateTimeKind.Unspecified).AddTicks(2487));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 277, DateTimeKind.Unspecified).AddTicks(2495));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 277, DateTimeKind.Unspecified).AddTicks(2496));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 277, DateTimeKind.Unspecified).AddTicks(2498));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 277, DateTimeKind.Unspecified).AddTicks(2499));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 277, DateTimeKind.Unspecified).AddTicks(3708));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 277, DateTimeKind.Unspecified).AddTicks(3717));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 277, DateTimeKind.Unspecified).AddTicks(3719));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 277, DateTimeKind.Unspecified).AddTicks(3720));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 2, 8, 1, 43, 277, DateTimeKind.Unspecified).AddTicks(3721));

            migrationBuilder.CreateIndex(
                name: "IX_Units_NameAr",
                table: "Units",
                column: "NameAr",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Units_NameEn",
                table: "Units",
                column: "NameEn",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_NameAr",
                table: "Suppliers",
                column: "NameAr",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_NameEn",
                table: "Suppliers",
                column: "NameEn",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Propellants_NameAr",
                table: "Propellants",
                column: "NameAr",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Propellants_NameEn",
                table: "Propellants",
                column: "NameEn",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectailMaterials_NameAr",
                table: "ProjectailMaterials",
                column: "NameAr",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectailMaterials_NameEn",
                table: "ProjectailMaterials",
                column: "NameEn",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PrimaryPurposes_NameAr",
                table: "PrimaryPurposes",
                column: "NameAr",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PrimaryPurposes_NameEn",
                table: "PrimaryPurposes",
                column: "NameEn",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Nsn_NameAr",
                table: "Nsn",
                column: "NameAr",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Nsn_NameEn",
                table: "Nsn",
                column: "NameEn",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NatureOptions_NameAr",
                table: "NatureOptions",
                column: "NameAr",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NatureOptions_NameEn",
                table: "NatureOptions",
                column: "NameEn",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Manufacturers_NameAr",
                table: "Manufacturers",
                column: "NameAr",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Manufacturers_NameEn",
                table: "Manufacturers",
                column: "NameEn",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Hcc_NameAr",
                table: "Hcc",
                column: "NameAr",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Hcc_NameEn",
                table: "Hcc",
                column: "NameEn",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HazardDivisions_NameAr",
                table: "HazardDivisions",
                column: "NameAr",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HazardDivisions_NameEn",
                table: "HazardDivisions",
                column: "NameEn",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Depots_NameAr",
                table: "Depots",
                column: "NameAr",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Depots_NameEn",
                table: "Depots",
                column: "NameEn",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_Code",
                table: "Departments",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_NameAr",
                table: "Departments",
                column: "NameAr",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_NameEn",
                table: "Departments",
                column: "NameEn",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Countries_Code",
                table: "Countries",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Countries_NameAr",
                table: "Countries",
                column: "NameAr",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Countries_NameEn",
                table: "Countries",
                column: "NameEn",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Compatibilities_NameAr",
                table: "Compatibilities",
                column: "NameAr",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Compatibilities_NameEn",
                table: "Compatibilities",
                column: "NameEn",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Colors_NameAr",
                table: "Colors",
                column: "NameAr",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Colors_NameEn",
                table: "Colors",
                column: "NameEn",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CaseTypes_NameAr",
                table: "CaseTypes",
                column: "NameAr",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CaseTypes_NameEn",
                table: "CaseTypes",
                column: "NameEn",
                unique: true);
        }
    }
}
