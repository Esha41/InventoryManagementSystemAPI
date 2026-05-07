using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class RefactorWorkflowTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // SQL Server requires dropping FKs (dependent columns) and PKs (identity Id) before widening.
            DropWorkflowRefactorForeignKeys(migrationBuilder);
            DropWorkflowRefactorPrimaryKeys(migrationBuilder);

            migrationBuilder.AlterColumn<long>(
                name: "TargetWorkflowStepId",
                table: "WorkflowStepTransitions",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "SourceWorkflowStepId",
                table: "WorkflowStepTransitions",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "WorkflowStepTransitions",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<long>(
                name: "WorkflowId",
                table: "WorkflowSteps",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "WorkflowSteps",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<long>(
                name: "WorkflowStepId",
                table: "WorkflowStepParallelRoles",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "WorkflowStepParallelRoles",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<long>(
                name: "WorkflowStepId",
                table: "WorkflowStepNotifiers",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "WorkflowStepNotifiers",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<long>(
                name: "WorkflowStepId",
                table: "WorkflowStepApprovalLog",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "WorkflowApprovalStepId",
                table: "WorkflowStepApprovalLog",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "WorkflowStepApprovalLog",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "Workflows",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<long>(
                name: "WorkflowStepId",
                table: "WorkflowApprovalSteps",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "ReturnToStepId",
                table: "WorkflowApprovalSteps",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "WorkflowApprovalSteps",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<long>(
                name: "WorkflowApprovalStepId",
                table: "WorkflowApprovalStepReminders",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "WorkflowApprovalStepReminders",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<long>(
                name: "WorkflowStepId",
                table: "OrderItemHistory",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "WorkflowApprovalStepId",
                table: "OrderItemHistory",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            AddWorkflowRefactorPrimaryKeys(migrationBuilder);
            AddWorkflowRefactorForeignKeys(migrationBuilder);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            DropWorkflowRefactorForeignKeys(migrationBuilder);
            DropWorkflowRefactorPrimaryKeys(migrationBuilder);

            migrationBuilder.AlterColumn<int>(
                name: "TargetWorkflowStepId",
                table: "WorkflowStepTransitions",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "SourceWorkflowStepId",
                table: "WorkflowStepTransitions",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "WorkflowStepTransitions",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "WorkflowId",
                table: "WorkflowSteps",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "WorkflowSteps",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "WorkflowStepId",
                table: "WorkflowStepParallelRoles",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "WorkflowStepParallelRoles",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "WorkflowStepId",
                table: "WorkflowStepNotifiers",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "WorkflowStepNotifiers",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "WorkflowStepId",
                table: "WorkflowStepApprovalLog",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "WorkflowApprovalStepId",
                table: "WorkflowStepApprovalLog",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "WorkflowStepApprovalLog",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Workflows",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "WorkflowStepId",
                table: "WorkflowApprovalSteps",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "ReturnToStepId",
                table: "WorkflowApprovalSteps",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "WorkflowApprovalSteps",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "WorkflowApprovalStepId",
                table: "WorkflowApprovalStepReminders",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "WorkflowApprovalStepReminders",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "WorkflowStepId",
                table: "OrderItemHistory",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "WorkflowApprovalStepId",
                table: "OrderItemHistory",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            AddWorkflowRefactorPrimaryKeys(migrationBuilder);
            AddWorkflowRefactorForeignKeys(migrationBuilder);
        }

        private static void DropWorkflowRefactorForeignKeys(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowStepTransitions_WorkflowSteps_TargetWorkflowStepId",
                table: "WorkflowStepTransitions");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowStepTransitions_WorkflowSteps_SourceWorkflowStepId",
                table: "WorkflowStepTransitions");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowStepParallelRoles_WorkflowSteps_WorkflowStepId",
                table: "WorkflowStepParallelRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowStepNotifiers_WorkflowSteps_WorkflowStepId",
                table: "WorkflowStepNotifiers");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowApprovalSteps_WorkflowSteps_WorkflowStepId",
                table: "WorkflowApprovalSteps");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowStepApprovalLog_WorkflowSteps_WorkflowStepId",
                table: "WorkflowStepApprovalLog");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItemHistory_WorkflowSteps_WorkflowStepId",
                table: "OrderItemHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItemHistory_WorkflowApprovalSteps_WorkflowApprovalStepId",
                table: "OrderItemHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowApprovalStepReminders_WorkflowApprovalSteps_WorkflowApprovalStepId",
                table: "WorkflowApprovalStepReminders");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowSteps_Workflows_WorkflowId",
                table: "WorkflowSteps");
        }

        private static void DropWorkflowRefactorPrimaryKeys(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkflowStepTransitions",
                table: "WorkflowStepTransitions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkflowStepParallelRoles",
                table: "WorkflowStepParallelRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkflowStepNotifiers",
                table: "WorkflowStepNotifiers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkflowStepApprovalLog",
                table: "WorkflowStepApprovalLog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkflowApprovalStepReminders",
                table: "WorkflowApprovalStepReminders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkflowApprovalSteps",
                table: "WorkflowApprovalSteps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkflowSteps",
                table: "WorkflowSteps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Workflows",
                table: "Workflows");
        }

        private static void AddWorkflowRefactorPrimaryKeys(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddPrimaryKey(
                name: "PK_Workflows",
                table: "Workflows",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkflowSteps",
                table: "WorkflowSteps",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkflowApprovalSteps",
                table: "WorkflowApprovalSteps",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkflowStepTransitions",
                table: "WorkflowStepTransitions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkflowStepParallelRoles",
                table: "WorkflowStepParallelRoles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkflowStepNotifiers",
                table: "WorkflowStepNotifiers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkflowStepApprovalLog",
                table: "WorkflowStepApprovalLog",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkflowApprovalStepReminders",
                table: "WorkflowApprovalStepReminders",
                column: "Id");
        }

        private static void AddWorkflowRefactorForeignKeys(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowSteps_Workflows_WorkflowId",
                table: "WorkflowSteps",
                column: "WorkflowId",
                principalTable: "Workflows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowStepTransitions_WorkflowSteps_SourceWorkflowStepId",
                table: "WorkflowStepTransitions",
                column: "SourceWorkflowStepId",
                principalTable: "WorkflowSteps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowStepTransitions_WorkflowSteps_TargetWorkflowStepId",
                table: "WorkflowStepTransitions",
                column: "TargetWorkflowStepId",
                principalTable: "WorkflowSteps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowStepParallelRoles_WorkflowSteps_WorkflowStepId",
                table: "WorkflowStepParallelRoles",
                column: "WorkflowStepId",
                principalTable: "WorkflowSteps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowStepNotifiers_WorkflowSteps_WorkflowStepId",
                table: "WorkflowStepNotifiers",
                column: "WorkflowStepId",
                principalTable: "WorkflowSteps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowApprovalSteps_WorkflowSteps_WorkflowStepId",
                table: "WorkflowApprovalSteps",
                column: "WorkflowStepId",
                principalTable: "WorkflowSteps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowStepApprovalLog_WorkflowSteps_WorkflowStepId",
                table: "WorkflowStepApprovalLog",
                column: "WorkflowStepId",
                principalTable: "WorkflowSteps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowApprovalStepReminders_WorkflowApprovalSteps_WorkflowApprovalStepId",
                table: "WorkflowApprovalStepReminders",
                column: "WorkflowApprovalStepId",
                principalTable: "WorkflowApprovalSteps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItemHistory_WorkflowApprovalSteps_WorkflowApprovalStepId",
                table: "OrderItemHistory",
                column: "WorkflowApprovalStepId",
                principalTable: "WorkflowApprovalSteps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItemHistory_WorkflowSteps_WorkflowStepId",
                table: "OrderItemHistory",
                column: "WorkflowStepId",
                principalTable: "WorkflowSteps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
