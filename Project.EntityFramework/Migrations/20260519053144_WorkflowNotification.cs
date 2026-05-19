using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class WorkflowNotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderAutoRejectPolicies_AspNetRoles_TriggerRoleId",
                table: "OrderAutoRejectPolicies");

            migrationBuilder.DropIndex(
                name: "IX_OrderAutoRejectPolicies_TriggerRoleId",
                table: "OrderAutoRejectPolicies");

            migrationBuilder.DropColumn(
                name: "TriggerRoleId",
                table: "OrderAutoRejectPolicies");

            migrationBuilder.CreateTable(
                name: "WorkflowStepRequesterQuantityNotifications",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkflowId = table.Column<long>(type: "bigint", nullable: false),
                    WorkflowStepId = table.Column<long>(type: "bigint", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowStepRequesterQuantityNotifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkflowStepRequesterQuantityNotifications_WorkflowSteps_WorkflowStepId",
                        column: x => x.WorkflowStepId,
                        principalTable: "WorkflowSteps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkflowStepRequesterQuantityNotifications_Workflows_WorkflowId",
                        column: x => x.WorkflowId,
                        principalTable: "Workflows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowStepRequesterQuantityNotifications_WorkflowId",
                table: "WorkflowStepRequesterQuantityNotifications",
                column: "WorkflowId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowStepRequesterQuantityNotifications_WorkflowStepId",
                table: "WorkflowStepRequesterQuantityNotifications",
                column: "WorkflowStepId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkflowStepRequesterQuantityNotifications");

            migrationBuilder.AddColumn<string>(
                name: "TriggerRoleId",
                table: "OrderAutoRejectPolicies",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_OrderAutoRejectPolicies_TriggerRoleId",
                table: "OrderAutoRejectPolicies",
                column: "TriggerRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderAutoRejectPolicies_AspNetRoles_TriggerRoleId",
                table: "OrderAutoRejectPolicies",
                column: "TriggerRoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
