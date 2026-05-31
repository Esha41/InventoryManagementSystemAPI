using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ettad.EntityFramework.Migrations
{
    /// <summary>
    /// Applies <c>dbo.GetAuditorPendingApprovals</c>. Single source mirror: Scripts/Sql/GetLongPendingApprovals.sql.
    /// </summary>
    public partial class GetAuditorPendingApprovalsStoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                CREATE OR ALTER PROCEDURE [dbo].[GetAuditorPendingApprovals]
                AS
                BEGIN
                  SET NOCOUNT ON;

                  DECLARE @MinimumPendingDays INT = 3;

                  DECLARE @Cutoff DATETIME2(7) = DATEADD(DAY, -@MinimumPendingDays, SYSUTCDATETIME());

                  WITH Candidates AS (
                    SELECT
                      br.Id,
                      br.RequestNo,
                      br.Priority,
                      br.RequestType,
                      br.Status,
                      br.CreationDate AS RequestDate,
                      ws.CreationDate AS PendingFrom,
                      ws.ApproverUserId,
                      wfs.Id AS WorkflowStepPk,
                      wfs.WorkflowId,
                      wfs.StepOrder AS CurrentStepOrder,
                      wfs.RequireHigherApproval,
                      app.Name AS ApplicationRoleName,
                      high.Name AS HigherApprovalRoleName,
                      ROW_NUMBER() OVER (PARTITION BY br.Id ORDER BY wfs.StepOrder ASC) AS rn
                    FROM WorkflowApprovalSteps ws
                    INNER JOIN BaseRequests br ON br.Id = ws.TargetRequestId
                    INNER JOIN WorkflowSteps wfs ON wfs.Id = ws.WorkflowStepId
                    LEFT JOIN AspNetRoles app ON app.Id = wfs.ApplicationRoleId
                    LEFT JOIN AspNetRoles high ON high.Id = wfs.HigherApprovalRoleId
                    WHERE br.IsDeleted = 0
                      AND br.[Status] = 2
                      AND ws.IsCurrent = 1
                      AND ws.[Status] IN (1, 2)
                      AND ws.CreationDate <= @Cutoff
                  ),
                  Ranked AS (
                    SELECT * FROM Candidates WHERE rn = 1
                  ),
                  ApprovedPairs AS (
                    SELECT DISTINCT
                      was.TargetRequestId AS RequestId,
                      CAST(log.WorkflowStepId AS BIGINT) AS WorkflowStepPk
                    FROM WorkflowStepApprovalLog log
                    INNER JOIN WorkflowApprovalSteps was ON was.Id = log.WorkflowApprovalStepId
                    WHERE log.NewRequestStatus = 3
                      AND log.WorkflowStepId IS NOT NULL
                      AND EXISTS (SELECT 1 FROM Ranked q WHERE q.Id = was.TargetRequestId)
                  )

                  SELECT *
                  FROM (
                    SELECT
                      r.Id AS RequestId,
                      ISNULL(r.RequestNo, N'') AS OrderId,
                      r.RequestDate,
                      CASE r.Priority
                        WHEN 1 THEN N'Normal'
                        WHEN 2 THEN N'Urgent'
                        WHEN 3 THEN N'Very Urgent'
                        ELSE CAST(r.Priority AS NVARCHAR(20))
                      END AS Priority,
                      CASE r.RequestType
                        WHEN 1 THEN N'Order'
                        WHEN 2 THEN N'Return'
                        WHEN 3 THEN N'Discard'
                        ELSE CAST(r.RequestType AS NVARCHAR(20))
                      END AS RequestType,
                      CASE r.[Status]
                        WHEN 1 THEN N'New'
                        WHEN 2 THEN N'UnderProcess'
                        WHEN 3 THEN N'Approved'
                        WHEN 4 THEN N'Rejected'
                        WHEN 5 THEN N'Cancelled'
                        WHEN 6 THEN N'ReturnedForReview'
                        WHEN 7 THEN N'AutoRejected'
                        ELSE CAST(r.[Status] AS NVARCHAR(20))
                      END AS [Status],
                      r.PendingFrom,
                      COALESCE(prv.FullNameEN, prv.FullNameAR, prv.UserName, la.ChangedBy) AS PreviousApprover,
                      COALESCE(curAsg.FullNameEN, curAsg.FullNameAR, curAsg.UserName, rl.RoleLineAgg) AS PendingBy,
                      nxf.NextApproverAgg AS NextApprover,
                      nextEmails.EmailAgg AS NextApproverUserEmails
                    FROM Ranked r
                    LEFT JOIN AspNetUsers curAsg ON curAsg.Id = r.ApproverUserId
                    LEFT JOIN ApprovedPairs apk
                      ON apk.RequestId = r.Id AND apk.WorkflowStepPk = r.WorkflowStepPk
                    OUTER APPLY (
                      SELECT CAST(
                        STRING_AGG(Line.T COLLATE DATABASE_DEFAULT, N' | ') COLLATE DATABASE_DEFAULT
                        AS NVARCHAR(MAX)
                      ) AS RoleLineAgg
                      FROM (
                        SELECT CAST(
                          CASE
                            WHEN r.RequireHigherApproval = 1
                              AND apk.RequestId IS NOT NULL
                              AND NULLIF(LTRIM(RTRIM(r.HigherApprovalRoleName)), N'') IS NOT NULL
                            THEN r.HigherApprovalRoleName
                            ELSE r.ApplicationRoleName
                          END AS NVARCHAR(400)
                        ) COLLATE DATABASE_DEFAULT AS T
                        UNION ALL
                        SELECT CAST(rp.Name AS NVARCHAR(400)) COLLATE DATABASE_DEFAULT
                        FROM WorkflowStepParallelRoles pr
                        INNER JOIN AspNetRoles rp ON rp.Id = pr.RoleId
                        WHERE pr.WorkflowStepId = r.WorkflowStepPk
                      ) Line
                      WHERE NULLIF(LTRIM(RTRIM(Line.T COLLATE DATABASE_DEFAULT)), N'') IS NOT NULL
                    ) rl
                    OUTER APPLY (
                      SELECT TOP (1)
                        nws.Id AS NextStepPk,
                        nws.RequireHigherApproval AS NextRequireHigherApproval,
                        appN.Name AS NextApplicationRoleName,
                        highN.Name AS NextHigherApprovalRoleName
                      FROM WorkflowSteps nws
                      LEFT JOIN AspNetRoles appN ON appN.Id = nws.ApplicationRoleId
                      LEFT JOIN AspNetRoles highN ON highN.Id = nws.HigherApprovalRoleId
                      WHERE nws.WorkflowId = r.WorkflowId
                        AND nws.StepOrder > r.CurrentStepOrder
                      ORDER BY nws.StepOrder ASC
                    ) nx
                    LEFT JOIN ApprovedPairs apkNext
                      ON apkNext.RequestId = r.Id AND apkNext.WorkflowStepPk = nx.NextStepPk
                    OUTER APPLY (
                      SELECT CAST(
                        STRING_AGG(LineNext.T COLLATE DATABASE_DEFAULT, N' | ') COLLATE DATABASE_DEFAULT
                        AS NVARCHAR(MAX)
                      ) AS NextApproverAgg
                      FROM (
                        SELECT CAST(
                          CASE
                            WHEN nx.NextRequireHigherApproval = 1
                              AND apkNext.RequestId IS NOT NULL
                              AND NULLIF(LTRIM(RTRIM(nx.NextHigherApprovalRoleName)), N'') IS NOT NULL
                            THEN nx.NextHigherApprovalRoleName
                            ELSE nx.NextApplicationRoleName
                          END AS NVARCHAR(400)
                        ) COLLATE DATABASE_DEFAULT AS T
                        UNION ALL
                        SELECT CAST(rn2.Name AS NVARCHAR(400)) COLLATE DATABASE_DEFAULT
                        FROM WorkflowStepParallelRoles pr2
                        INNER JOIN AspNetRoles rn2 ON rn2.Id = pr2.RoleId
                        WHERE nx.NextStepPk IS NOT NULL
                          AND pr2.WorkflowStepId = nx.NextStepPk
                      ) LineNext
                      WHERE nx.NextStepPk IS NOT NULL
                        AND NULLIF(LTRIM(RTRIM(LineNext.T COLLATE DATABASE_DEFAULT)), N'') IS NOT NULL
                    ) nxf
                    OUTER APPLY (
                      SELECT CAST(STRING_AGG(EmailDistinct.EmailNorm, N'; ') AS NVARCHAR(MAX)) AS EmailAgg
                      FROM (
                        SELECT DISTINCT LTRIM(RTRIM(CAST(u.Email AS NVARCHAR(256)))) AS EmailNorm
                        FROM STRING_SPLIT(COALESCE(nxf.NextApproverAgg, N''), N'|') AS seg
                        INNER JOIN AspNetRoles ro
                          ON (
                            (ro.[Name] IS NOT NULL AND LOWER(LTRIM(RTRIM(ro.[Name]))) = LOWER(LTRIM(RTRIM(seg.value))))
                            OR (ro.NameAr IS NOT NULL AND LOWER(LTRIM(RTRIM(ro.NameAr))) = LOWER(LTRIM(RTRIM(seg.value))))
                          )
                        INNER JOIN AspNetUserRoles aur ON aur.RoleId = ro.Id
                        INNER JOIN AspNetUsers u ON u.Id = aur.UserId
                        WHERE NULLIF(LTRIM(RTRIM(seg.value)), N'') IS NOT NULL
                          AND u.Email IS NOT NULL
                          AND NULLIF(LTRIM(RTRIM(u.Email)), N'') IS NOT NULL
                      ) EmailDistinct
                    ) nextEmails
                    OUTER APPLY (
                      SELECT TOP (1)
                        log.ChangedBy
                      FROM WorkflowStepApprovalLog log
                      INNER JOIN WorkflowApprovalSteps wx ON wx.Id = log.WorkflowApprovalStepId
                      WHERE wx.TargetRequestId = r.Id
                        AND log.NewRequestStatus = 3
                      ORDER BY log.ChangedAt DESC
                    ) la
                    LEFT JOIN AspNetUsers prv
                      ON prv.Id = la.ChangedBy OR (prv.UserName IS NOT NULL AND prv.UserName = la.ChangedBy)
                  ) z
                  WHERE z.PendingBy IS NOT NULL
                    AND NULLIF(LTRIM(RTRIM(z.PendingBy)), N'') IS NOT NULL
                    AND z.NextApprover IS NOT NULL
                    AND NULLIF(LTRIM(RTRIM(z.NextApprover)), N'') IS NOT NULL
                       AND (
                    (
                        CHARINDEX(N'Auditor', z.PendingBy COLLATE DATABASE_DEFAULT) > 0
                        AND CHARINDEX(N'Head', z.NextApprover COLLATE DATABASE_DEFAULT) > 0
                    )
                    OR
                    (
                        CHARINDEX(N'Military Training Officer', z.PendingBy COLLATE DATABASE_DEFAULT) > 0
                        AND CHARINDEX(N'Head of Military Training', z.NextApprover COLLATE DATABASE_DEFAULT) > 0
                    )
                 )

                  ORDER BY z.PendingFrom DESC, z.OrderId ASC;
                END;
             GO
             """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                IF OBJECT_ID(N'dbo.GetAuditorPendingApprovals', N'P') IS NOT NULL
                  DROP PROCEDURE dbo.GetAuditorPendingApprovals;
                """);
        }
    }
}
