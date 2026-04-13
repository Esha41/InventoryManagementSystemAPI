using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class AseetsMissingColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Batches_PrimaryPurposes_PrimaryPurposId",
                table: "Batches");

            migrationBuilder.DropIndex(
                name: "IX_Batches_PrimaryPurposId",
                table: "Batches");

            migrationBuilder.DropColumn(
                name: "PrimaryPurposId",
                table: "Batches");

            migrationBuilder.AddColumn<long>(
                name: "ManufacturerId",
                table: "Assets",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PrimaryPurposId",
                table: "Assets",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SupplierId",
                table: "Assets",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Assets_ManufacturerId",
                table: "Assets",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_PrimaryPurposId",
                table: "Assets",
                column: "PrimaryPurposId");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_SupplierId",
                table: "Assets",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_Manufacturers_ManufacturerId",
                table: "Assets",
                column: "ManufacturerId",
                principalTable: "Manufacturers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_PrimaryPurposes_PrimaryPurposId",
                table: "Assets",
                column: "PrimaryPurposId",
                principalTable: "PrimaryPurposes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_Suppliers_SupplierId",
                table: "Assets",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assets_Manufacturers_ManufacturerId",
                table: "Assets");

            migrationBuilder.DropForeignKey(
                name: "FK_Assets_PrimaryPurposes_PrimaryPurposId",
                table: "Assets");

            migrationBuilder.DropForeignKey(
                name: "FK_Assets_Suppliers_SupplierId",
                table: "Assets");

            migrationBuilder.DropIndex(
                name: "IX_Assets_ManufacturerId",
                table: "Assets");

            migrationBuilder.DropIndex(
                name: "IX_Assets_PrimaryPurposId",
                table: "Assets");

            migrationBuilder.DropIndex(
                name: "IX_Assets_SupplierId",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "ManufacturerId",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "PrimaryPurposId",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "Assets");

            migrationBuilder.AddColumn<long>(
                name: "PrimaryPurposId",
                table: "Batches",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Batches_PrimaryPurposId",
                table: "Batches",
                column: "PrimaryPurposId");

            migrationBuilder.AddForeignKey(
                name: "FK_Batches_PrimaryPurposes_PrimaryPurposId",
                table: "Batches",
                column: "PrimaryPurposId",
                principalTable: "PrimaryPurposes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
