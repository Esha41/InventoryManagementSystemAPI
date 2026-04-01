using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class itemDepartmentTableUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ItemDepartmentAssignments_ItemId_DepartmentId",
                table: "ItemDepartmentAssignments");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "ItemDepartmentAssignments");

            migrationBuilder.DropColumn(
                name: "DeletionDate",
                table: "ItemDepartmentAssignments");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ItemDepartmentAssignments");

            migrationBuilder.CreateIndex(
                name: "IX_ItemDepartmentAssignments_ItemId_DepartmentId",
                table: "ItemDepartmentAssignments",
                columns: new[] { "ItemId", "DepartmentId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ItemDepartmentAssignments_ItemId_DepartmentId",
                table: "ItemDepartmentAssignments");

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "ItemDepartmentAssignments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionDate",
                table: "ItemDepartmentAssignments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ItemDepartmentAssignments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_ItemDepartmentAssignments_ItemId_DepartmentId",
                table: "ItemDepartmentAssignments",
                columns: new[] { "ItemId", "DepartmentId" },
                unique: true,
                filter: "[IsDeleted] = 0");
        }
    }
}
