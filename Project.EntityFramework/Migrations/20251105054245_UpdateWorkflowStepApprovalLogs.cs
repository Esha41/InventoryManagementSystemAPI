using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class UpdateWorkflowStepApprovalLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowApprovalHistories_WorkflowSteps_WorkflowStepId",
                table: "WorkflowApprovalHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkflowApprovalHistories",
                table: "WorkflowApprovalHistories");

            migrationBuilder.RenameTable(
                name: "WorkflowApprovalHistories",
                newName: "WorkflowStepApprovalLog");

            migrationBuilder.RenameIndex(
                name: "IX_WorkflowApprovalHistories_WorkflowStepId",
                table: "WorkflowStepApprovalLog",
                newName: "IX_WorkflowStepApprovalLog_WorkflowStepId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkflowStepApprovalLog",
                table: "WorkflowStepApprovalLog",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 372, DateTimeKind.Unspecified).AddTicks(3654));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 372, DateTimeKind.Unspecified).AddTicks(3723));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 372, DateTimeKind.Unspecified).AddTicks(3725));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 372, DateTimeKind.Unspecified).AddTicks(3726));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 372, DateTimeKind.Unspecified).AddTicks(3728));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 372, DateTimeKind.Unspecified).AddTicks(3729));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 372, DateTimeKind.Unspecified).AddTicks(3731));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 372, DateTimeKind.Unspecified).AddTicks(3733));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 372, DateTimeKind.Unspecified).AddTicks(8596));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 372, DateTimeKind.Unspecified).AddTicks(8619));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 372, DateTimeKind.Unspecified).AddTicks(8621));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 372, DateTimeKind.Unspecified).AddTicks(8623));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 372, DateTimeKind.Unspecified).AddTicks(8624));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 373, DateTimeKind.Unspecified).AddTicks(1822));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 373, DateTimeKind.Unspecified).AddTicks(1840));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 373, DateTimeKind.Unspecified).AddTicks(1841));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 373, DateTimeKind.Unspecified).AddTicks(1843));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 373, DateTimeKind.Unspecified).AddTicks(1845));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 373, DateTimeKind.Unspecified).AddTicks(5306));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 373, DateTimeKind.Unspecified).AddTicks(5324));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 373, DateTimeKind.Unspecified).AddTicks(5326));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 373, DateTimeKind.Unspecified).AddTicks(5328));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 373, DateTimeKind.Unspecified).AddTicks(5330));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 373, DateTimeKind.Unspecified).AddTicks(6895));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 373, DateTimeKind.Unspecified).AddTicks(6911));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 373, DateTimeKind.Unspecified).AddTicks(6913));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 373, DateTimeKind.Unspecified).AddTicks(6915));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 373, DateTimeKind.Unspecified).AddTicks(8448));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 373, DateTimeKind.Unspecified).AddTicks(8459));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 373, DateTimeKind.Unspecified).AddTicks(8461));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 373, DateTimeKind.Unspecified).AddTicks(8463));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 373, DateTimeKind.Unspecified).AddTicks(8464));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 373, DateTimeKind.Unspecified).AddTicks(9971));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 373, DateTimeKind.Unspecified).AddTicks(9981));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 373, DateTimeKind.Unspecified).AddTicks(9983));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 373, DateTimeKind.Unspecified).AddTicks(9985));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 373, DateTimeKind.Unspecified).AddTicks(9986));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 374, DateTimeKind.Unspecified).AddTicks(7245));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 374, DateTimeKind.Unspecified).AddTicks(7266));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 374, DateTimeKind.Unspecified).AddTicks(7268));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 374, DateTimeKind.Unspecified).AddTicks(7270));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 374, DateTimeKind.Unspecified).AddTicks(7271));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 374, DateTimeKind.Unspecified).AddTicks(8937));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 374, DateTimeKind.Unspecified).AddTicks(8949));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 374, DateTimeKind.Unspecified).AddTicks(8951));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 374, DateTimeKind.Unspecified).AddTicks(8953));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 374, DateTimeKind.Unspecified).AddTicks(8954));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 375, DateTimeKind.Unspecified).AddTicks(490));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 375, DateTimeKind.Unspecified).AddTicks(500));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 375, DateTimeKind.Unspecified).AddTicks(502));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 375, DateTimeKind.Unspecified).AddTicks(504));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 375, DateTimeKind.Unspecified).AddTicks(506));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 375, DateTimeKind.Unspecified).AddTicks(1923));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 375, DateTimeKind.Unspecified).AddTicks(1934));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 375, DateTimeKind.Unspecified).AddTicks(1936));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 375, DateTimeKind.Unspecified).AddTicks(1937));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 375, DateTimeKind.Unspecified).AddTicks(1939));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 375, DateTimeKind.Unspecified).AddTicks(3351));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 375, DateTimeKind.Unspecified).AddTicks(3360));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 375, DateTimeKind.Unspecified).AddTicks(3362));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 375, DateTimeKind.Unspecified).AddTicks(3364));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 375, DateTimeKind.Unspecified).AddTicks(3365));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 375, DateTimeKind.Unspecified).AddTicks(4751));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 375, DateTimeKind.Unspecified).AddTicks(4760));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 375, DateTimeKind.Unspecified).AddTicks(4762));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 375, DateTimeKind.Unspecified).AddTicks(4764));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 375, DateTimeKind.Unspecified).AddTicks(4765));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 375, DateTimeKind.Unspecified).AddTicks(6221));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 375, DateTimeKind.Unspecified).AddTicks(6230));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 375, DateTimeKind.Unspecified).AddTicks(6231));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 375, DateTimeKind.Unspecified).AddTicks(6233));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 375, DateTimeKind.Unspecified).AddTicks(6234));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 375, DateTimeKind.Unspecified).AddTicks(7593));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 375, DateTimeKind.Unspecified).AddTicks(7603));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 375, DateTimeKind.Unspecified).AddTicks(7605));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 375, DateTimeKind.Unspecified).AddTicks(7607));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 42, 45, 375, DateTimeKind.Unspecified).AddTicks(7608));

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowStepApprovalLog_WorkflowSteps_WorkflowStepId",
                table: "WorkflowStepApprovalLog",
                column: "WorkflowStepId",
                principalTable: "WorkflowSteps",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowStepApprovalLog_WorkflowSteps_WorkflowStepId",
                table: "WorkflowStepApprovalLog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkflowStepApprovalLog",
                table: "WorkflowStepApprovalLog");

            migrationBuilder.RenameTable(
                name: "WorkflowStepApprovalLog",
                newName: "WorkflowApprovalHistories");

            migrationBuilder.RenameIndex(
                name: "IX_WorkflowStepApprovalLog_WorkflowStepId",
                table: "WorkflowApprovalHistories",
                newName: "IX_WorkflowApprovalHistories_WorkflowStepId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkflowApprovalHistories",
                table: "WorkflowApprovalHistories",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 444, DateTimeKind.Unspecified).AddTicks(6101));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 444, DateTimeKind.Unspecified).AddTicks(6160));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 444, DateTimeKind.Unspecified).AddTicks(6162));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 444, DateTimeKind.Unspecified).AddTicks(6163));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 444, DateTimeKind.Unspecified).AddTicks(6164));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 444, DateTimeKind.Unspecified).AddTicks(6166));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 444, DateTimeKind.Unspecified).AddTicks(6167));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 444, DateTimeKind.Unspecified).AddTicks(6168));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 445, DateTimeKind.Unspecified).AddTicks(562));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 445, DateTimeKind.Unspecified).AddTicks(589));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 445, DateTimeKind.Unspecified).AddTicks(590));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 445, DateTimeKind.Unspecified).AddTicks(592));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 445, DateTimeKind.Unspecified).AddTicks(593));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 445, DateTimeKind.Unspecified).AddTicks(3574));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 445, DateTimeKind.Unspecified).AddTicks(3589));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 445, DateTimeKind.Unspecified).AddTicks(3590));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 445, DateTimeKind.Unspecified).AddTicks(3591));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 445, DateTimeKind.Unspecified).AddTicks(3593));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 445, DateTimeKind.Unspecified).AddTicks(6632));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 445, DateTimeKind.Unspecified).AddTicks(6648));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 445, DateTimeKind.Unspecified).AddTicks(6650));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 445, DateTimeKind.Unspecified).AddTicks(6651));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 445, DateTimeKind.Unspecified).AddTicks(6653));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 445, DateTimeKind.Unspecified).AddTicks(8092));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 445, DateTimeKind.Unspecified).AddTicks(8105));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 445, DateTimeKind.Unspecified).AddTicks(8107));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 445, DateTimeKind.Unspecified).AddTicks(8109));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 445, DateTimeKind.Unspecified).AddTicks(9436));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 445, DateTimeKind.Unspecified).AddTicks(9444));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 445, DateTimeKind.Unspecified).AddTicks(9445));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 445, DateTimeKind.Unspecified).AddTicks(9447));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 445, DateTimeKind.Unspecified).AddTicks(9448));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 446, DateTimeKind.Unspecified).AddTicks(746));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 446, DateTimeKind.Unspecified).AddTicks(752));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 446, DateTimeKind.Unspecified).AddTicks(754));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 446, DateTimeKind.Unspecified).AddTicks(755));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 446, DateTimeKind.Unspecified).AddTicks(756));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 446, DateTimeKind.Unspecified).AddTicks(7425));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 446, DateTimeKind.Unspecified).AddTicks(7446));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 446, DateTimeKind.Unspecified).AddTicks(7448));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 446, DateTimeKind.Unspecified).AddTicks(7449));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 446, DateTimeKind.Unspecified).AddTicks(7450));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 446, DateTimeKind.Unspecified).AddTicks(8891));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 446, DateTimeKind.Unspecified).AddTicks(8902));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 446, DateTimeKind.Unspecified).AddTicks(8903));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 446, DateTimeKind.Unspecified).AddTicks(8904));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 446, DateTimeKind.Unspecified).AddTicks(8906));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 447, DateTimeKind.Unspecified).AddTicks(223));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 447, DateTimeKind.Unspecified).AddTicks(230));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 447, DateTimeKind.Unspecified).AddTicks(231));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 447, DateTimeKind.Unspecified).AddTicks(233));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 447, DateTimeKind.Unspecified).AddTicks(234));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 447, DateTimeKind.Unspecified).AddTicks(1562));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 447, DateTimeKind.Unspecified).AddTicks(1570));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 447, DateTimeKind.Unspecified).AddTicks(1571));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 447, DateTimeKind.Unspecified).AddTicks(1573));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 447, DateTimeKind.Unspecified).AddTicks(1574));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 447, DateTimeKind.Unspecified).AddTicks(2864));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 447, DateTimeKind.Unspecified).AddTicks(2873));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 447, DateTimeKind.Unspecified).AddTicks(2874));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 447, DateTimeKind.Unspecified).AddTicks(2875));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 447, DateTimeKind.Unspecified).AddTicks(2876));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 447, DateTimeKind.Unspecified).AddTicks(4208));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 447, DateTimeKind.Unspecified).AddTicks(4216));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 447, DateTimeKind.Unspecified).AddTicks(4217));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 447, DateTimeKind.Unspecified).AddTicks(4218));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 447, DateTimeKind.Unspecified).AddTicks(4219));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 447, DateTimeKind.Unspecified).AddTicks(5494));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 447, DateTimeKind.Unspecified).AddTicks(5501));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 447, DateTimeKind.Unspecified).AddTicks(5502));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 447, DateTimeKind.Unspecified).AddTicks(5543));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 447, DateTimeKind.Unspecified).AddTicks(5545));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 447, DateTimeKind.Unspecified).AddTicks(6831));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 447, DateTimeKind.Unspecified).AddTicks(6838));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 447, DateTimeKind.Unspecified).AddTicks(6839));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 447, DateTimeKind.Unspecified).AddTicks(6840));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 5, 8, 37, 52, 447, DateTimeKind.Unspecified).AddTicks(6842));

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowApprovalHistories_WorkflowSteps_WorkflowStepId",
                table: "WorkflowApprovalHistories",
                column: "WorkflowStepId",
                principalTable: "WorkflowSteps",
                principalColumn: "Id");
        }
    }
}
