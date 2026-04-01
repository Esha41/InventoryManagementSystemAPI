using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class ReturnColumns : Migration
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

            migrationBuilder.CreateIndex(
                name: "IX_Returns_ReturnToDepotId",
                table: "Returns",
                column: "ReturnToDepotId");

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
        }
    }
}
