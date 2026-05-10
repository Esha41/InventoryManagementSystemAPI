using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class BulkDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssetBulkDeletionJobs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HangfireJobId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    InitiatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    JobStatus = table.Column<byte>(type: "tinyint", nullable: false),
                    Scope = table.Column<byte>(type: "tinyint", nullable: false),
                    BatchId = table.Column<long>(type: "bigint", nullable: true),
                    DepotId = table.Column<long>(type: "bigint", nullable: true),
                    ExplicitAssetIdsJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalCandidates = table.Column<int>(type: "int", nullable: false),
                    ProcessedCount = table.Column<int>(type: "int", nullable: false),
                    DeletedCount = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartedUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetBulkDeletionJobs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssetBulkDeletionJobs_CreatedUtc",
                table: "AssetBulkDeletionJobs",
                column: "CreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_AssetBulkDeletionJobs_InitiatedByUserId",
                table: "AssetBulkDeletionJobs",
                column: "InitiatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetBulkDeletionJobs_JobStatus_CreatedUtc",
                table: "AssetBulkDeletionJobs",
                columns: new[] { "JobStatus", "CreatedUtc" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssetBulkDeletionJobs");
        }
    }
}
