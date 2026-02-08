using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class AddReportsEntityToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Ammunitions",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Ammunitions",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Ammunitions",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "Ammunitions",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "Ammunitions",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "Ammunitions",
                keyColumn: "Id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "Ammunitions",
                keyColumn: "Id",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                table: "Ammunitions",
                keyColumn: "Id",
                keyValue: 8L);

            migrationBuilder.DeleteData(
                table: "Ammunitions",
                keyColumn: "Id",
                keyValue: 9L);

            migrationBuilder.DeleteData(
                table: "BaseItems",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "BaseItems",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "BaseItems",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "BaseItems",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "BaseItems",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "BaseItems",
                keyColumn: "Id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "BaseItems",
                keyColumn: "Id",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                table: "BaseItems",
                keyColumn: "Id",
                keyValue: 8L);

            migrationBuilder.DeleteData(
                table: "BaseItems",
                keyColumn: "Id",
                keyValue: 9L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "BaseItems",
                columns: new[] { "Id", "ClassificationId", "CreatedBy", "DeletedBy", "DeletionDate", "Distribution", "IsDeleted", "ItemNo", "ItemType", "MinimumQuantity", "ModificationDate", "ModifiedBy", "Name", "Notes", "Nsn", "PartNo", "Price", "ReferenceNo", "TypeId", "UNNumber" },
                values: new object[,]
                {
                    { 1L, null, null, null, null, null, false, "AMM-001", 1, 200L, null, null, "5.56x45mm NATO", null, "1305-01-000-0001", "PN-556-001", 0.65m, null, null, null },
                    { 2L, null, null, null, null, null, false, "AMM-002", 1, 150L, null, null, "7.62x51mm NATO", null, "1305-01-000-0002", "PN-762-001", 1.25m, null, null, null },
                    { 3L, null, null, null, null, null, false, "AMM-003", 1, 200L, null, null, "9x19mm Parabellum", null, "1305-01-000-0003", "PN-9MM-001", 0.70m, null, null, null },
                    { 4L, null, null, null, null, null, false, "AMM-004", 1, 50L, null, null, ".50 BMG", null, "1305-01-000-0004", "PN-50BMG-001", 3.50m, null, null, null },
                    { 5L, null, null, null, null, null, false, "AMM-005", 1, 100L, null, null, ".308 Winchester", null, "1305-01-000-0005", "PN-308-001", 1.50m, null, null, null },
                    { 6L, null, null, null, null, null, false, "AMM-006", 1, 150L, null, null, ".45 ACP", null, "1305-01-000-0006", "PN-45ACP-001", 0.75m, null, null, null },
                    { 7L, null, null, null, null, null, false, "AMM-007", 1, 50L, null, null, "12.7x108mm", null, "1305-01-000-0007", "PN-127-001", 2.50m, null, null, null },
                    { 8L, null, null, null, null, null, false, "AMM-008", 1, 200L, null, null, "5.45x39mm", null, "1305-01-000-0008", "PN-545-001", 0.60m, null, null, null },
                    { 9L, null, null, null, null, null, false, "AMM-009", 1, 150L, null, null, ".40 S&W", null, "1305-01-000-0009", "PN-40SW-001", 0.80m, null, null, null }
                });

            migrationBuilder.InsertData(
                table: "Ammunitions",
                columns: new[] { "Id", "AmmunitionType", "ArmNumber", "BulletDiameter", "BulletDiameterUnitId", "CaseTypeId", "CompatibilityId", "HazardDivisionId", "IsLinked", "NatureOptionId", "PrimaryPurposId", "Primer", "ProjectailMaterialId", "ProjectileColorId", "PropellantId", "TotalWeight" },
                values: new object[,]
                {
                    { 1L, 1, null, 5.56m, 1L, 1L, 1L, 1L, false, 1L, 1L, "Boxer", 1L, 1L, 1L, 12.0m },
                    { 2L, 1, null, 7.62m, 2L, 2L, 2L, 2L, false, 2L, 2L, "Berdan", 2L, 2L, 2L, 24.0m },
                    { 3L, 1, null, 9.0m, 3L, 3L, 3L, 3L, false, 3L, 3L, "Boxer", 3L, 3L, 3L, 7.5m },
                    { 4L, 1, null, 12.7m, 1L, 1L, 1L, 1L, false, 1L, 1L, "Berdan", 1L, 1L, 1L, 115.0m },
                    { 5L, 1, null, 7.62m, 2L, 2L, 2L, 2L, false, 2L, 2L, "Boxer", 2L, 2L, 2L, 23.0m },
                    { 6L, 1, null, 11.43m, 3L, 3L, 3L, 3L, false, 3L, 3L, "Boxer", 3L, 3L, 3L, 15.0m },
                    { 7L, 1, null, 12.7m, 1L, 1L, 1L, 1L, false, 1L, 1L, "Berdan", 1L, 1L, 1L, 130.0m },
                    { 8L, 1, null, 5.45m, 2L, 2L, 2L, 2L, false, 2L, 2L, "Berdan", 2L, 2L, 2L, 10.5m },
                    { 9L, 1, null, 10.16m, 3L, 3L, 3L, 3L, false, 3L, 3L, "Boxer", 3L, 3L, 3L, 11.0m }
                });
        }
    }
}
