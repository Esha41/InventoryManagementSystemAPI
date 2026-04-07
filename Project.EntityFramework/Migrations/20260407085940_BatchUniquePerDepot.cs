using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class BatchUniquePerDepot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Batches_BatchNumber",
                table: "Batches");

            migrationBuilder.DropIndex(
                name: "IX_Batches_DepotId",
                table: "Batches");

            migrationBuilder.CreateIndex(
                name: "IX_Batches_DepotId_BatchNumber",
                table: "Batches",
                columns: new[] { "DepotId", "BatchNumber" },
                unique: true,
                filter: "[IsDeleted] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Batches_DepotId_BatchNumber",
                table: "Batches");

            migrationBuilder.CreateIndex(
                name: "IX_Batches_BatchNumber",
                table: "Batches",
                column: "BatchNumber",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Batches_DepotId",
                table: "Batches",
                column: "DepotId");
        }
    }
}
