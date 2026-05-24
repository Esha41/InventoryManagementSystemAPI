using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class ExtendAttachmentsSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApplicableEntityType",
                table: "AttachmentRequirements",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "AttachmentRequirements",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentRequirements_Code",
                table: "AttachmentRequirements",
                column: "Code",
                unique: true,
                filter: "[Code] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AttachmentRequirements_Code",
                table: "AttachmentRequirements");

            migrationBuilder.DropColumn(
                name: "ApplicableEntityType",
                table: "AttachmentRequirements");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "AttachmentRequirements");
        }
    }
}
