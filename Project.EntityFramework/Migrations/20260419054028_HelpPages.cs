using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class HelpPages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "LastAcceptedTermsConditionsId",
                table: "AspNetUsers",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "HelpCenterArticles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
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
                    table.PrimaryKey("PK_HelpCenterArticles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HelpCenterContactDisplaySettings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    SupportEmail = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SupportPhone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HelpCenterContactDisplaySettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HelpCenterContactMessages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    SenderEmail = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    AdminReply = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RepliedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RepliedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
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
                    table.PrimaryKey("PK_HelpCenterContactMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HelpCenterTermsConditions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Version = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    EffectiveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    table.PrimaryKey("PK_HelpCenterTermsConditions", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "HelpCenterContactDisplaySettings",
                columns: new[] { "Id", "ModifiedAt", "ModifiedBy", "SupportEmail", "SupportPhone" },
                values: new object[] { 1L, null, null, "", "" });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_LastAcceptedTermsConditionsId",
                table: "AspNetUsers",
                column: "LastAcceptedTermsConditionsId");

            migrationBuilder.CreateIndex(
                name: "IX_HelpCenterArticles_Category",
                table: "HelpCenterArticles",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_HelpCenterArticles_IsPublished",
                table: "HelpCenterArticles",
                column: "IsPublished");

            migrationBuilder.CreateIndex(
                name: "IX_HelpCenterArticles_SortOrder",
                table: "HelpCenterArticles",
                column: "SortOrder");

            migrationBuilder.CreateIndex(
                name: "IX_HelpCenterContactMessages_CreationDate",
                table: "HelpCenterContactMessages",
                column: "CreationDate");

            migrationBuilder.CreateIndex(
                name: "IX_HelpCenterContactMessages_IsRead",
                table: "HelpCenterContactMessages",
                column: "IsRead");

            migrationBuilder.CreateIndex(
                name: "IX_HelpCenterTermsConditions_EffectiveDate",
                table: "HelpCenterTermsConditions",
                column: "EffectiveDate");

            migrationBuilder.CreateIndex(
                name: "IX_HelpCenterTermsConditions_IsActive",
                table: "HelpCenterTermsConditions",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_HelpCenterTermsConditions_Version",
                table: "HelpCenterTermsConditions",
                column: "Version",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_HelpCenterTermsConditions_LastAcceptedTermsConditionsId",
                table: "AspNetUsers",
                column: "LastAcceptedTermsConditionsId",
                principalTable: "HelpCenterTermsConditions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_HelpCenterTermsConditions_LastAcceptedTermsConditionsId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "HelpCenterArticles");

            migrationBuilder.DropTable(
                name: "HelpCenterContactDisplaySettings");

            migrationBuilder.DropTable(
                name: "HelpCenterContactMessages");

            migrationBuilder.DropTable(
                name: "HelpCenterTermsConditions");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_LastAcceptedTermsConditionsId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastAcceptedTermsConditionsId",
                table: "AspNetUsers");
        }
    }
}
