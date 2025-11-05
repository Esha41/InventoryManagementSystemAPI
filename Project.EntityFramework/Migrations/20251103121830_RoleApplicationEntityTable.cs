using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class RoleApplicationEntityTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RoleApplicationEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApplicationEntityId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleApplicationEntities", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 220, DateTimeKind.Unspecified).AddTicks(3510));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 220, DateTimeKind.Unspecified).AddTicks(3588));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 220, DateTimeKind.Unspecified).AddTicks(3590));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 220, DateTimeKind.Unspecified).AddTicks(3592));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 220, DateTimeKind.Unspecified).AddTicks(3594));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 220, DateTimeKind.Unspecified).AddTicks(3596));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 220, DateTimeKind.Unspecified).AddTicks(3597));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 220, DateTimeKind.Unspecified).AddTicks(3599));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 221, DateTimeKind.Unspecified).AddTicks(4489));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 221, DateTimeKind.Unspecified).AddTicks(4519));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 221, DateTimeKind.Unspecified).AddTicks(4520));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 221, DateTimeKind.Unspecified).AddTicks(4522));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 221, DateTimeKind.Unspecified).AddTicks(4524));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 221, DateTimeKind.Unspecified).AddTicks(7650));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 221, DateTimeKind.Unspecified).AddTicks(7668));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 221, DateTimeKind.Unspecified).AddTicks(7670));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 221, DateTimeKind.Unspecified).AddTicks(7672));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 221, DateTimeKind.Unspecified).AddTicks(7674));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 222, DateTimeKind.Unspecified).AddTicks(1133));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 222, DateTimeKind.Unspecified).AddTicks(1153));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 222, DateTimeKind.Unspecified).AddTicks(1155));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 222, DateTimeKind.Unspecified).AddTicks(1157));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 222, DateTimeKind.Unspecified).AddTicks(1159));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 222, DateTimeKind.Unspecified).AddTicks(2748));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 222, DateTimeKind.Unspecified).AddTicks(2766));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 222, DateTimeKind.Unspecified).AddTicks(2769));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 222, DateTimeKind.Unspecified).AddTicks(2771));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 222, DateTimeKind.Unspecified).AddTicks(4189));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 222, DateTimeKind.Unspecified).AddTicks(4201));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 222, DateTimeKind.Unspecified).AddTicks(4202));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 222, DateTimeKind.Unspecified).AddTicks(4204));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 222, DateTimeKind.Unspecified).AddTicks(4206));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 222, DateTimeKind.Unspecified).AddTicks(5707));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 222, DateTimeKind.Unspecified).AddTicks(5720));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 222, DateTimeKind.Unspecified).AddTicks(5721));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 222, DateTimeKind.Unspecified).AddTicks(5723));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 222, DateTimeKind.Unspecified).AddTicks(5725));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 223, DateTimeKind.Unspecified).AddTicks(3005));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 223, DateTimeKind.Unspecified).AddTicks(3028));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 223, DateTimeKind.Unspecified).AddTicks(3030));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 223, DateTimeKind.Unspecified).AddTicks(3031));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 223, DateTimeKind.Unspecified).AddTicks(3033));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 223, DateTimeKind.Unspecified).AddTicks(4619));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 223, DateTimeKind.Unspecified).AddTicks(4630));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 223, DateTimeKind.Unspecified).AddTicks(4632));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 223, DateTimeKind.Unspecified).AddTicks(4633));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 223, DateTimeKind.Unspecified).AddTicks(4635));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 223, DateTimeKind.Unspecified).AddTicks(6094));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 223, DateTimeKind.Unspecified).AddTicks(6104));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 223, DateTimeKind.Unspecified).AddTicks(6105));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 223, DateTimeKind.Unspecified).AddTicks(6107));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 223, DateTimeKind.Unspecified).AddTicks(6108));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 223, DateTimeKind.Unspecified).AddTicks(7570));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 223, DateTimeKind.Unspecified).AddTicks(7579));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 223, DateTimeKind.Unspecified).AddTicks(7581));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 223, DateTimeKind.Unspecified).AddTicks(7584));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 223, DateTimeKind.Unspecified).AddTicks(7586));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 223, DateTimeKind.Unspecified).AddTicks(9008));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 223, DateTimeKind.Unspecified).AddTicks(9018));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 223, DateTimeKind.Unspecified).AddTicks(9020));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 223, DateTimeKind.Unspecified).AddTicks(9021));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 223, DateTimeKind.Unspecified).AddTicks(9023));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 224, DateTimeKind.Unspecified).AddTicks(473));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 224, DateTimeKind.Unspecified).AddTicks(482));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 224, DateTimeKind.Unspecified).AddTicks(484));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 224, DateTimeKind.Unspecified).AddTicks(486));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 224, DateTimeKind.Unspecified).AddTicks(488));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 224, DateTimeKind.Unspecified).AddTicks(1858));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 224, DateTimeKind.Unspecified).AddTicks(1867));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 224, DateTimeKind.Unspecified).AddTicks(1868));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 224, DateTimeKind.Unspecified).AddTicks(1870));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 224, DateTimeKind.Unspecified).AddTicks(1872));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 224, DateTimeKind.Unspecified).AddTicks(3353));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 224, DateTimeKind.Unspecified).AddTicks(3364));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 224, DateTimeKind.Unspecified).AddTicks(3365));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 224, DateTimeKind.Unspecified).AddTicks(3367));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 15, 18, 30, 224, DateTimeKind.Unspecified).AddTicks(3369));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleApplicationEntities");

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 944, DateTimeKind.Unspecified).AddTicks(1443));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 944, DateTimeKind.Unspecified).AddTicks(1517));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 944, DateTimeKind.Unspecified).AddTicks(1520));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 944, DateTimeKind.Unspecified).AddTicks(1522));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 944, DateTimeKind.Unspecified).AddTicks(1524));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 944, DateTimeKind.Unspecified).AddTicks(1526));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 944, DateTimeKind.Unspecified).AddTicks(1527));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 3, 14, 39, 10, 944, DateTimeKind.Unspecified).AddTicks(1529));

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
        }
    }
}
