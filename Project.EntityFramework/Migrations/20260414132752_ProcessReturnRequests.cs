using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class ProcessReturnRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeliveryDate",
                table: "Returns",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ReturnToDepotId",
                table: "Returns",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsReturned",
                table: "InventoryDetails",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "InventoryDetails",
                type: "nvarchar(max)",
                maxLength: 5000,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ReturnTrackingLines",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReturnId = table.Column<long>(type: "bigint", nullable: false),
                    RequestId = table.Column<long>(type: "bigint", nullable: false),
                    DepotId = table.Column<long>(type: "bigint", nullable: false),
                    RequestItemId = table.Column<long>(type: "bigint", nullable: true),
                    ReturnedQuantity = table.Column<long>(type: "bigint", nullable: true),
                    ReceivedQuantity = table.Column<long>(type: "bigint", nullable: true),
                    Lot = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    BatchNumber = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SerialNumber = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    AssetId = table.Column<long>(type: "bigint", nullable: true),
                    InventoryDetailId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_ReturnTrackingLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReturnTrackingLines_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReturnTrackingLines_BaseRequests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "BaseRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReturnTrackingLines_Depots_DepotId",
                        column: x => x.DepotId,
                        principalTable: "Depots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReturnTrackingLines_InventoryDetails_InventoryDetailId",
                        column: x => x.InventoryDetailId,
                        principalTable: "InventoryDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReturnTrackingLines_RequestItems_RequestItemId",
                        column: x => x.RequestItemId,
                        principalTable: "RequestItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReturnTrackingLines_Returns_ReturnId",
                        column: x => x.ReturnId,
                        principalTable: "Returns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Returns_ReturnToDepotId",
                table: "Returns",
                column: "ReturnToDepotId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnTrackingLines_AssetId",
                table: "ReturnTrackingLines",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnTrackingLines_DepotId",
                table: "ReturnTrackingLines",
                column: "DepotId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnTrackingLines_InventoryDetailId",
                table: "ReturnTrackingLines",
                column: "InventoryDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnTrackingLines_RequestId",
                table: "ReturnTrackingLines",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnTrackingLines_RequestItemId",
                table: "ReturnTrackingLines",
                column: "RequestItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnTrackingLines_ReturnId",
                table: "ReturnTrackingLines",
                column: "ReturnId");

            migrationBuilder.AddForeignKey(
                name: "FK_Returns_Depots_ReturnToDepotId",
                table: "Returns",
                column: "ReturnToDepotId",
                principalTable: "Depots",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Returns_Depots_ReturnToDepotId",
                table: "Returns");

            migrationBuilder.DropTable(
                name: "ReturnTrackingLines");

            migrationBuilder.DropIndex(
                name: "IX_Returns_ReturnToDepotId",
                table: "Returns");

            migrationBuilder.DropColumn(
                name: "DeliveryDate",
                table: "Returns");

            migrationBuilder.DropColumn(
                name: "ReturnToDepotId",
                table: "Returns");

            migrationBuilder.DropColumn(
                name: "IsReturned",
                table: "InventoryDetails");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "InventoryDetails");
        }
    }
}
