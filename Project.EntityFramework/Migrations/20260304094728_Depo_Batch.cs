using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class Depo_Batch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WeaponSupplySelections",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    DepotId = table.Column<long>(type: "bigint", nullable: false),
                    BatchId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_WeaponSupplySelections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeaponSupplySelections_Batches_BatchId",
                        column: x => x.BatchId,
                        principalTable: "Batches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WeaponSupplySelections_Depots_DepotId",
                        column: x => x.DepotId,
                        principalTable: "Depots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WeaponSupplySelections_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WeaponSupplySelections_BatchId",
                table: "WeaponSupplySelections",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_WeaponSupplySelections_DepotId",
                table: "WeaponSupplySelections",
                column: "DepotId");

            migrationBuilder.CreateIndex(
                name: "IX_WeaponSupplySelections_OrderId",
                table: "WeaponSupplySelections",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_WeaponSupplySelections_OrderId_DepotId_BatchId",
                table: "WeaponSupplySelections",
                columns: new[] { "OrderId", "DepotId", "BatchId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeaponSupplySelections");
        }
    }
}
