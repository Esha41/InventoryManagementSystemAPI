using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class AddReportEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReportStatuses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReportName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ReportStatusId = table.Column<long>(type: "bigint", nullable: false, defaultValue: 1L),
                    Url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    LayoutData = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ReportType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ReportParameters = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsTemplate = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reports_ReportStatuses_ReportStatusId",
                        column: x => x.ReportStatusId,
                        principalTable: "ReportStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "ReportStatuses",
                columns: new[] { "Id", "CreatedBy", "IsDeleted", "ModificationDate", "ModifiedBy", "NameAr", "NameEn" },
                values: new object[,]
                {
                    { 1L, null, false, null, null, "مسودة", "Draft" },
                    { 2L, null, false, null, null, "نشط", "Active" },
                    { 3L, null, false, null, null, "منشور", "Published" },
                    { 5L, null, false, null, null, "غير نشط", "Inactive" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reports_CreatedBy",
                table: "Reports",
                column: "CreatedBy",
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ReportName",
                table: "Reports",
                column: "ReportName",
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ReportStatusId",
                table: "Reports",
                column: "ReportStatusId",
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ReportType",
                table: "Reports",
                column: "ReportType",
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_Url",
                table: "Reports",
                column: "Url",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_ReportStatuses_NameAr",
                table: "ReportStatuses",
                column: "NameAr",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_ReportStatuses_NameEn",
                table: "ReportStatuses",
                column: "NameEn",
                unique: true,
                filter: "[IsDeleted] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "ReportStatuses");
        }
    }
}
