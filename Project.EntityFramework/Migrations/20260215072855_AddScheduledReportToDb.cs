using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ettad.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class AddScheduledReportToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScheduledReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScheduleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OutputFormat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Frequency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TimeOfDay = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DayOfWeek = table.Column<int>(type: "int", nullable: true),
                    DayOfMonth = table.Column<int>(type: "int", nullable: true),
                    NextRunDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastRunDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    EmailSubject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailBody = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_ScheduledReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduledReports_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScheduledReportExecutions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScheduledReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExecutionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecipientCount = table.Column<int>(type: "int", nullable: false),
                    FileSizeBytes = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_ScheduledReportExecutions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduledReportExecutions_ScheduledReports_ScheduledReportId",
                        column: x => x.ScheduledReportId,
                        principalTable: "ScheduledReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScheduledReportRecipients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScheduledReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecipientType = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_ScheduledReportRecipients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduledReportRecipients_ScheduledReports_ScheduledReportId",
                        column: x => x.ScheduledReportId,
                        principalTable: "ScheduledReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledReportExecutions_ScheduledReportId",
                table: "ScheduledReportExecutions",
                column: "ScheduledReportId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledReportRecipients_ScheduledReportId",
                table: "ScheduledReportRecipients",
                column: "ScheduledReportId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledReports_ReportId",
                table: "ScheduledReports",
                column: "ReportId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScheduledReportExecutions");

            migrationBuilder.DropTable(
                name: "ScheduledReportRecipients");

            migrationBuilder.DropTable(
                name: "ScheduledReports");
        }
    }
}
