using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class Accessories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BaseItems_ItemNo",
                table: "BaseItems");

            migrationBuilder.AlterColumn<string>(
                name: "ItemNo",
                table: "BaseItems",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.CreateTable(
                name: "Accessories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accessories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accessories_BaseItems_Id",
                        column: x => x.Id,
                        principalTable: "BaseItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssetSupplyAccessoryDetails",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssetSupplyDetailId = table.Column<long>(type: "bigint", nullable: false),
                    AccessoryId = table.Column<long>(type: "bigint", nullable: false),
                    DefaultQuantity = table.Column<long>(type: "bigint", nullable: false),
                    SuppliedQuantity = table.Column<long>(type: "bigint", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetSupplyAccessoryDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssetSupplyAccessoryDetails_AssetSupplyDetails_AssetSupplyDetailId",
                        column: x => x.AssetSupplyDetailId,
                        principalTable: "AssetSupplyDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssetSupplyAccessoryDetails_BaseItems_AccessoryId",
                        column: x => x.AccessoryId,
                        principalTable: "BaseItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WeaponAccessories",
                columns: table => new
                {
                    WeaponId = table.Column<long>(type: "bigint", nullable: false),
                    AccessoryId = table.Column<long>(type: "bigint", nullable: false),
                    DefaultQuantity = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeaponAccessories", x => new { x.WeaponId, x.AccessoryId });
                    table.ForeignKey(
                        name: "FK_WeaponAccessories_Accessories_AccessoryId",
                        column: x => x.AccessoryId,
                        principalTable: "Accessories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WeaponAccessories_Weapons_WeaponId",
                        column: x => x.WeaponId,
                        principalTable: "Weapons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BaseItems_ItemNo",
                table: "BaseItems",
                column: "ItemNo",
                unique: true,
                filter: "[ItemType] <> 4");

            migrationBuilder.CreateIndex(
                name: "IX_AssetSupplyAccessoryDetails_AccessoryId",
                table: "AssetSupplyAccessoryDetails",
                column: "AccessoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetSupplyAccessoryDetails_AssetSupplyDetailId",
                table: "AssetSupplyAccessoryDetails",
                column: "AssetSupplyDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetSupplyAccessoryDetails_AssetSupplyDetailId_AccessoryId",
                table: "AssetSupplyAccessoryDetails",
                columns: new[] { "AssetSupplyDetailId", "AccessoryId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WeaponAccessories_AccessoryId",
                table: "WeaponAccessories",
                column: "AccessoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssetSupplyAccessoryDetails");

            migrationBuilder.DropTable(
                name: "WeaponAccessories");

            migrationBuilder.DropTable(
                name: "Accessories");

            migrationBuilder.DropIndex(
                name: "IX_BaseItems_ItemNo",
                table: "BaseItems");

            migrationBuilder.AlterColumn<string>(
                name: "ItemNo",
                table: "BaseItems",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BaseItems_ItemNo",
                table: "BaseItems",
                column: "ItemNo",
                unique: true);
        }
    }
}
