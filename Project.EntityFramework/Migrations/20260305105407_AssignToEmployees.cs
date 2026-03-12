using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class AssignToEmployees : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssetAssignments_AspNetUsers_CustodianId",
                table: "AssetAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_AssetHistory_AspNetUsers_NewCustodianId",
                table: "AssetHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_AssetHistory_AspNetUsers_PreviousCustodianId",
                table: "AssetHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_AssetSupplyDetails_AspNetUsers_CustodianId",
                table: "AssetSupplyDetails");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Employees",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CustodianId",
                table: "AssetSupplyDetails",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450);

            migrationBuilder.AlterColumn<long>(
                name: "PreviousCustodianId",
                table: "AssetHistory",
                type: "bigint",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "NewCustodianId",
                table: "AssetHistory",
                type: "bigint",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CustodianId",
                table: "AssetAssignments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_UserId",
                table: "Employees",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssetAssignments_Employees_CustodianId",
                table: "AssetAssignments",
                column: "CustodianId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetHistory_Employees_NewCustodianId",
                table: "AssetHistory",
                column: "NewCustodianId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetHistory_Employees_PreviousCustodianId",
                table: "AssetHistory",
                column: "PreviousCustodianId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetSupplyDetails_Employees_CustodianId",
                table: "AssetSupplyDetails",
                column: "CustodianId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_AspNetUsers_UserId",
                table: "Employees",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssetAssignments_Employees_CustodianId",
                table: "AssetAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_AssetHistory_Employees_NewCustodianId",
                table: "AssetHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_AssetHistory_Employees_PreviousCustodianId",
                table: "AssetHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_AssetSupplyDetails_Employees_CustodianId",
                table: "AssetSupplyDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_AspNetUsers_UserId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_UserId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Employees");

            migrationBuilder.AlterColumn<string>(
                name: "CustodianId",
                table: "AssetSupplyDetails",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PreviousCustodianId",
                table: "AssetHistory",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NewCustodianId",
                table: "AssetHistory",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CustodianId",
                table: "AssetAssignments",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_AssetAssignments_AspNetUsers_CustodianId",
                table: "AssetAssignments",
                column: "CustodianId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetHistory_AspNetUsers_NewCustodianId",
                table: "AssetHistory",
                column: "NewCustodianId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetHistory_AspNetUsers_PreviousCustodianId",
                table: "AssetHistory",
                column: "PreviousCustodianId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetSupplyDetails_AspNetUsers_CustodianId",
                table: "AssetSupplyDetails",
                column: "CustodianId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
