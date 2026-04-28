using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class CalibarTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Caliber",
                table: "Weapons");

            migrationBuilder.DropColumn(
                name: "Caliber",
                table: "Ammunitions");

            migrationBuilder.AddColumn<long>(
                name: "CaliberId",
                table: "Weapons",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CaliberId",
                table: "Ammunitions",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Calibers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ItemType = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calibers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Weapons_CaliberId",
                table: "Weapons",
                column: "CaliberId");

            migrationBuilder.CreateIndex(
                name: "IX_Ammunitions_CaliberId",
                table: "Ammunitions",
                column: "CaliberId");

            migrationBuilder.CreateIndex(
                name: "IX_Calibers_NameAr_ItemType",
                table: "Calibers",
                columns: new[] { "NameAr", "ItemType" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Calibers_NameEn_ItemType",
                table: "Calibers",
                columns: new[] { "NameEn", "ItemType" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.AddForeignKey(
                name: "FK_Ammunitions_Calibers_CaliberId",
                table: "Ammunitions",
                column: "CaliberId",
                principalTable: "Calibers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Weapons_Calibers_CaliberId",
                table: "Weapons",
                column: "CaliberId",
                principalTable: "Calibers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ammunitions_Calibers_CaliberId",
                table: "Ammunitions");

            migrationBuilder.DropForeignKey(
                name: "FK_Weapons_Calibers_CaliberId",
                table: "Weapons");

            migrationBuilder.DropTable(
                name: "Calibers");

            migrationBuilder.DropIndex(
                name: "IX_Weapons_CaliberId",
                table: "Weapons");

            migrationBuilder.DropIndex(
                name: "IX_Ammunitions_CaliberId",
                table: "Ammunitions");

            migrationBuilder.DropColumn(
                name: "CaliberId",
                table: "Weapons");

            migrationBuilder.DropColumn(
                name: "CaliberId",
                table: "Ammunitions");

            migrationBuilder.AddColumn<string>(
                name: "Caliber",
                table: "Weapons",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Caliber",
                table: "Ammunitions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
