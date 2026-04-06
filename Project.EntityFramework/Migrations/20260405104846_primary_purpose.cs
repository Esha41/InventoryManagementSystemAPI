using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class primary_purpose : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ammunitions_PrimaryPurposes_PrimaryPurposId",
                table: "Ammunitions");

            migrationBuilder.DropIndex(
                name: "IX_Ammunitions_PrimaryPurposId",
                table: "Ammunitions");

            migrationBuilder.DropColumn(
                name: "PrimaryPurposId",
                table: "Ammunitions");

            migrationBuilder.AddColumn<long>(
                name: "PrimaryPurposId",
                table: "InventoryDetails",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PrimaryPurposId",
                table: "Batches",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BaseItemPrimaryPurposes",
                columns: table => new
                {
                    BaseItemId = table.Column<long>(type: "bigint", nullable: false),
                    PrimaryPurposId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseItemPrimaryPurposes", x => new { x.BaseItemId, x.PrimaryPurposId });
                    table.ForeignKey(
                        name: "FK_BaseItemPrimaryPurposes_BaseItems_BaseItemId",
                        column: x => x.BaseItemId,
                        principalTable: "BaseItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseItemPrimaryPurposes_PrimaryPurposes_PrimaryPurposId",
                        column: x => x.PrimaryPurposId,
                        principalTable: "PrimaryPurposes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryDetails_PrimaryPurposId",
                table: "InventoryDetails",
                column: "PrimaryPurposId");

            migrationBuilder.CreateIndex(
                name: "IX_Batches_PrimaryPurposId",
                table: "Batches",
                column: "PrimaryPurposId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseItemPrimaryPurposes_PrimaryPurposId",
                table: "BaseItemPrimaryPurposes",
                column: "PrimaryPurposId");

            migrationBuilder.AddForeignKey(
                name: "FK_Batches_PrimaryPurposes_PrimaryPurposId",
                table: "Batches",
                column: "PrimaryPurposId",
                principalTable: "PrimaryPurposes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryDetails_PrimaryPurposes_PrimaryPurposId",
                table: "InventoryDetails",
                column: "PrimaryPurposId",
                principalTable: "PrimaryPurposes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Batches_PrimaryPurposes_PrimaryPurposId",
                table: "Batches");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryDetails_PrimaryPurposes_PrimaryPurposId",
                table: "InventoryDetails");

            migrationBuilder.DropTable(
                name: "BaseItemPrimaryPurposes");

            migrationBuilder.DropIndex(
                name: "IX_InventoryDetails_PrimaryPurposId",
                table: "InventoryDetails");

            migrationBuilder.DropIndex(
                name: "IX_Batches_PrimaryPurposId",
                table: "Batches");

            migrationBuilder.DropColumn(
                name: "PrimaryPurposId",
                table: "InventoryDetails");

            migrationBuilder.DropColumn(
                name: "PrimaryPurposId",
                table: "Batches");

            migrationBuilder.AddColumn<long>(
                name: "PrimaryPurposId",
                table: "Ammunitions",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ammunitions_PrimaryPurposId",
                table: "Ammunitions",
                column: "PrimaryPurposId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ammunitions_PrimaryPurposes_PrimaryPurposId",
                table: "Ammunitions",
                column: "PrimaryPurposId",
                principalTable: "PrimaryPurposes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
