using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class PatchNoTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "RankId",
                table: "Employees",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "BatchId",
                table: "Assets",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "Batches",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchNumber = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DepotId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_Batches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Batches_Depots_DepotId",
                        column: x => x.DepotId,
                        principalTable: "Depots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_RankId",
                table: "Employees",
                column: "RankId");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_BatchId",
                table: "Assets",
                column: "BatchId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_Batches_BatchId",
                table: "Assets",
                column: "BatchId",
                principalTable: "Batches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Ranks_RankId",
                table: "Employees",
                column: "RankId",
                principalTable: "Ranks",
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
                name: "FK_Employees_Ranks_RankId",
                table: "Employees");

            migrationBuilder.DropTable(
                name: "Batches");

            migrationBuilder.DropIndex(
                name: "IX_Employees_RankId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Assets_BatchId",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "RankId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "BatchId",
                table: "Assets");
        }
    }
}
