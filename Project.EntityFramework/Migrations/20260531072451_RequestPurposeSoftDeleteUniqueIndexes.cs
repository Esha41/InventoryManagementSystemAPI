using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class RequestPurposeSoftDeleteUniqueIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RequestPurposes_NameAr",
                table: "RequestPurposes");

            migrationBuilder.DropIndex(
                name: "IX_RequestPurposes_NameEn",
                table: "RequestPurposes");

            migrationBuilder.DropIndex(
                name: "IX_AttachmentRequirements_ParentType_ParentId_NameEn",
                table: "AttachmentRequirements");

            migrationBuilder.CreateIndex(
                name: "IX_RequestPurposes_NameAr",
                table: "RequestPurposes",
                column: "NameAr",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_RequestPurposes_NameEn",
                table: "RequestPurposes",
                column: "NameEn",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentRequirements_ParentType_ParentId_NameEn",
                table: "AttachmentRequirements",
                columns: new[] { "ParentType", "ParentId", "NameEn" },
                unique: true,
                filter: "[IsDeleted] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RequestPurposes_NameAr",
                table: "RequestPurposes");

            migrationBuilder.DropIndex(
                name: "IX_RequestPurposes_NameEn",
                table: "RequestPurposes");

            migrationBuilder.DropIndex(
                name: "IX_AttachmentRequirements_ParentType_ParentId_NameEn",
                table: "AttachmentRequirements");

            migrationBuilder.CreateIndex(
                name: "IX_RequestPurposes_NameAr",
                table: "RequestPurposes",
                column: "NameAr",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RequestPurposes_NameEn",
                table: "RequestPurposes",
                column: "NameEn",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentRequirements_ParentType_ParentId_NameEn",
                table: "AttachmentRequirements",
                columns: new[] { "ParentType", "ParentId", "NameEn" },
                unique: true);
        }
    }
}
