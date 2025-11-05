using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class UpdateWorkflowStepForRoleAndEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApproverEmployeeId",
                table: "WorkflowSteps");

            migrationBuilder.DropColumn(
                name: "ApproverType",
                table: "WorkflowSteps");

            migrationBuilder.RenameColumn(
                name: "EntityId",
                table: "WorkflowSteps",
                newName: "ApplicationEntityId");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationRoleId",
                table: "WorkflowSteps",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 847, DateTimeKind.Unspecified).AddTicks(3517));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 847, DateTimeKind.Unspecified).AddTicks(3581));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 847, DateTimeKind.Unspecified).AddTicks(3583));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 847, DateTimeKind.Unspecified).AddTicks(3584));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 847, DateTimeKind.Unspecified).AddTicks(3586));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 847, DateTimeKind.Unspecified).AddTicks(3587));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 847, DateTimeKind.Unspecified).AddTicks(3588));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 847, DateTimeKind.Unspecified).AddTicks(3590));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 847, DateTimeKind.Unspecified).AddTicks(8354));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 847, DateTimeKind.Unspecified).AddTicks(8377));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 847, DateTimeKind.Unspecified).AddTicks(8379));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 847, DateTimeKind.Unspecified).AddTicks(8380));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 847, DateTimeKind.Unspecified).AddTicks(8382));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 848, DateTimeKind.Unspecified).AddTicks(1572));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 848, DateTimeKind.Unspecified).AddTicks(1591));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 848, DateTimeKind.Unspecified).AddTicks(1593));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 848, DateTimeKind.Unspecified).AddTicks(1594));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 848, DateTimeKind.Unspecified).AddTicks(1595));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 848, DateTimeKind.Unspecified).AddTicks(4837));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 848, DateTimeKind.Unspecified).AddTicks(4855));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 848, DateTimeKind.Unspecified).AddTicks(4882));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 848, DateTimeKind.Unspecified).AddTicks(4885));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 848, DateTimeKind.Unspecified).AddTicks(4887));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 848, DateTimeKind.Unspecified).AddTicks(6274));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 848, DateTimeKind.Unspecified).AddTicks(6289));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 848, DateTimeKind.Unspecified).AddTicks(6291));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 848, DateTimeKind.Unspecified).AddTicks(6293));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 848, DateTimeKind.Unspecified).AddTicks(7722));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 848, DateTimeKind.Unspecified).AddTicks(7733));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 848, DateTimeKind.Unspecified).AddTicks(7735));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 848, DateTimeKind.Unspecified).AddTicks(7736));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 848, DateTimeKind.Unspecified).AddTicks(7738));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 848, DateTimeKind.Unspecified).AddTicks(9149));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 848, DateTimeKind.Unspecified).AddTicks(9158));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 848, DateTimeKind.Unspecified).AddTicks(9160));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 848, DateTimeKind.Unspecified).AddTicks(9161));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 848, DateTimeKind.Unspecified).AddTicks(9163));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 849, DateTimeKind.Unspecified).AddTicks(6094));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 849, DateTimeKind.Unspecified).AddTicks(6117));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 849, DateTimeKind.Unspecified).AddTicks(6119));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 849, DateTimeKind.Unspecified).AddTicks(6120));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 849, DateTimeKind.Unspecified).AddTicks(6122));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 849, DateTimeKind.Unspecified).AddTicks(7593));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 849, DateTimeKind.Unspecified).AddTicks(7604));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 849, DateTimeKind.Unspecified).AddTicks(7606));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 849, DateTimeKind.Unspecified).AddTicks(7608));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 849, DateTimeKind.Unspecified).AddTicks(7609));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 849, DateTimeKind.Unspecified).AddTicks(9121));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 849, DateTimeKind.Unspecified).AddTicks(9131));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 849, DateTimeKind.Unspecified).AddTicks(9133));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 849, DateTimeKind.Unspecified).AddTicks(9135));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 849, DateTimeKind.Unspecified).AddTicks(9136));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 850, DateTimeKind.Unspecified).AddTicks(740));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 850, DateTimeKind.Unspecified).AddTicks(752));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 850, DateTimeKind.Unspecified).AddTicks(753));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 850, DateTimeKind.Unspecified).AddTicks(755));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 850, DateTimeKind.Unspecified).AddTicks(756));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 850, DateTimeKind.Unspecified).AddTicks(2297));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 850, DateTimeKind.Unspecified).AddTicks(2306));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 850, DateTimeKind.Unspecified).AddTicks(2308));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 850, DateTimeKind.Unspecified).AddTicks(2309));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 850, DateTimeKind.Unspecified).AddTicks(2311));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 850, DateTimeKind.Unspecified).AddTicks(3673));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 850, DateTimeKind.Unspecified).AddTicks(3682));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 850, DateTimeKind.Unspecified).AddTicks(3684));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 850, DateTimeKind.Unspecified).AddTicks(3685));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 850, DateTimeKind.Unspecified).AddTicks(3686));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 850, DateTimeKind.Unspecified).AddTicks(5090));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 850, DateTimeKind.Unspecified).AddTicks(5100));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 850, DateTimeKind.Unspecified).AddTicks(5101));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 850, DateTimeKind.Unspecified).AddTicks(5103));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 850, DateTimeKind.Unspecified).AddTicks(5104));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 850, DateTimeKind.Unspecified).AddTicks(6520));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 850, DateTimeKind.Unspecified).AddTicks(6530));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 850, DateTimeKind.Unspecified).AddTicks(6531));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 850, DateTimeKind.Unspecified).AddTicks(6533));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 11, 24, 30, 850, DateTimeKind.Unspecified).AddTicks(6535));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ApplicationEntityId",
                table: "WorkflowSteps",
                newName: "EntityId");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationRoleId",
                table: "WorkflowSteps",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "ApproverEmployeeId",
                table: "WorkflowSteps",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ApproverType",
                table: "WorkflowSteps",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 669, DateTimeKind.Unspecified).AddTicks(7697));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 669, DateTimeKind.Unspecified).AddTicks(7765));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 669, DateTimeKind.Unspecified).AddTicks(7766));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 669, DateTimeKind.Unspecified).AddTicks(7768));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 669, DateTimeKind.Unspecified).AddTicks(7769));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 669, DateTimeKind.Unspecified).AddTicks(7771));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 669, DateTimeKind.Unspecified).AddTicks(7772));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 669, DateTimeKind.Unspecified).AddTicks(7773));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 670, DateTimeKind.Unspecified).AddTicks(2508));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 670, DateTimeKind.Unspecified).AddTicks(2532));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 670, DateTimeKind.Unspecified).AddTicks(2533));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 670, DateTimeKind.Unspecified).AddTicks(2535));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 670, DateTimeKind.Unspecified).AddTicks(2536));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 670, DateTimeKind.Unspecified).AddTicks(5307));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 670, DateTimeKind.Unspecified).AddTicks(5321));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 670, DateTimeKind.Unspecified).AddTicks(5323));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 670, DateTimeKind.Unspecified).AddTicks(5325));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 670, DateTimeKind.Unspecified).AddTicks(5326));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 670, DateTimeKind.Unspecified).AddTicks(8415));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 670, DateTimeKind.Unspecified).AddTicks(8431));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 670, DateTimeKind.Unspecified).AddTicks(8433));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 670, DateTimeKind.Unspecified).AddTicks(8434));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 670, DateTimeKind.Unspecified).AddTicks(8436));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 670, DateTimeKind.Unspecified).AddTicks(9832));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 670, DateTimeKind.Unspecified).AddTicks(9844));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 670, DateTimeKind.Unspecified).AddTicks(9847));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 670, DateTimeKind.Unspecified).AddTicks(9849));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 671, DateTimeKind.Unspecified).AddTicks(1156));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 671, DateTimeKind.Unspecified).AddTicks(1165));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 671, DateTimeKind.Unspecified).AddTicks(1166));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 671, DateTimeKind.Unspecified).AddTicks(1168));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 671, DateTimeKind.Unspecified).AddTicks(1169));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 671, DateTimeKind.Unspecified).AddTicks(2528));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 671, DateTimeKind.Unspecified).AddTicks(2537));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 671, DateTimeKind.Unspecified).AddTicks(2538));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 671, DateTimeKind.Unspecified).AddTicks(2539));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 671, DateTimeKind.Unspecified).AddTicks(2541));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 671, DateTimeKind.Unspecified).AddTicks(9566));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 671, DateTimeKind.Unspecified).AddTicks(9586));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 671, DateTimeKind.Unspecified).AddTicks(9587));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 671, DateTimeKind.Unspecified).AddTicks(9589));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 671, DateTimeKind.Unspecified).AddTicks(9590));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 672, DateTimeKind.Unspecified).AddTicks(995));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 672, DateTimeKind.Unspecified).AddTicks(1005));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 672, DateTimeKind.Unspecified).AddTicks(1006));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 672, DateTimeKind.Unspecified).AddTicks(1008));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 672, DateTimeKind.Unspecified).AddTicks(1009));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 672, DateTimeKind.Unspecified).AddTicks(2317));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 672, DateTimeKind.Unspecified).AddTicks(2326));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 672, DateTimeKind.Unspecified).AddTicks(2328));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 672, DateTimeKind.Unspecified).AddTicks(2329));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 672, DateTimeKind.Unspecified).AddTicks(2331));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 672, DateTimeKind.Unspecified).AddTicks(3715));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 672, DateTimeKind.Unspecified).AddTicks(3724));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 672, DateTimeKind.Unspecified).AddTicks(3726));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 672, DateTimeKind.Unspecified).AddTicks(3727));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 672, DateTimeKind.Unspecified).AddTicks(3729));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 672, DateTimeKind.Unspecified).AddTicks(5065));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 672, DateTimeKind.Unspecified).AddTicks(5076));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 672, DateTimeKind.Unspecified).AddTicks(5078));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 672, DateTimeKind.Unspecified).AddTicks(5079));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 672, DateTimeKind.Unspecified).AddTicks(5081));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 672, DateTimeKind.Unspecified).AddTicks(6366));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 672, DateTimeKind.Unspecified).AddTicks(6374));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 672, DateTimeKind.Unspecified).AddTicks(6375));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 672, DateTimeKind.Unspecified).AddTicks(6377));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 672, DateTimeKind.Unspecified).AddTicks(6378));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 672, DateTimeKind.Unspecified).AddTicks(7762));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 672, DateTimeKind.Unspecified).AddTicks(7770));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 672, DateTimeKind.Unspecified).AddTicks(7772));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 672, DateTimeKind.Unspecified).AddTicks(7773));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 672, DateTimeKind.Unspecified).AddTicks(7774));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 672, DateTimeKind.Unspecified).AddTicks(9326));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 672, DateTimeKind.Unspecified).AddTicks(9337));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 672, DateTimeKind.Unspecified).AddTicks(9338));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 672, DateTimeKind.Unspecified).AddTicks(9340));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 10, 46, 52, 672, DateTimeKind.Unspecified).AddTicks(9341));
        }
    }
}
