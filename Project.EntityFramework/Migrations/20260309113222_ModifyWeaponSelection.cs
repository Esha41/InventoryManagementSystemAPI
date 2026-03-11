using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class ModifyWeaponSelection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assets_Batches_BatchId",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "WeaponSupplySelections");

            migrationBuilder.DropColumn(
                name: "DeletionDate",
                table: "WeaponSupplySelections");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "WeaponSupplySelections");

            migrationBuilder.AlterColumn<long>(
                name: "BatchId",
                table: "WeaponSupplySelections",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ItemId",
                table: "WeaponSupplySelections",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "WeaponSupplySelections",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_WeaponSupplySelections_ItemId",
                table: "WeaponSupplySelections",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_WeaponSupplySelections_OrderId_DepotId_BatchId_ItemId",
                table: "WeaponSupplySelections",
                columns: new[] { "OrderId", "DepotId", "BatchId", "ItemId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_Batches_BatchId",
                table: "Assets",
                column: "BatchId",
                principalTable: "Batches",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WeaponSupplySelections_BaseItems_ItemId",
                table: "WeaponSupplySelections",
                column: "ItemId",
                principalTable: "BaseItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assets_Batches_BatchId",
                table: "Assets");

            migrationBuilder.DropForeignKey(
                name: "FK_WeaponSupplySelections_BaseItems_ItemId",
                table: "WeaponSupplySelections");

            migrationBuilder.DropIndex(
                name: "IX_WeaponSupplySelections_ItemId",
                table: "WeaponSupplySelections");

            migrationBuilder.DropIndex(
                name: "IX_WeaponSupplySelections_OrderId_DepotId_BatchId_ItemId",
                table: "WeaponSupplySelections");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "WeaponSupplySelections");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "WeaponSupplySelections");

            migrationBuilder.AlterColumn<long>(
                name: "BatchId",
                table: "WeaponSupplySelections",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "WeaponSupplySelections",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionDate",
                table: "WeaponSupplySelections",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "WeaponSupplySelections",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_Batches_BatchId",
                table: "Assets",
                column: "BatchId",
                principalTable: "Batches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
