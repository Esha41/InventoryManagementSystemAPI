using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class UpdateReceiverInfoToEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssetAssignments_Ranks_ReceiverRankId",
                table: "AssetAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_AssetSupplies_Ranks_ReceiverRankId",
                table: "AssetSupplies");

            migrationBuilder.DropForeignKey(
                name: "FK_Supplies_Ranks_ReceiverRankId",
                table: "Supplies");

            migrationBuilder.DropColumn(
                name: "RecieverMilitaryId",
                table: "Supplies");

            migrationBuilder.DropColumn(
                name: "RecieverName",
                table: "Supplies");

            migrationBuilder.DropColumn(
                name: "ReceiverMilitaryId",
                table: "AssetSupplies");

            migrationBuilder.DropColumn(
                name: "ReceiverName",
                table: "AssetSupplies");

            migrationBuilder.DropColumn(
                name: "ReceiverMilitaryId",
                table: "AssetAssignments");

            migrationBuilder.DropColumn(
                name: "ReceiverName",
                table: "AssetAssignments");

            migrationBuilder.RenameColumn(
                name: "ReceiverRankId",
                table: "Supplies",
                newName: "ReceiverEmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Supplies_ReceiverRankId",
                table: "Supplies",
                newName: "IX_Supplies_ReceiverEmployeeId");

            migrationBuilder.RenameColumn(
                name: "ReceiverRankId",
                table: "AssetSupplies",
                newName: "ReceiverEmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_AssetSupplies_ReceiverRankId",
                table: "AssetSupplies",
                newName: "IX_AssetSupplies_ReceiverEmployeeId");

            migrationBuilder.RenameColumn(
                name: "ReceiverRankId",
                table: "AssetAssignments",
                newName: "ReceiverEmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_AssetAssignments_ReceiverRankId",
                table: "AssetAssignments",
                newName: "IX_AssetAssignments_ReceiverEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssetAssignments_Employees_ReceiverEmployeeId",
                table: "AssetAssignments",
                column: "ReceiverEmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetSupplies_Employees_ReceiverEmployeeId",
                table: "AssetSupplies",
                column: "ReceiverEmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Supplies_Employees_ReceiverEmployeeId",
                table: "Supplies",
                column: "ReceiverEmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssetAssignments_Employees_ReceiverEmployeeId",
                table: "AssetAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_AssetSupplies_Employees_ReceiverEmployeeId",
                table: "AssetSupplies");

            migrationBuilder.DropForeignKey(
                name: "FK_Supplies_Employees_ReceiverEmployeeId",
                table: "Supplies");

            migrationBuilder.RenameColumn(
                name: "ReceiverEmployeeId",
                table: "Supplies",
                newName: "ReceiverRankId");

            migrationBuilder.RenameIndex(
                name: "IX_Supplies_ReceiverEmployeeId",
                table: "Supplies",
                newName: "IX_Supplies_ReceiverRankId");

            migrationBuilder.RenameColumn(
                name: "ReceiverEmployeeId",
                table: "AssetSupplies",
                newName: "ReceiverRankId");

            migrationBuilder.RenameIndex(
                name: "IX_AssetSupplies_ReceiverEmployeeId",
                table: "AssetSupplies",
                newName: "IX_AssetSupplies_ReceiverRankId");

            migrationBuilder.RenameColumn(
                name: "ReceiverEmployeeId",
                table: "AssetAssignments",
                newName: "ReceiverRankId");

            migrationBuilder.RenameIndex(
                name: "IX_AssetAssignments_ReceiverEmployeeId",
                table: "AssetAssignments",
                newName: "IX_AssetAssignments_ReceiverRankId");

            migrationBuilder.AddColumn<string>(
                name: "RecieverMilitaryId",
                table: "Supplies",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecieverName",
                table: "Supplies",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReceiverMilitaryId",
                table: "AssetSupplies",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReceiverName",
                table: "AssetSupplies",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReceiverMilitaryId",
                table: "AssetAssignments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReceiverName",
                table: "AssetAssignments",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetAssignments_Ranks_ReceiverRankId",
                table: "AssetAssignments",
                column: "ReceiverRankId",
                principalTable: "Ranks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetSupplies_Ranks_ReceiverRankId",
                table: "AssetSupplies",
                column: "ReceiverRankId",
                principalTable: "Ranks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Supplies_Ranks_ReceiverRankId",
                table: "Supplies",
                column: "ReceiverRankId",
                principalTable: "Ranks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
