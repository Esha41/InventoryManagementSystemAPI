using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class CreateEntityTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationEntity",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationEntity", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ApplicationEntity",
                columns: new[] { "Id", "Code", "CreatedBy", "CreationDate", "IsDeleted", "ModificationDate", "ModifiedBy", "NameAr", "NameEn" },
                values: new object[,]
                {
                    { 1L, "ORE", null, new DateTime(2025, 11, 3, 14, 39, 10, 944, DateTimeKind.Unspecified).AddTicks(1443), false, null, null, "الجهة الطالبة للطلب", "Order Requesting Entity" },
                    { 2L, "MT", null, new DateTime(2025, 11, 3, 14, 39, 10, 944, DateTimeKind.Unspecified).AddTicks(1517), false, null, null, "التدريب العسكري", "Military Training" },
                    { 3L, "DoA", null, new DateTime(2025, 11, 3, 14, 39, 10, 944, DateTimeKind.Unspecified).AddTicks(1520), false, null, null, "مدير التسليح", "Director of Armament" },
                    { 4L, "MO", null, new DateTime(2025, 11, 3, 14, 39, 10, 944, DateTimeKind.Unspecified).AddTicks(1522), false, null, null, "العمليات العسكرية", "Military Operations" },
                    { 5L, "DCoS", null, new DateTime(2025, 11, 3, 14, 39, 10, 944, DateTimeKind.Unspecified).AddTicks(1524), false, null, null, "نائب رئيس الأركان", "Deputy Chief of Staff" },
                    { 6L, "Function", null, new DateTime(2025, 11, 3, 14, 39, 10, 944, DateTimeKind.Unspecified).AddTicks(1526), false, null, null, "وظيفة", "Function" },
                    { 7L, "CoS", null, new DateTime(2025, 11, 3, 14, 39, 10, 944, DateTimeKind.Unspecified).AddTicks(1527), false, null, null, "رئيس الأركان", "Chief of Staff" },
                    { 8L, "Inventory", null, new DateTime(2025, 11, 3, 14, 39, 10, 944, DateTimeKind.Unspecified).AddTicks(1529), false, null, null, "المخزون", "Inventory" }
                });

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 944, DateTimeKind.Unspecified).AddTicks(6890));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 944, DateTimeKind.Unspecified).AddTicks(6916));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 944, DateTimeKind.Unspecified).AddTicks(6919));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 944, DateTimeKind.Unspecified).AddTicks(6921));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 944, DateTimeKind.Unspecified).AddTicks(6922));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 945, DateTimeKind.Unspecified).AddTicks(252));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 945, DateTimeKind.Unspecified).AddTicks(271));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 945, DateTimeKind.Unspecified).AddTicks(273));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 945, DateTimeKind.Unspecified).AddTicks(274));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 945, DateTimeKind.Unspecified).AddTicks(276));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 945, DateTimeKind.Unspecified).AddTicks(4165));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 945, DateTimeKind.Unspecified).AddTicks(4187));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 945, DateTimeKind.Unspecified).AddTicks(4189));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 945, DateTimeKind.Unspecified).AddTicks(4191));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 945, DateTimeKind.Unspecified).AddTicks(4193));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 945, DateTimeKind.Unspecified).AddTicks(5767));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 945, DateTimeKind.Unspecified).AddTicks(5783));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 945, DateTimeKind.Unspecified).AddTicks(5786));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 945, DateTimeKind.Unspecified).AddTicks(5789));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 945, DateTimeKind.Unspecified).AddTicks(7383));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 945, DateTimeKind.Unspecified).AddTicks(7394));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 945, DateTimeKind.Unspecified).AddTicks(7397));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 945, DateTimeKind.Unspecified).AddTicks(7399));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 945, DateTimeKind.Unspecified).AddTicks(7400));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 945, DateTimeKind.Unspecified).AddTicks(9014));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 945, DateTimeKind.Unspecified).AddTicks(9025));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 945, DateTimeKind.Unspecified).AddTicks(9027));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 945, DateTimeKind.Unspecified).AddTicks(9029));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 945, DateTimeKind.Unspecified).AddTicks(9030));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 946, DateTimeKind.Unspecified).AddTicks(7039));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 946, DateTimeKind.Unspecified).AddTicks(7064));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 946, DateTimeKind.Unspecified).AddTicks(7066));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 946, DateTimeKind.Unspecified).AddTicks(7068));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 946, DateTimeKind.Unspecified).AddTicks(7070));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 946, DateTimeKind.Unspecified).AddTicks(8702));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 946, DateTimeKind.Unspecified).AddTicks(8715));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 946, DateTimeKind.Unspecified).AddTicks(8717));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 946, DateTimeKind.Unspecified).AddTicks(8745));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 946, DateTimeKind.Unspecified).AddTicks(8747));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 947, DateTimeKind.Unspecified).AddTicks(335));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 947, DateTimeKind.Unspecified).AddTicks(347));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 947, DateTimeKind.Unspecified).AddTicks(350));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 947, DateTimeKind.Unspecified).AddTicks(352));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 947, DateTimeKind.Unspecified).AddTicks(354));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 947, DateTimeKind.Unspecified).AddTicks(2083));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 947, DateTimeKind.Unspecified).AddTicks(2098));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 947, DateTimeKind.Unspecified).AddTicks(2100));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 947, DateTimeKind.Unspecified).AddTicks(2102));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 947, DateTimeKind.Unspecified).AddTicks(2104));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 947, DateTimeKind.Unspecified).AddTicks(3732));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 947, DateTimeKind.Unspecified).AddTicks(3744));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 947, DateTimeKind.Unspecified).AddTicks(3746));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 947, DateTimeKind.Unspecified).AddTicks(3748));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 947, DateTimeKind.Unspecified).AddTicks(3750));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 947, DateTimeKind.Unspecified).AddTicks(5310));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 947, DateTimeKind.Unspecified).AddTicks(5322));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 947, DateTimeKind.Unspecified).AddTicks(5324));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 947, DateTimeKind.Unspecified).AddTicks(5326));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 947, DateTimeKind.Unspecified).AddTicks(5328));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 947, DateTimeKind.Unspecified).AddTicks(6893));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 947, DateTimeKind.Unspecified).AddTicks(6905));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 947, DateTimeKind.Unspecified).AddTicks(6907));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 947, DateTimeKind.Unspecified).AddTicks(6909));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 947, DateTimeKind.Unspecified).AddTicks(6911));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 947, DateTimeKind.Unspecified).AddTicks(8585));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 947, DateTimeKind.Unspecified).AddTicks(8597));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 947, DateTimeKind.Unspecified).AddTicks(8599));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 947, DateTimeKind.Unspecified).AddTicks(8601));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 947, DateTimeKind.Unspecified).AddTicks(8602));

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationEntity_Code",
                table: "ApplicationEntity",
                column: "Code",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationEntity_NameAr",
                table: "ApplicationEntity",
                column: "NameAr",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationEntity_NameEn",
                table: "ApplicationEntity",
                column: "NameEn",
                unique: true,
                filter: "[IsDeleted] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationEntity");

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 132, DateTimeKind.Unspecified).AddTicks(2457));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 132, DateTimeKind.Unspecified).AddTicks(2576));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 132, DateTimeKind.Unspecified).AddTicks(2578));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 132, DateTimeKind.Unspecified).AddTicks(2580));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 132, DateTimeKind.Unspecified).AddTicks(2605));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 132, DateTimeKind.Unspecified).AddTicks(6022));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 132, DateTimeKind.Unspecified).AddTicks(6041));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 132, DateTimeKind.Unspecified).AddTicks(6075));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 132, DateTimeKind.Unspecified).AddTicks(6077));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 132, DateTimeKind.Unspecified).AddTicks(6079));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 132, DateTimeKind.Unspecified).AddTicks(9290));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 132, DateTimeKind.Unspecified).AddTicks(9311));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 132, DateTimeKind.Unspecified).AddTicks(9313));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 132, DateTimeKind.Unspecified).AddTicks(9315));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 132, DateTimeKind.Unspecified).AddTicks(9317));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 133, DateTimeKind.Unspecified).AddTicks(858));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 133, DateTimeKind.Unspecified).AddTicks(876));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 133, DateTimeKind.Unspecified).AddTicks(878));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 133, DateTimeKind.Unspecified).AddTicks(881));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 133, DateTimeKind.Unspecified).AddTicks(2333));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 133, DateTimeKind.Unspecified).AddTicks(2344));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 133, DateTimeKind.Unspecified).AddTicks(2346));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 133, DateTimeKind.Unspecified).AddTicks(2348));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 133, DateTimeKind.Unspecified).AddTicks(2350));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 133, DateTimeKind.Unspecified).AddTicks(3797));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 133, DateTimeKind.Unspecified).AddTicks(3808));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 133, DateTimeKind.Unspecified).AddTicks(3810));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 133, DateTimeKind.Unspecified).AddTicks(3812));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 133, DateTimeKind.Unspecified).AddTicks(3813));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(715));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(760));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(765));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(767));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(769));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(2283));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(2294));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(2296));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(2298));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(2299));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(3676));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(3685));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(3687));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(3689));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(3690));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(5029));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(5040));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(5042));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(5044));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(5046));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(6426));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(6437));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(6439));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(6440));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(6442));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(7775));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(7786));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(7788));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(7789));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(7791));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(9106));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(9116));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(9118));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(9120));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 134, DateTimeKind.Unspecified).AddTicks(9121));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 135, DateTimeKind.Unspecified).AddTicks(383));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 135, DateTimeKind.Unspecified).AddTicks(393));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 135, DateTimeKind.Unspecified).AddTicks(395));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 135, DateTimeKind.Unspecified).AddTicks(396));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 9, 51, 18, 135, DateTimeKind.Unspecified).AddTicks(398));
        }
    }
}
