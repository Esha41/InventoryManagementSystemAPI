using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class MakeBatchNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WeaponSupplySelections_OrderId_DepotId_BatchId",
                table: "WeaponSupplySelections");

            migrationBuilder.AlterColumn<long>(
                name: "BatchId",
                table: "WeaponSupplySelections",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "BatchId",
                table: "WeaponSupplySelections",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WeaponSupplySelections_OrderId_DepotId_BatchId",
                table: "WeaponSupplySelections",
                columns: new[] { "OrderId", "DepotId", "BatchId" });
        }
    }
}
