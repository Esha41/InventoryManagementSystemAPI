using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class ItemType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Units_NameAr",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Units_NameEn",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "Explosives");

            migrationBuilder.AddColumn<int>(
                name: "ItemType",
                table: "Units",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UnitId",
                table: "Explosives",
                type: "bigint",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ItemType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ItemType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ItemType",
                value: 1);

            migrationBuilder.InsertData(
                table: "Units",
                columns: new[] { "Id", "CreatedBy", "IsDeleted", "ItemType", "ModificationDate", "ModifiedBy", "NameAr", "NameEn" },
                values: new object[,]
                {
                    { 4L, null, false, 2, null, null, "غرام", "Gram" },
                    { 5L, null, false, 2, null, null, "مليمتر", "Millimeter" },
                    { 6L, null, false, 2, null, null, "قطعة", "Piece" },
                    { 7L, null, false, 3, null, null, "غرام", "Gram" },
                    { 8L, null, false, 3, null, null, "متر", "Meter" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Units_NameAr_ItemType",
                table: "Units",
                columns: new[] { "NameAr", "ItemType" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Units_NameEn_ItemType",
                table: "Units",
                columns: new[] { "NameEn", "ItemType" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Explosives_UnitId",
                table: "Explosives",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Explosives_Units_UnitId",
                table: "Explosives",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Explosives_Units_UnitId",
                table: "Explosives");

            migrationBuilder.DropIndex(
                name: "IX_Units_NameAr_ItemType",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Units_NameEn_ItemType",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Explosives_UnitId",
                table: "Explosives");

            migrationBuilder.DeleteData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 8L);

            migrationBuilder.DropColumn(
                name: "ItemType",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "Explosives");

            migrationBuilder.AddColumn<int>(
                name: "Unit",
                table: "Explosives",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Units_NameAr",
                table: "Units",
                column: "NameAr",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Units_NameEn",
                table: "Units",
                column: "NameEn",
                unique: true,
                filter: "[IsDeleted] = 0");
        }
    }
}
