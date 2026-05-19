using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class AttachementRequirments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AttachmentRequirementId",
                table: "FileUplodDetails",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AttachmentRequirements",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentType = table.Column<int>(type: "int", nullable: false),
                    ParentId = table.Column<long>(type: "bigint", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    MinCount = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    MaxCount = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
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
                    table.PrimaryKey("PK_AttachmentRequirements", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileUplodDetails_AttachmentRequirementId",
                table: "FileUplodDetails",
                column: "AttachmentRequirementId");

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentRequirements_ParentType_ParentId_NameEn",
                table: "AttachmentRequirements",
                columns: new[] { "ParentType", "ParentId", "NameEn" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FileUplodDetails_AttachmentRequirements_AttachmentRequirementId",
                table: "FileUplodDetails",
                column: "AttachmentRequirementId",
                principalTable: "AttachmentRequirements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileUplodDetails_AttachmentRequirements_AttachmentRequirementId",
                table: "FileUplodDetails");

            migrationBuilder.DropTable(
                name: "AttachmentRequirements");

            migrationBuilder.DropIndex(
                name: "IX_FileUplodDetails_AttachmentRequirementId",
                table: "FileUplodDetails");

            migrationBuilder.DropColumn(
                name: "AttachmentRequirementId",
                table: "FileUplodDetails");
        }
    }
}
