using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class InitialWorkflowSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowApprovalHistory_WorkflowSteps_WorkflowStepId",
                table: "WorkflowApprovalHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkflowApprovalHistory",
                table: "WorkflowApprovalHistory");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Workflows");

            migrationBuilder.DropColumn(
                name: "DepartementId",
                table: "Workflows");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "Workflows");

            migrationBuilder.DropColumn(
                name: "RequesterType",
                table: "Workflows");

            migrationBuilder.DropColumn(
                name: "ApprovedDate",
                table: "WorkflowApprovalHistory");

            migrationBuilder.DropColumn(
                name: "ApproverEmployeeId",
                table: "WorkflowApprovalHistory");

            migrationBuilder.DropColumn(
                name: "IsDelagation",
                table: "WorkflowApprovalHistory");

            migrationBuilder.DropColumn(
                name: "RequestType",
                table: "WorkflowApprovalHistory");

            migrationBuilder.RenameTable(
                name: "WorkflowApprovalHistory",
                newName: "WorkflowApprovalHistories");

            migrationBuilder.RenameColumn(
                name: "TargetRequestId",
                table: "WorkflowApprovalHistories",
                newName: "OldRequestStatus");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "WorkflowApprovalHistories",
                newName: "NewRequestStatus");

            migrationBuilder.RenameIndex(
                name: "IX_WorkflowApprovalHistory_WorkflowStepId",
                table: "WorkflowApprovalHistories",
                newName: "IX_WorkflowApprovalHistories_WorkflowStepId");

            migrationBuilder.AlterColumn<bool>(
                name: "MustApprove",
                table: "WorkflowSteps",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationRoleId",
                table: "WorkflowSteps",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EntityId",
                table: "WorkflowSteps",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "HigherApprovalRoleId",
                table: "WorkflowSteps",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "RequireHigherApproval",
                table: "WorkflowSteps",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ReserveQty",
                table: "WorkflowSteps",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Workflows",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Workflows",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<bool>(
                name: "IsSpecialOrReserved",
                table: "Workflows",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Comments",
                table: "WorkflowApprovalHistories",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ChangedAt",
                table: "WorkflowApprovalHistories",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<string>(
                name: "ChangedBy",
                table: "WorkflowApprovalHistories",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkflowApprovalHistories",
                table: "WorkflowApprovalHistories",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "WorkflowApprovalSteps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkflowStepId = table.Column<int>(type: "int", nullable: false),
                    TargetRequestId = table.Column<int>(type: "int", nullable: false),
                    RequestType = table.Column<int>(type: "int", nullable: false),
                    ApproverEmployeeId = table.Column<int>(type: "int", nullable: true),
                    IsDelegation = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsCurrent = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowApprovalSteps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkflowApprovalSteps_WorkflowSteps_WorkflowStepId",
                        column: x => x.WorkflowStepId,
                        principalTable: "WorkflowSteps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 587, DateTimeKind.Unspecified).AddTicks(3890));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 587, DateTimeKind.Unspecified).AddTicks(3961));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 587, DateTimeKind.Unspecified).AddTicks(3964));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 587, DateTimeKind.Unspecified).AddTicks(3966));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 587, DateTimeKind.Unspecified).AddTicks(3967));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 587, DateTimeKind.Unspecified).AddTicks(3969));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 587, DateTimeKind.Unspecified).AddTicks(3970));

            migrationBuilder.UpdateData(
                table: "ApplicationEntity",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 587, DateTimeKind.Unspecified).AddTicks(3972));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 587, DateTimeKind.Unspecified).AddTicks(9029));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 587, DateTimeKind.Unspecified).AddTicks(9056));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 587, DateTimeKind.Unspecified).AddTicks(9058));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 587, DateTimeKind.Unspecified).AddTicks(9060));

            migrationBuilder.UpdateData(
                table: "CaseTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 587, DateTimeKind.Unspecified).AddTicks(9062));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 588, DateTimeKind.Unspecified).AddTicks(2325));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 588, DateTimeKind.Unspecified).AddTicks(2343));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 588, DateTimeKind.Unspecified).AddTicks(2346));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 588, DateTimeKind.Unspecified).AddTicks(2348));

            migrationBuilder.UpdateData(
                table: "Compatibilities",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 588, DateTimeKind.Unspecified).AddTicks(2350));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 588, DateTimeKind.Unspecified).AddTicks(5948));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 588, DateTimeKind.Unspecified).AddTicks(5969));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 588, DateTimeKind.Unspecified).AddTicks(5971));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 588, DateTimeKind.Unspecified).AddTicks(5973));

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 588, DateTimeKind.Unspecified).AddTicks(5974));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 588, DateTimeKind.Unspecified).AddTicks(7508));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 588, DateTimeKind.Unspecified).AddTicks(7524));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 588, DateTimeKind.Unspecified).AddTicks(7527));

            migrationBuilder.UpdateData(
                table: "Depots",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 588, DateTimeKind.Unspecified).AddTicks(7529));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 588, DateTimeKind.Unspecified).AddTicks(9061));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 588, DateTimeKind.Unspecified).AddTicks(9072));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 588, DateTimeKind.Unspecified).AddTicks(9075));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 588, DateTimeKind.Unspecified).AddTicks(9077));

            migrationBuilder.UpdateData(
                table: "HazardDivisions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 588, DateTimeKind.Unspecified).AddTicks(9078));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 589, DateTimeKind.Unspecified).AddTicks(594));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 589, DateTimeKind.Unspecified).AddTicks(606));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 589, DateTimeKind.Unspecified).AddTicks(608));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 589, DateTimeKind.Unspecified).AddTicks(609));

            migrationBuilder.UpdateData(
                table: "Hcc",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 589, DateTimeKind.Unspecified).AddTicks(611));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 589, DateTimeKind.Unspecified).AddTicks(8921));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 589, DateTimeKind.Unspecified).AddTicks(8946));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 589, DateTimeKind.Unspecified).AddTicks(8948));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 589, DateTimeKind.Unspecified).AddTicks(8950));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 589, DateTimeKind.Unspecified).AddTicks(8952));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 590, DateTimeKind.Unspecified).AddTicks(586));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 590, DateTimeKind.Unspecified).AddTicks(599));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 590, DateTimeKind.Unspecified).AddTicks(601));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 590, DateTimeKind.Unspecified).AddTicks(603));

            migrationBuilder.UpdateData(
                table: "NatureOptions",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 590, DateTimeKind.Unspecified).AddTicks(605));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 590, DateTimeKind.Unspecified).AddTicks(2173));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 590, DateTimeKind.Unspecified).AddTicks(2183));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 590, DateTimeKind.Unspecified).AddTicks(2186));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 590, DateTimeKind.Unspecified).AddTicks(2187));

            migrationBuilder.UpdateData(
                table: "Nsn",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 590, DateTimeKind.Unspecified).AddTicks(2189));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 590, DateTimeKind.Unspecified).AddTicks(3708));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 590, DateTimeKind.Unspecified).AddTicks(3719));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 590, DateTimeKind.Unspecified).AddTicks(3721));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 590, DateTimeKind.Unspecified).AddTicks(3723));

            migrationBuilder.UpdateData(
                table: "PrimaryPurposes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 590, DateTimeKind.Unspecified).AddTicks(3725));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 590, DateTimeKind.Unspecified).AddTicks(5246));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 590, DateTimeKind.Unspecified).AddTicks(5259));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 590, DateTimeKind.Unspecified).AddTicks(5261));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 590, DateTimeKind.Unspecified).AddTicks(5263));

            migrationBuilder.UpdateData(
                table: "ProjectailMaterials",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 590, DateTimeKind.Unspecified).AddTicks(5264));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 590, DateTimeKind.Unspecified).AddTicks(6770));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 590, DateTimeKind.Unspecified).AddTicks(6780));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 590, DateTimeKind.Unspecified).AddTicks(6782));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 590, DateTimeKind.Unspecified).AddTicks(6783));

            migrationBuilder.UpdateData(
                table: "Propellants",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 590, DateTimeKind.Unspecified).AddTicks(6785));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 590, DateTimeKind.Unspecified).AddTicks(8258));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 590, DateTimeKind.Unspecified).AddTicks(8268));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 590, DateTimeKind.Unspecified).AddTicks(8270));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 590, DateTimeKind.Unspecified).AddTicks(8272));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 590, DateTimeKind.Unspecified).AddTicks(8273));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 590, DateTimeKind.Unspecified).AddTicks(9735));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 590, DateTimeKind.Unspecified).AddTicks(9744));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 590, DateTimeKind.Unspecified).AddTicks(9746));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 590, DateTimeKind.Unspecified).AddTicks(9748));

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDate",
                value: new DateTime(2025, 11, 4, 9, 40, 7, 590, DateTimeKind.Unspecified).AddTicks(9749));

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowSteps_ApplicationRoleId",
                table: "WorkflowSteps",
                column: "ApplicationRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowSteps_HigherApprovalRoleId",
                table: "WorkflowSteps",
                column: "HigherApprovalRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowApprovalSteps_TargetRequestId_IsCurrent",
                table: "WorkflowApprovalSteps",
                columns: new[] { "TargetRequestId", "IsCurrent" });

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowApprovalSteps_WorkflowStepId",
                table: "WorkflowApprovalSteps",
                column: "WorkflowStepId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowApprovalHistories_WorkflowSteps_WorkflowStepId",
                table: "WorkflowApprovalHistories",
                column: "WorkflowStepId",
                principalTable: "WorkflowSteps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowSteps_AspNetRoles_ApplicationRoleId",
                table: "WorkflowSteps",
                column: "ApplicationRoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowSteps_AspNetRoles_HigherApprovalRoleId",
                table: "WorkflowSteps",
                column: "HigherApprovalRoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowApprovalHistories_WorkflowSteps_WorkflowStepId",
                table: "WorkflowApprovalHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowSteps_AspNetRoles_ApplicationRoleId",
                table: "WorkflowSteps");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowSteps_AspNetRoles_HigherApprovalRoleId",
                table: "WorkflowSteps");

            migrationBuilder.DropTable(
                name: "WorkflowApprovalSteps");

            migrationBuilder.DropIndex(
                name: "IX_WorkflowSteps_ApplicationRoleId",
                table: "WorkflowSteps");

            migrationBuilder.DropIndex(
                name: "IX_WorkflowSteps_HigherApprovalRoleId",
                table: "WorkflowSteps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkflowApprovalHistories",
                table: "WorkflowApprovalHistories");

            migrationBuilder.DropColumn(
                name: "ApplicationRoleId",
                table: "WorkflowSteps");

            migrationBuilder.DropColumn(
                name: "EntityId",
                table: "WorkflowSteps");

            migrationBuilder.DropColumn(
                name: "HigherApprovalRoleId",
                table: "WorkflowSteps");

            migrationBuilder.DropColumn(
                name: "RequireHigherApproval",
                table: "WorkflowSteps");

            migrationBuilder.DropColumn(
                name: "ReserveQty",
                table: "WorkflowSteps");

            migrationBuilder.DropColumn(
                name: "IsSpecialOrReserved",
                table: "Workflows");

            migrationBuilder.DropColumn(
                name: "ChangedAt",
                table: "WorkflowApprovalHistories");

            migrationBuilder.DropColumn(
                name: "ChangedBy",
                table: "WorkflowApprovalHistories");

            migrationBuilder.RenameTable(
                name: "WorkflowApprovalHistories",
                newName: "WorkflowApprovalHistory");

            migrationBuilder.RenameColumn(
                name: "OldRequestStatus",
                table: "WorkflowApprovalHistory",
                newName: "TargetRequestId");

            migrationBuilder.RenameColumn(
                name: "NewRequestStatus",
                table: "WorkflowApprovalHistory",
                newName: "Status");

            migrationBuilder.RenameIndex(
                name: "IX_WorkflowApprovalHistories_WorkflowStepId",
                table: "WorkflowApprovalHistory",
                newName: "IX_WorkflowApprovalHistory_WorkflowStepId");

            migrationBuilder.AlterColumn<bool>(
                name: "MustApprove",
                table: "WorkflowSteps",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Workflows",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Workflows",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Workflows",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DepartementId",
                table: "Workflows",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrganizationId",
                table: "Workflows",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RequesterType",
                table: "Workflows",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Comments",
                table: "WorkflowApprovalHistory",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedDate",
                table: "WorkflowApprovalHistory",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ApproverEmployeeId",
                table: "WorkflowApprovalHistory",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IsDelagation",
                table: "WorkflowApprovalHistory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RequestType",
                table: "WorkflowApprovalHistory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkflowApprovalHistory",
                table: "WorkflowApprovalHistory",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowApprovalHistory_WorkflowSteps_WorkflowStepId",
                table: "WorkflowApprovalHistory",
                column: "WorkflowStepId",
                principalTable: "WorkflowSteps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
