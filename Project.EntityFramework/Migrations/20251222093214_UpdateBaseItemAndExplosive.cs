using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBaseItemAndExplosive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Explosives_Compatibilities_CompatibilityId",
                table: "Explosives");

            migrationBuilder.DropForeignKey(
                name: "FK_Explosives_HazardDivisions_HazardDivisionId",
                table: "Explosives");

            migrationBuilder.DropForeignKey(
                name: "FK_Explosives_Units_NetExplosiveQuantityUnitId",
                table: "Explosives");

            migrationBuilder.DropForeignKey(
                name: "FK_Explosives_Units_TotalWeightUnitId",
                table: "Explosives");

            migrationBuilder.DropIndex(
                name: "IX_Explosives_CompatibilityId",
                table: "Explosives");

            migrationBuilder.DropIndex(
                name: "IX_Explosives_NetExplosiveQuantityUnitId",
                table: "Explosives");

            migrationBuilder.DropIndex(
                name: "IX_Explosives_TotalWeightUnitId",
                table: "Explosives");

            migrationBuilder.DropColumn(
                name: "CompatibilityId",
                table: "Explosives");

            migrationBuilder.DropColumn(
                name: "NetExplosiveQuantity",
                table: "Explosives");

            migrationBuilder.DropColumn(
                name: "NetExplosiveQuantityUnitId",
                table: "Explosives");

            migrationBuilder.DropColumn(
                name: "TotalWeight",
                table: "Explosives");

            migrationBuilder.DropColumn(
                name: "TotalWeightUnitId",
                table: "Explosives");

            migrationBuilder.DropColumn(
                name: "UNNumber",
                table: "Explosives");

            migrationBuilder.RenameColumn(
                name: "ExplosiveType",
                table: "Explosives",
                newName: "Unit");

            migrationBuilder.AddColumn<int>(
                name: "YearOfManufacture",
                table: "InventoryDetails",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ClassificationId",
                table: "BaseItems",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Distribution",
                table: "BaseItems",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "BaseItems",
                type: "nvarchar(max)",
                maxLength: 5000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferenceNo",
                table: "BaseItems",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TypeId",
                table: "BaseItems",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UNNumber",
                table: "BaseItems",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Classifications",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                    table.PrimaryKey("PK_Classifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItemTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ItemType = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemTypes", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "BaseItems",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ClassificationId", "Distribution", "Notes", "ReferenceNo", "TypeId", "UNNumber" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "BaseItems",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "ClassificationId", "Distribution", "Notes", "ReferenceNo", "TypeId", "UNNumber" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "BaseItems",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "ClassificationId", "Distribution", "Notes", "ReferenceNo", "TypeId", "UNNumber" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "BaseItems",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "ClassificationId", "Distribution", "Notes", "ReferenceNo", "TypeId", "UNNumber" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "BaseItems",
                keyColumn: "Id",
                keyValue: 5L,
                columns: new[] { "ClassificationId", "Distribution", "Notes", "ReferenceNo", "TypeId", "UNNumber" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "BaseItems",
                keyColumn: "Id",
                keyValue: 6L,
                columns: new[] { "ClassificationId", "Distribution", "Notes", "ReferenceNo", "TypeId", "UNNumber" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "BaseItems",
                keyColumn: "Id",
                keyValue: 7L,
                columns: new[] { "ClassificationId", "Distribution", "Notes", "ReferenceNo", "TypeId", "UNNumber" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "BaseItems",
                keyColumn: "Id",
                keyValue: 8L,
                columns: new[] { "ClassificationId", "Distribution", "Notes", "ReferenceNo", "TypeId", "UNNumber" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "BaseItems",
                keyColumn: "Id",
                keyValue: 9L,
                columns: new[] { "ClassificationId", "Distribution", "Notes", "ReferenceNo", "TypeId", "UNNumber" },
                values: new object[] { null, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 226, DateTimeKind.Unspecified).AddTicks(9119));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 226, DateTimeKind.Unspecified).AddTicks(9148));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 226, DateTimeKind.Unspecified).AddTicks(9150));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 226, DateTimeKind.Unspecified).AddTicks(9152));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 226, DateTimeKind.Unspecified).AddTicks(9153));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 227, DateTimeKind.Unspecified).AddTicks(3955));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 227, DateTimeKind.Unspecified).AddTicks(3979));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 227, DateTimeKind.Unspecified).AddTicks(3980));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 227, DateTimeKind.Unspecified).AddTicks(3982));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 227, DateTimeKind.Unspecified).AddTicks(3984));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 227, DateTimeKind.Unspecified).AddTicks(7415));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 227, DateTimeKind.Unspecified).AddTicks(7436));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 227, DateTimeKind.Unspecified).AddTicks(7438));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 227, DateTimeKind.Unspecified).AddTicks(7440));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 227, DateTimeKind.Unspecified).AddTicks(7442));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 227, DateTimeKind.Unspecified).AddTicks(7443));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 227, DateTimeKind.Unspecified).AddTicks(7445));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 227, DateTimeKind.Unspecified).AddTicks(7447));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 227, DateTimeKind.Unspecified).AddTicks(7449));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 227, DateTimeKind.Unspecified).AddTicks(7451));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 227, DateTimeKind.Unspecified).AddTicks(7453));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 227, DateTimeKind.Unspecified).AddTicks(7478));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 227, DateTimeKind.Unspecified).AddTicks(7480));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 227, DateTimeKind.Unspecified).AddTicks(7482));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 15L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 227, DateTimeKind.Unspecified).AddTicks(7484));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 16L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 227, DateTimeKind.Unspecified).AddTicks(7486));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 17L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 227, DateTimeKind.Unspecified).AddTicks(7487));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 18L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 227, DateTimeKind.Unspecified).AddTicks(7489));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 227, DateTimeKind.Unspecified).AddTicks(9247));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 227, DateTimeKind.Unspecified).AddTicks(9263));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 227, DateTimeKind.Unspecified).AddTicks(9266));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 227, DateTimeKind.Unspecified).AddTicks(9268));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 228, DateTimeKind.Unspecified).AddTicks(7609));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 228, DateTimeKind.Unspecified).AddTicks(7634));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 228, DateTimeKind.Unspecified).AddTicks(7636));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 228, DateTimeKind.Unspecified).AddTicks(7638));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 228, DateTimeKind.Unspecified).AddTicks(7639));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 229, DateTimeKind.Unspecified).AddTicks(8208));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 229, DateTimeKind.Unspecified).AddTicks(8234));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 229, DateTimeKind.Unspecified).AddTicks(8236));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 229, DateTimeKind.Unspecified).AddTicks(8238));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 229, DateTimeKind.Unspecified).AddTicks(8240));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 229, DateTimeKind.Unspecified).AddTicks(9824));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 229, DateTimeKind.Unspecified).AddTicks(9837));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 229, DateTimeKind.Unspecified).AddTicks(9839));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 229, DateTimeKind.Unspecified).AddTicks(9841));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 229, DateTimeKind.Unspecified).AddTicks(9843));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 229, DateTimeKind.Unspecified).AddTicks(9844));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 229, DateTimeKind.Unspecified).AddTicks(9846));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 229, DateTimeKind.Unspecified).AddTicks(9847));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 229, DateTimeKind.Unspecified).AddTicks(9848));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 229, DateTimeKind.Unspecified).AddTicks(9850));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 229, DateTimeKind.Unspecified).AddTicks(9851));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 230, DateTimeKind.Unspecified).AddTicks(8610));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 230, DateTimeKind.Unspecified).AddTicks(8636));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 230, DateTimeKind.Unspecified).AddTicks(8639));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 230, DateTimeKind.Unspecified).AddTicks(8640));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 230, DateTimeKind.Unspecified).AddTicks(8666));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 231, DateTimeKind.Unspecified).AddTicks(346));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 231, DateTimeKind.Unspecified).AddTicks(361));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 231, DateTimeKind.Unspecified).AddTicks(363));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 231, DateTimeKind.Unspecified).AddTicks(365));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 231, DateTimeKind.Unspecified).AddTicks(367));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 231, DateTimeKind.Unspecified).AddTicks(1909));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 231, DateTimeKind.Unspecified).AddTicks(1923));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 231, DateTimeKind.Unspecified).AddTicks(1925));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 231, DateTimeKind.Unspecified).AddTicks(1927));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 231, DateTimeKind.Unspecified).AddTicks(1928));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 231, DateTimeKind.Unspecified).AddTicks(3388));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 231, DateTimeKind.Unspecified).AddTicks(3400));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 231, DateTimeKind.Unspecified).AddTicks(3402));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 231, DateTimeKind.Unspecified).AddTicks(3404));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 231, DateTimeKind.Unspecified).AddTicks(3405));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 231, DateTimeKind.Unspecified).AddTicks(3407));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 231, DateTimeKind.Unspecified).AddTicks(3409));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 231, DateTimeKind.Unspecified).AddTicks(3411));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 231, DateTimeKind.Unspecified).AddTicks(3412));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 231, DateTimeKind.Unspecified).AddTicks(3414));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 231, DateTimeKind.Unspecified).AddTicks(7363));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 231, DateTimeKind.Unspecified).AddTicks(7388));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 231, DateTimeKind.Unspecified).AddTicks(7390));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 231, DateTimeKind.Unspecified).AddTicks(7392));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 231, DateTimeKind.Unspecified).AddTicks(7395));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 231, DateTimeKind.Unspecified).AddTicks(7397));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 231, DateTimeKind.Unspecified).AddTicks(7398));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 231, DateTimeKind.Unspecified).AddTicks(7400));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 231, DateTimeKind.Unspecified).AddTicks(7402));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 231, DateTimeKind.Unspecified).AddTicks(7403));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 231, DateTimeKind.Unspecified).AddTicks(7405));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 231, DateTimeKind.Unspecified).AddTicks(7393));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 231, DateTimeKind.Unspecified).AddTicks(9624));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 231, DateTimeKind.Unspecified).AddTicks(9643));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 231, DateTimeKind.Unspecified).AddTicks(9645));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 231, DateTimeKind.Unspecified).AddTicks(9646));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 231, DateTimeKind.Unspecified).AddTicks(9648));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 232, DateTimeKind.Unspecified).AddTicks(1204));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 232, DateTimeKind.Unspecified).AddTicks(1218));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 232, DateTimeKind.Unspecified).AddTicks(1221));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 232, DateTimeKind.Unspecified).AddTicks(1222));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 232, DateTimeKind.Unspecified).AddTicks(1224));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 232, DateTimeKind.Unspecified).AddTicks(7702));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 232, DateTimeKind.Unspecified).AddTicks(7728));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 232, DateTimeKind.Unspecified).AddTicks(7730));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 234, DateTimeKind.Unspecified).AddTicks(4461));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 234, DateTimeKind.Unspecified).AddTicks(4486));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 22, 12, 32, 14, 234, DateTimeKind.Unspecified).AddTicks(4488));

            migrationBuilder.CreateIndex(
                name: "IX_BaseItems_ClassificationId",
                table: "BaseItems",
                column: "ClassificationId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseItems_TypeId",
                table: "BaseItems",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Classifications_NameAr",
                table: "Classifications",
                column: "NameAr",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Classifications_NameEn",
                table: "Classifications",
                column: "NameEn",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_ItemTypes_NameAr_ItemType",
                table: "ItemTypes",
                columns: new[] { "NameAr", "ItemType" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_ItemTypes_NameEn_ItemType",
                table: "ItemTypes",
                columns: new[] { "NameEn", "ItemType" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.AddForeignKey(
                name: "FK_BaseItems_Classifications_ClassificationId",
                table: "BaseItems",
                column: "ClassificationId",
                principalTable: "Classifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BaseItems_ItemTypes_TypeId",
                table: "BaseItems",
                column: "TypeId",
                principalTable: "ItemTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Explosives_HazardDivisions_HazardDivisionId",
                table: "Explosives",
                column: "HazardDivisionId",
                principalTable: "HazardDivisions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BaseItems_Classifications_ClassificationId",
                table: "BaseItems");

            migrationBuilder.DropForeignKey(
                name: "FK_BaseItems_ItemTypes_TypeId",
                table: "BaseItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Explosives_HazardDivisions_HazardDivisionId",
                table: "Explosives");

            migrationBuilder.DropTable(
                name: "Classifications");

            migrationBuilder.DropTable(
                name: "ItemTypes");

            migrationBuilder.DropIndex(
                name: "IX_BaseItems_ClassificationId",
                table: "BaseItems");

            migrationBuilder.DropIndex(
                name: "IX_BaseItems_TypeId",
                table: "BaseItems");

            migrationBuilder.DropColumn(
                name: "YearOfManufacture",
                table: "InventoryDetails");

            migrationBuilder.DropColumn(
                name: "ClassificationId",
                table: "BaseItems");

            migrationBuilder.DropColumn(
                name: "Distribution",
                table: "BaseItems");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "BaseItems");

            migrationBuilder.DropColumn(
                name: "ReferenceNo",
                table: "BaseItems");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "BaseItems");

            migrationBuilder.DropColumn(
                name: "UNNumber",
                table: "BaseItems");

            migrationBuilder.RenameColumn(
                name: "Unit",
                table: "Explosives",
                newName: "ExplosiveType");

            migrationBuilder.AddColumn<long>(
                name: "CompatibilityId",
                table: "Explosives",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "NetExplosiveQuantity",
                table: "Explosives",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "NetExplosiveQuantityUnitId",
                table: "Explosives",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalWeight",
                table: "Explosives",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TotalWeightUnitId",
                table: "Explosives",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UNNumber",
                table: "Explosives",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 268, DateTimeKind.Unspecified).AddTicks(6808));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 268, DateTimeKind.Unspecified).AddTicks(6831));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 268, DateTimeKind.Unspecified).AddTicks(6833));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 268, DateTimeKind.Unspecified).AddTicks(6834));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 268, DateTimeKind.Unspecified).AddTicks(6835));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 268, DateTimeKind.Unspecified).AddTicks(9389));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 268, DateTimeKind.Unspecified).AddTicks(9404));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 268, DateTimeKind.Unspecified).AddTicks(9406));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 268, DateTimeKind.Unspecified).AddTicks(9407));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 268, DateTimeKind.Unspecified).AddTicks(9408));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 269, DateTimeKind.Unspecified).AddTicks(2386));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 269, DateTimeKind.Unspecified).AddTicks(2402));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 269, DateTimeKind.Unspecified).AddTicks(2403));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 269, DateTimeKind.Unspecified).AddTicks(2405));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 269, DateTimeKind.Unspecified).AddTicks(2406));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 269, DateTimeKind.Unspecified).AddTicks(2407));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 269, DateTimeKind.Unspecified).AddTicks(2408));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 269, DateTimeKind.Unspecified).AddTicks(2410));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 269, DateTimeKind.Unspecified).AddTicks(2411));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 269, DateTimeKind.Unspecified).AddTicks(2412));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 269, DateTimeKind.Unspecified).AddTicks(2414));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 269, DateTimeKind.Unspecified).AddTicks(2415));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 269, DateTimeKind.Unspecified).AddTicks(2416));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 269, DateTimeKind.Unspecified).AddTicks(2417));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 15L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 269, DateTimeKind.Unspecified).AddTicks(2418));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 16L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 269, DateTimeKind.Unspecified).AddTicks(2419));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 17L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 269, DateTimeKind.Unspecified).AddTicks(2420));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 18L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 269, DateTimeKind.Unspecified).AddTicks(2422));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 269, DateTimeKind.Unspecified).AddTicks(3968));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 269, DateTimeKind.Unspecified).AddTicks(3981));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 269, DateTimeKind.Unspecified).AddTicks(3983));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 269, DateTimeKind.Unspecified).AddTicks(3985));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 269, DateTimeKind.Unspecified).AddTicks(9789));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 269, DateTimeKind.Unspecified).AddTicks(9809));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 269, DateTimeKind.Unspecified).AddTicks(9811));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 269, DateTimeKind.Unspecified).AddTicks(9812));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 269, DateTimeKind.Unspecified).AddTicks(9813));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 270, DateTimeKind.Unspecified).AddTicks(6828));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 270, DateTimeKind.Unspecified).AddTicks(6848));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 270, DateTimeKind.Unspecified).AddTicks(6850));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 270, DateTimeKind.Unspecified).AddTicks(6851));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 270, DateTimeKind.Unspecified).AddTicks(6852));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 270, DateTimeKind.Unspecified).AddTicks(8158));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 270, DateTimeKind.Unspecified).AddTicks(8168));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 270, DateTimeKind.Unspecified).AddTicks(8169));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 270, DateTimeKind.Unspecified).AddTicks(8170));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 270, DateTimeKind.Unspecified).AddTicks(8171));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 270, DateTimeKind.Unspecified).AddTicks(8173));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 270, DateTimeKind.Unspecified).AddTicks(8174));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 270, DateTimeKind.Unspecified).AddTicks(8175));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 270, DateTimeKind.Unspecified).AddTicks(8176));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 270, DateTimeKind.Unspecified).AddTicks(8177));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 270, DateTimeKind.Unspecified).AddTicks(8178));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 271, DateTimeKind.Unspecified).AddTicks(5419));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 271, DateTimeKind.Unspecified).AddTicks(5440));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 271, DateTimeKind.Unspecified).AddTicks(5441));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 271, DateTimeKind.Unspecified).AddTicks(5442));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 271, DateTimeKind.Unspecified).AddTicks(5443));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 271, DateTimeKind.Unspecified).AddTicks(6718));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 271, DateTimeKind.Unspecified).AddTicks(6727));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 271, DateTimeKind.Unspecified).AddTicks(6729));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 271, DateTimeKind.Unspecified).AddTicks(6730));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 271, DateTimeKind.Unspecified).AddTicks(6731));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 271, DateTimeKind.Unspecified).AddTicks(7949));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 271, DateTimeKind.Unspecified).AddTicks(7958));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 271, DateTimeKind.Unspecified).AddTicks(7959));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 271, DateTimeKind.Unspecified).AddTicks(7961));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 271, DateTimeKind.Unspecified).AddTicks(7962));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 271, DateTimeKind.Unspecified).AddTicks(9174));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 271, DateTimeKind.Unspecified).AddTicks(9183));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 271, DateTimeKind.Unspecified).AddTicks(9184));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 271, DateTimeKind.Unspecified).AddTicks(9186));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 271, DateTimeKind.Unspecified).AddTicks(9187));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 271, DateTimeKind.Unspecified).AddTicks(9188));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 271, DateTimeKind.Unspecified).AddTicks(9189));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 271, DateTimeKind.Unspecified).AddTicks(9190));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 271, DateTimeKind.Unspecified).AddTicks(9191));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 271, DateTimeKind.Unspecified).AddTicks(9192));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 272, DateTimeKind.Unspecified).AddTicks(2313));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 272, DateTimeKind.Unspecified).AddTicks(2331));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 272, DateTimeKind.Unspecified).AddTicks(2333));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 272, DateTimeKind.Unspecified).AddTicks(2334));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 272, DateTimeKind.Unspecified).AddTicks(2337));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 272, DateTimeKind.Unspecified).AddTicks(2338));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 272, DateTimeKind.Unspecified).AddTicks(2339));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 272, DateTimeKind.Unspecified).AddTicks(2340));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 272, DateTimeKind.Unspecified).AddTicks(2341));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 272, DateTimeKind.Unspecified).AddTicks(2342));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 272, DateTimeKind.Unspecified).AddTicks(2343));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 272, DateTimeKind.Unspecified).AddTicks(2335));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 272, DateTimeKind.Unspecified).AddTicks(4199));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 272, DateTimeKind.Unspecified).AddTicks(4215));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 272, DateTimeKind.Unspecified).AddTicks(4217));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 272, DateTimeKind.Unspecified).AddTicks(4218));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 272, DateTimeKind.Unspecified).AddTicks(4219));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 272, DateTimeKind.Unspecified).AddTicks(5464));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 272, DateTimeKind.Unspecified).AddTicks(5473));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 272, DateTimeKind.Unspecified).AddTicks(5474));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 272, DateTimeKind.Unspecified).AddTicks(5475));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 272, DateTimeKind.Unspecified).AddTicks(5477));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 273, DateTimeKind.Unspecified).AddTicks(563));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 273, DateTimeKind.Unspecified).AddTicks(582));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 273, DateTimeKind.Unspecified).AddTicks(584));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 274, DateTimeKind.Unspecified).AddTicks(3591));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 274, DateTimeKind.Unspecified).AddTicks(3612));

            migrationBuilder.UpdateData(
                table: "WorkFlowType",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 12, 21, 8, 55, 47, 274, DateTimeKind.Unspecified).AddTicks(3613));

            migrationBuilder.CreateIndex(
                name: "IX_Explosives_CompatibilityId",
                table: "Explosives",
                column: "CompatibilityId");

            migrationBuilder.CreateIndex(
                name: "IX_Explosives_NetExplosiveQuantityUnitId",
                table: "Explosives",
                column: "NetExplosiveQuantityUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Explosives_TotalWeightUnitId",
                table: "Explosives",
                column: "TotalWeightUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Explosives_Compatibilities_CompatibilityId",
                table: "Explosives",
                column: "CompatibilityId",
                principalTable: "Compatibilities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Explosives_HazardDivisions_HazardDivisionId",
                table: "Explosives",
                column: "HazardDivisionId",
                principalTable: "HazardDivisions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Explosives_Units_NetExplosiveQuantityUnitId",
                table: "Explosives",
                column: "NetExplosiveQuantityUnitId",
                principalTable: "Units",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Explosives_Units_TotalWeightUnitId",
                table: "Explosives",
                column: "TotalWeightUnitId",
                principalTable: "Units",
                principalColumn: "Id");
        }
    }
}
