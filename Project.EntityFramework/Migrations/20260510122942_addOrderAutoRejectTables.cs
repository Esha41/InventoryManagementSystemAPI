using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class addOrderAutoRejectTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkflowAutoRejectTriggers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkflowId = table.Column<long>(type: "bigint", nullable: false),
                    Mode = table.Column<int>(type: "int", nullable: false),
                    ResetOnReApproval = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowAutoRejectTriggers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkflowAutoRejectTriggers_Workflows_WorkflowId",
                        column: x => x.WorkflowId,
                        principalTable: "Workflows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowAutoRejectTriggerRoles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkflowAutoRejectTriggerId = table.Column<long>(type: "bigint", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowAutoRejectTriggerRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkflowAutoRejectTriggerRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkflowAutoRejectTriggerRoles_WorkflowAutoRejectTriggers_WorkflowAutoRejectTriggerId",
                        column: x => x.WorkflowAutoRejectTriggerId,
                        principalTable: "WorkflowAutoRejectTriggers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowAutoRejectTriggerSteps",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkflowAutoRejectTriggerId = table.Column<long>(type: "bigint", nullable: false),
                    WorkflowStepId = table.Column<long>(type: "bigint", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowAutoRejectTriggerSteps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkflowAutoRejectTriggerSteps_WorkflowAutoRejectTriggers_WorkflowAutoRejectTriggerId",
                        column: x => x.WorkflowAutoRejectTriggerId,
                        principalTable: "WorkflowAutoRejectTriggers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkflowAutoRejectTriggerSteps_WorkflowSteps_WorkflowStepId",
                        column: x => x.WorkflowStepId,
                        principalTable: "WorkflowSteps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowAutoRejectTriggerRoles_RoleId",
                table: "WorkflowAutoRejectTriggerRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowAutoRejectTriggerRoles_WorkflowAutoRejectTriggerId_RoleId",
                table: "WorkflowAutoRejectTriggerRoles",
                columns: new[] { "WorkflowAutoRejectTriggerId", "RoleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowAutoRejectTriggers_WorkflowId",
                table: "WorkflowAutoRejectTriggers",
                column: "WorkflowId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowAutoRejectTriggerSteps_WorkflowAutoRejectTriggerId_WorkflowStepId",
                table: "WorkflowAutoRejectTriggerSteps",
                columns: new[] { "WorkflowAutoRejectTriggerId", "WorkflowStepId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowAutoRejectTriggerSteps_WorkflowStepId",
                table: "WorkflowAutoRejectTriggerSteps",
                column: "WorkflowStepId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkflowAutoRejectTriggerRoles");

            migrationBuilder.DropTable(
                name: "WorkflowAutoRejectTriggerSteps");

            migrationBuilder.DropTable(
                name: "WorkflowAutoRejectTriggers");
        }
    }
}
