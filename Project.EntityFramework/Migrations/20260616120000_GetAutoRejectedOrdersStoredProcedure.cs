using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ettad.EntityFramework.Migrations
{
    /// <summary>
    /// Applies <c>dbo.GetAutoRejectedOrders</c> for the auto-rejected orders report.
    /// </summary>
    public partial class GetAutoRejectedOrdersStoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                CREATE OR ALTER PROCEDURE [dbo].[GetAutoRejectedOrders]
                AS
                BEGIN
                  SET NOCOUNT ON;

                  SELECT
                    ISNULL(br.RequestNo, N'') AS OrderId,
                    br.CreationDate AS RequestDate,
                    CASE br.Priority
                      WHEN 1 THEN N'Normal'
                      WHEN 2 THEN N'Urgent'
                      WHEN 3 THEN N'Very Urgent'
                      ELSE CAST(br.Priority AS NVARCHAR(20))
                    END AS Priority,
                    CASE br.RequestType
                      WHEN 1 THEN N'Order'
                      WHEN 2 THEN N'Return'
                      WHEN 3 THEN N'Discard'
                      ELSE CAST(br.RequestType AS NVARCHAR(20))
                    END AS RequestType,
                    CASE br.[Status]
                      WHEN 1 THEN N'New'
                      WHEN 2 THEN N'UnderProcess'
                      WHEN 3 THEN N'Approved'
                      WHEN 4 THEN N'Rejected'
                      WHEN 5 THEN N'Cancelled'
                      WHEN 6 THEN N'ReturnedForReview'
                      WHEN 7 THEN N'AutoRejected'
                      ELSE CAST(br.[Status] AS NVARCHAR(20))
                    END AS [Status]
                  FROM BaseRequests br
                  WHERE br.IsDeleted = 0
                    AND br.[Status] = 7
                  ORDER BY br.ModificationDate DESC, br.Id DESC;
                END;
                GO
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                IF OBJECT_ID(N'dbo.GetAutoRejectedOrders', N'P') IS NOT NULL
                  DROP PROCEDURE dbo.GetAutoRejectedOrders;
                """);
        }
    }
}
