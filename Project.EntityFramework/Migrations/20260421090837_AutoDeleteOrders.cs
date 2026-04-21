using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class AutoDeleteOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_ReportStatuses_ReportStatusId",
                table: "Reports");

            migrationBuilder.DropTable(
                name: "ReportStatuses");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationWarningSentAt",
                table: "WorkflowApprovalSteps",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ReportStatusId",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldDefaultValue: 1L);

            migrationBuilder.AddColumn<string>(
                name: "RequestPurposeNotes",
                table: "BaseRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "CriticalQuantity",
                table: "BaseItems",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OrderAutoRejectPolicies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TriggerRoleId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    ThresholdDays = table.Column<int>(type: "int", nullable: false),
                    ScanCron = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    NotifyRequester = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderAutoRejectPolicies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderAutoRejectPolicies_AspNetRoles_TriggerRoleId",
                        column: x => x.TriggerRoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowApprovalStepReminders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkflowApprovalStepId = table.Column<int>(type: "int", nullable: false),
                    LeadDays = table.Column<int>(type: "int", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowApprovalStepReminders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkflowApprovalStepReminders_WorkflowApprovalSteps_WorkflowApprovalStepId",
                        column: x => x.WorkflowApprovalStepId,
                        principalTable: "WorkflowApprovalSteps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderAutoRejectPolicyNotifyRoles",
                columns: table => new
                {
                    PolicyId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderAutoRejectPolicyNotifyRoles", x => new { x.PolicyId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_OrderAutoRejectPolicyNotifyRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderAutoRejectPolicyNotifyRoles_OrderAutoRejectPolicies_PolicyId",
                        column: x => x.PolicyId,
                        principalTable: "OrderAutoRejectPolicies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderAutoRejectPolicyReminderDays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PolicyId = table.Column<int>(type: "int", nullable: false),
                    LeadDays = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderAutoRejectPolicyReminderDays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderAutoRejectPolicyReminderDays_OrderAutoRejectPolicies_PolicyId",
                        column: x => x.PolicyId,
                        principalTable: "OrderAutoRejectPolicies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderAutoRejectPolicies_TriggerRoleId",
                table: "OrderAutoRejectPolicies",
                column: "TriggerRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderAutoRejectPolicyNotifyRoles_RoleId",
                table: "OrderAutoRejectPolicyNotifyRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderAutoRejectPolicyReminderDays_PolicyId_LeadDays",
                table: "OrderAutoRejectPolicyReminderDays",
                columns: new[] { "PolicyId", "LeadDays" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowApprovalStepReminders_WorkflowApprovalStepId_LeadDays",
                table: "WorkflowApprovalStepReminders",
                columns: new[] { "WorkflowApprovalStepId", "LeadDays" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderAutoRejectPolicyNotifyRoles");

            migrationBuilder.DropTable(
                name: "OrderAutoRejectPolicyReminderDays");

            migrationBuilder.DropTable(
                name: "WorkflowApprovalStepReminders");

            migrationBuilder.DropTable(
                name: "OrderAutoRejectPolicies");

            migrationBuilder.DropColumn(
                name: "ExpirationWarningSentAt",
                table: "WorkflowApprovalSteps");

            migrationBuilder.DropColumn(
                name: "RequestPurposeNotes",
                table: "BaseRequests");

            migrationBuilder.DropColumn(
                name: "CriticalQuantity",
                table: "BaseItems");

            migrationBuilder.AlterColumn<long>(
                name: "ReportStatusId",
                table: "Reports",
                type: "bigint",
                nullable: false,
                defaultValue: 1L,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1);

            migrationBuilder.CreateTable(
                name: "ReportStatuses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportStatuses", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ReportStatuses",
                columns: new[] { "Id", "CreatedBy", "IsDeleted", "ModificationDate", "ModifiedBy", "NameAr", "NameEn" },
                values: new object[,]
                {
                    { 1L, null, false, null, null, "مسودة", "Draft" },
                    { 2L, null, false, null, null, "منشور", "Published" },
                    { 3L, null, false, null, null, "غير نشط", "Inactive" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReportStatuses_NameAr",
                table: "ReportStatuses",
                column: "NameAr",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_ReportStatuses_NameEn",
                table: "ReportStatuses",
                column: "NameEn",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_ReportStatuses_ReportStatusId",
                table: "Reports",
                column: "ReportStatusId",
                principalTable: "ReportStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
