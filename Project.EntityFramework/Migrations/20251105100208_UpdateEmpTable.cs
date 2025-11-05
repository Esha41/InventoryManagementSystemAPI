using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEmpTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Employees");

            migrationBuilder.AddColumn<string>(
                name: "NameAr",
                table: "Employees",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NameEn",
                table: "Employees",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

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
                columns: new[] { "CreationDate", "NameAr", "NameEn" },
                values: new object[] { new DateTime(2025, 11, 5, 13, 2, 7, 807, DateTimeKind.Unspecified).AddTicks(1813), "أحمد محمد العلي", "Ahmed Mohammed Al-Ali" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreationDate", "NameAr", "NameEn" },
                values: new object[] { new DateTime(2025, 11, 5, 13, 2, 7, 807, DateTimeKind.Unspecified).AddTicks(1833), "محمد عبدالله السالم", "Mohammed Abdullah Al-Salem" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreationDate", "NameAr", "NameEn" },
                values: new object[] { new DateTime(2025, 11, 5, 13, 2, 7, 807, DateTimeKind.Unspecified).AddTicks(1835), "خالد سعد الدوسري", "Khalid Saad Al-Dosari" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "CreationDate", "NameAr", "NameEn" },
                values: new object[] { new DateTime(2025, 11, 5, 13, 2, 7, 807, DateTimeKind.Unspecified).AddTicks(1837), "فهد عبدالرحمن القحطاني", "Fahad Abdulrahman Al-Qahtani" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 5L,
                columns: new[] { "CreationDate", "NameAr", "NameEn" },
                values: new object[] { new DateTime(2025, 11, 5, 13, 2, 7, 807, DateTimeKind.Unspecified).AddTicks(1838), "عبدالله يوسف العتيبي", "Abdullah Youssef Al-Otaibi" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 6L,
                columns: new[] { "CreationDate", "NameAr", "NameEn" },
                values: new object[] { new DateTime(2025, 11, 5, 13, 2, 7, 807, DateTimeKind.Unspecified).AddTicks(1840), "سعد علي الحربي", "Saad Ali Al-Harbi" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 7L,
                columns: new[] { "CreationDate", "NameAr", "NameEn" },
                values: new object[] { new DateTime(2025, 11, 5, 13, 2, 7, 807, DateTimeKind.Unspecified).AddTicks(1842), "عمر حسن الغامدي", "Omar Hassan Al-Ghamdi" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 8L,
                columns: new[] { "CreationDate", "NameAr", "NameEn" },
                values: new object[] { new DateTime(2025, 11, 5, 13, 2, 7, 807, DateTimeKind.Unspecified).AddTicks(1844), "يوسف إبراهيم الزهراني", "Youssef Ibrahim Al-Zahrani" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 9L,
                columns: new[] { "CreationDate", "NameAr", "NameEn" },
                values: new object[] { new DateTime(2025, 11, 5, 13, 2, 7, 807, DateTimeKind.Unspecified).AddTicks(1845), "علي محمود الشمري", "Ali Mahmoud Al-Shamri" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 10L,
                columns: new[] { "CreationDate", "NameAr", "NameEn" },
                values: new object[] { new DateTime(2025, 11, 5, 13, 2, 7, 807, DateTimeKind.Unspecified).AddTicks(1847), "حسن عبدالعزيز المطيري", "Hassan Abdulaziz Al-Mutairi" });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameAr",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "NameEn",
                table: "Employees");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Employees",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Employees",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 344, DateTimeKind.Unspecified).AddTicks(9328));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 344, DateTimeKind.Unspecified).AddTicks(9385));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 344, DateTimeKind.Unspecified).AddTicks(9387));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 344, DateTimeKind.Unspecified).AddTicks(9428));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 344, DateTimeKind.Unspecified).AddTicks(9430));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 345, DateTimeKind.Unspecified).AddTicks(2395));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 345, DateTimeKind.Unspecified).AddTicks(2413));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 345, DateTimeKind.Unspecified).AddTicks(2414));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 345, DateTimeKind.Unspecified).AddTicks(2441));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 345, DateTimeKind.Unspecified).AddTicks(2443));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 345, DateTimeKind.Unspecified).AddTicks(5548));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 345, DateTimeKind.Unspecified).AddTicks(5565));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 345, DateTimeKind.Unspecified).AddTicks(5567));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 345, DateTimeKind.Unspecified).AddTicks(5568));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 345, DateTimeKind.Unspecified).AddTicks(5570));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 345, DateTimeKind.Unspecified).AddTicks(6877));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 345, DateTimeKind.Unspecified).AddTicks(6890));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 345, DateTimeKind.Unspecified).AddTicks(6892));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 345, DateTimeKind.Unspecified).AddTicks(6894));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreationDate", "FirstName", "LastName" },
                values: new object[] { new DateTime(2025, 11, 5, 11, 54, 20, 345, DateTimeKind.Unspecified).AddTicks(9432), "أحمد", "محمد العلي" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreationDate", "FirstName", "LastName" },
                values: new object[] { new DateTime(2025, 11, 5, 11, 54, 20, 345, DateTimeKind.Unspecified).AddTicks(9453), "محمد", "عبدالله السالم" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreationDate", "FirstName", "LastName" },
                values: new object[] { new DateTime(2025, 11, 5, 11, 54, 20, 345, DateTimeKind.Unspecified).AddTicks(9455), "خالد", "سعد الدوسري" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "CreationDate", "FirstName", "LastName" },
                values: new object[] { new DateTime(2025, 11, 5, 11, 54, 20, 345, DateTimeKind.Unspecified).AddTicks(9501), "فهد", "عبدالرحمن القحطاني" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 5L,
                columns: new[] { "CreationDate", "FirstName", "LastName" },
                values: new object[] { new DateTime(2025, 11, 5, 11, 54, 20, 345, DateTimeKind.Unspecified).AddTicks(9504), "عبدالله", "يوسف العتيبي" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 6L,
                columns: new[] { "CreationDate", "FirstName", "LastName" },
                values: new object[] { new DateTime(2025, 11, 5, 11, 54, 20, 345, DateTimeKind.Unspecified).AddTicks(9505), "سعد", "علي الحربي" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 7L,
                columns: new[] { "CreationDate", "FirstName", "LastName" },
                values: new object[] { new DateTime(2025, 11, 5, 11, 54, 20, 345, DateTimeKind.Unspecified).AddTicks(9507), "عمر", "حسن الغامدي" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 8L,
                columns: new[] { "CreationDate", "FirstName", "LastName" },
                values: new object[] { new DateTime(2025, 11, 5, 11, 54, 20, 345, DateTimeKind.Unspecified).AddTicks(9509), "يوسف", "إبراهيم الزهراني" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 9L,
                columns: new[] { "CreationDate", "FirstName", "LastName" },
                values: new object[] { new DateTime(2025, 11, 5, 11, 54, 20, 345, DateTimeKind.Unspecified).AddTicks(9510), "علي", "محمود الشمري" });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 10L,
                columns: new[] { "CreationDate", "FirstName", "LastName" },
                values: new object[] { new DateTime(2025, 11, 5, 11, 54, 20, 345, DateTimeKind.Unspecified).AddTicks(9512), "حسن", "عبدالعزيز المطيري" });

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 346, DateTimeKind.Unspecified).AddTicks(925));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 346, DateTimeKind.Unspecified).AddTicks(935));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 346, DateTimeKind.Unspecified).AddTicks(937));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 346, DateTimeKind.Unspecified).AddTicks(938));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 346, DateTimeKind.Unspecified).AddTicks(939));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 346, DateTimeKind.Unspecified).AddTicks(2256));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 346, DateTimeKind.Unspecified).AddTicks(2264));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 346, DateTimeKind.Unspecified).AddTicks(2265));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 346, DateTimeKind.Unspecified).AddTicks(2267));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 346, DateTimeKind.Unspecified).AddTicks(2268));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 346, DateTimeKind.Unspecified).AddTicks(9460));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 346, DateTimeKind.Unspecified).AddTicks(9483));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 346, DateTimeKind.Unspecified).AddTicks(9485));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 346, DateTimeKind.Unspecified).AddTicks(9486));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 346, DateTimeKind.Unspecified).AddTicks(9487));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 347, DateTimeKind.Unspecified).AddTicks(941));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 347, DateTimeKind.Unspecified).AddTicks(951));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 347, DateTimeKind.Unspecified).AddTicks(952));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 347, DateTimeKind.Unspecified).AddTicks(953));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 347, DateTimeKind.Unspecified).AddTicks(955));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 347, DateTimeKind.Unspecified).AddTicks(2284));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 347, DateTimeKind.Unspecified).AddTicks(2293));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 347, DateTimeKind.Unspecified).AddTicks(2294));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 347, DateTimeKind.Unspecified).AddTicks(2295));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 347, DateTimeKind.Unspecified).AddTicks(2297));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 347, DateTimeKind.Unspecified).AddTicks(4859));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 347, DateTimeKind.Unspecified).AddTicks(4874));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 347, DateTimeKind.Unspecified).AddTicks(4875));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 347, DateTimeKind.Unspecified).AddTicks(4876));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 347, DateTimeKind.Unspecified).AddTicks(4878));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 347, DateTimeKind.Unspecified).AddTicks(6160));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 347, DateTimeKind.Unspecified).AddTicks(6169));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 347, DateTimeKind.Unspecified).AddTicks(6170));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 347, DateTimeKind.Unspecified).AddTicks(6171));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 347, DateTimeKind.Unspecified).AddTicks(6173));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 347, DateTimeKind.Unspecified).AddTicks(7447));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 347, DateTimeKind.Unspecified).AddTicks(7458));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 347, DateTimeKind.Unspecified).AddTicks(7459));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 347, DateTimeKind.Unspecified).AddTicks(7461));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 347, DateTimeKind.Unspecified).AddTicks(7462));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 347, DateTimeKind.Unspecified).AddTicks(8675));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 347, DateTimeKind.Unspecified).AddTicks(8685));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 347, DateTimeKind.Unspecified).AddTicks(8686));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 347, DateTimeKind.Unspecified).AddTicks(8688));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 347, DateTimeKind.Unspecified).AddTicks(8689));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 347, DateTimeKind.Unspecified).AddTicks(8690));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 347, DateTimeKind.Unspecified).AddTicks(8691));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 347, DateTimeKind.Unspecified).AddTicks(8693));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 347, DateTimeKind.Unspecified).AddTicks(8694));

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 347, DateTimeKind.Unspecified).AddTicks(8695));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 348, DateTimeKind.Unspecified).AddTicks(2167));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 348, DateTimeKind.Unspecified).AddTicks(2185));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 348, DateTimeKind.Unspecified).AddTicks(2187));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 348, DateTimeKind.Unspecified).AddTicks(2189));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 348, DateTimeKind.Unspecified).AddTicks(2190));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 348, DateTimeKind.Unspecified).AddTicks(2191));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 348, DateTimeKind.Unspecified).AddTicks(2193));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 348, DateTimeKind.Unspecified).AddTicks(2194));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 348, DateTimeKind.Unspecified).AddTicks(2195));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 348, DateTimeKind.Unspecified).AddTicks(2196));

            migrationBuilder.UpdateData(
                table: "RequestPurposes",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 348, DateTimeKind.Unspecified).AddTicks(2197));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 348, DateTimeKind.Unspecified).AddTicks(4475));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 348, DateTimeKind.Unspecified).AddTicks(4489));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 348, DateTimeKind.Unspecified).AddTicks(4491));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 348, DateTimeKind.Unspecified).AddTicks(4493));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 348, DateTimeKind.Unspecified).AddTicks(4494));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 348, DateTimeKind.Unspecified).AddTicks(6023));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 348, DateTimeKind.Unspecified).AddTicks(6034));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 348, DateTimeKind.Unspecified).AddTicks(6036));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 348, DateTimeKind.Unspecified).AddTicks(6037));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 11, 54, 20, 348, DateTimeKind.Unspecified).AddTicks(6038));
        }
    }
}
