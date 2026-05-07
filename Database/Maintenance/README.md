# Database maintenance (SQL Server + EF Core)

Notes for the workflow **bigint** refactor migration [`20260503103033_RefactorWorkflowTables.cs`](../../Project.EntityFramework/Migrations/20260503103033_RefactorWorkflowTables.cs).

## Apply migration

The migration is **self-contained**: it drops workflow FKs/PKs that block SQL Server, widens columns, then recreates PKs and FKs. Use **only** app startup (`MigrateAsync` in `DataSeeding/ApplicationDbInitializer.cs`) or `dotnet ef database update`—no separate SQL step.

If a database upgrade ever fails partway, prefer **restore from backup** and retry with a known-good build; manually fixing mixed `int`/`bigint` or missing keys is error-prone.

## Optional verification (troubleshooting)

Default schema is **`dbo`**. If you use another schema, adjust `OBJECT_ID`/`DBCC` targets.

### List workflow-related foreign keys

```sql
SELECT fk.name AS ForeignKeyName,
       OBJECT_SCHEMA_NAME(fk.parent_object_id) + '.' + OBJECT_NAME(fk.parent_object_id) AS [Table],
       COL_NAME(fc.parent_object_id, fc.parent_column_id) AS [Column],
       OBJECT_SCHEMA_NAME(fk.referenced_object_id) + '.' + OBJECT_NAME(fk.referenced_object_id) AS ReferencedTable,
       COL_NAME(fc.referenced_object_id, fc.referenced_column_id) AS ReferencedColumn
FROM sys.foreign_keys AS fk
INNER JOIN sys.foreign_key_columns AS fc ON fk.object_id = fc.constraint_object_id
WHERE fk.referenced_object_id IN (
        OBJECT_ID('dbo.Workflows'),
        OBJECT_ID('dbo.WorkflowSteps'),
        OBJECT_ID('dbo.WorkflowApprovalSteps'))
   OR fk.parent_object_id IN (
        OBJECT_ID('dbo.WorkflowStepTransitions'),
        OBJECT_ID('dbo.WorkflowSteps'),
        OBJECT_ID('dbo.WorkflowApprovalSteps'),
        OBJECT_ID('dbo.WorkflowStepParallelRoles'),
        OBJECT_ID('dbo.WorkflowStepNotifiers'),
        OBJECT_ID('dbo.WorkflowStepApprovalLog'),
        OBJECT_ID('dbo.WorkflowApprovalStepReminders'),
        OBJECT_ID('dbo.OrderItemHistory'))
ORDER BY ReferencedTable, [Table], ForeignKeyName;
```

### Expected primary keys (workflow tables)

| Constraint name | Table |
|-----------------|-------|
| `PK_Workflows` | `Workflows` |
| `PK_WorkflowSteps` | `WorkflowSteps` |
| `PK_WorkflowApprovalSteps` | `WorkflowApprovalSteps` |
| `PK_WorkflowStepTransitions` | `WorkflowStepTransitions` |
| `PK_WorkflowStepParallelRoles` | `WorkflowStepParallelRoles` |
| `PK_WorkflowStepNotifiers` | `WorkflowStepNotifiers` |
| `PK_WorkflowStepApprovalLog` | `WorkflowStepApprovalLog` |
| `PK_WorkflowApprovalStepReminders` | `WorkflowApprovalStepReminders` |

```sql
SELECT kc.name AS PkName,
       OBJECT_SCHEMA_NAME(kc.parent_object_id) + '.' + OBJECT_NAME(kc.parent_object_id) AS [Table]
FROM sys.key_constraints kc
WHERE kc.type = N'PK'
  AND kc.parent_object_id IN (
      OBJECT_ID('dbo.Workflows'),
      OBJECT_ID('dbo.WorkflowSteps'),
      OBJECT_ID('dbo.WorkflowApprovalSteps'),
      OBJECT_ID('dbo.WorkflowStepTransitions'),
      OBJECT_ID('dbo.WorkflowStepParallelRoles'),
      OBJECT_ID('dbo.WorkflowStepNotifiers'),
      OBJECT_ID('dbo.WorkflowStepApprovalLog'),
      OBJECT_ID('dbo.WorkflowApprovalStepReminders'))
ORDER BY [Table];
```

### Expected workflow FK names (this repo)

| Constraint name |
|-----------------|
| `FK_WorkflowSteps_Workflows_WorkflowId` |
| `FK_WorkflowStepTransitions_WorkflowSteps_SourceWorkflowStepId` |
| `FK_WorkflowStepTransitions_WorkflowSteps_TargetWorkflowStepId` |
| `FK_WorkflowStepParallelRoles_WorkflowSteps_WorkflowStepId` |
| `FK_WorkflowStepNotifiers_WorkflowSteps_WorkflowStepId` |
| `FK_WorkflowApprovalSteps_WorkflowSteps_WorkflowStepId` |
| `FK_WorkflowStepApprovalLog_WorkflowSteps_WorkflowStepId` |
| `FK_WorkflowApprovalStepReminders_WorkflowApprovalSteps_WorkflowApprovalStepId` |
| `FK_OrderItemHistory_WorkflowApprovalSteps_WorkflowApprovalStepId` |
| `FK_OrderItemHistory_WorkflowSteps_WorkflowStepId` |

```sql
DECLARE @Expected TABLE (name SYSNAME PRIMARY KEY);
INSERT INTO @Expected (name) VALUES
(N'FK_WorkflowSteps_Workflows_WorkflowId'),
(N'FK_WorkflowStepTransitions_WorkflowSteps_SourceWorkflowStepId'),
(N'FK_WorkflowStepTransitions_WorkflowSteps_TargetWorkflowStepId'),
(N'FK_WorkflowStepParallelRoles_WorkflowSteps_WorkflowStepId'),
(N'FK_WorkflowStepNotifiers_WorkflowSteps_WorkflowStepId'),
(N'FK_WorkflowApprovalSteps_WorkflowSteps_WorkflowStepId'),
(N'FK_WorkflowStepApprovalLog_WorkflowSteps_WorkflowStepId'),
(N'FK_WorkflowApprovalStepReminders_WorkflowApprovalSteps_WorkflowApprovalStepId'),
(N'FK_OrderItemHistory_WorkflowApprovalSteps_WorkflowApprovalStepId'),
(N'FK_OrderItemHistory_WorkflowSteps_WorkflowStepId');

SELECT e.name AS ExpectedFkName,
       CASE WHEN fk.name IS NULL THEN N'MISSING' ELSE N'OK' END AS Status
FROM @Expected e
LEFT JOIN sys.foreign_keys fk ON fk.name = e.name
ORDER BY e.name;
```

Optional data check:

```sql
DBCC CHECKCONSTRAINTS ('dbo.WorkflowSteps');
DBCC CHECKCONSTRAINTS ('dbo.WorkflowApprovalSteps');
DBCC CHECKCONSTRAINTS ('dbo.OrderItemHistory');
```

## Rollback

For this type change, production rollback is usually **restore from backup**, not `Down()`.

## Staging checklist

- [ ] Backup or disposable clone.
- [ ] `MigrateAsync` / `dotnet ef database update` completes without errors.
- [ ] Optional: PK/FK verification queries above show all expected keys.
- [ ] Smoke test: workflow definition; approve a request; `OrderItemHistory` if applicable.

## Related code

- Startup: `Project.EntityFramework/DataBaseContext/DataSeeding/ApplicationDbInitializer.cs` → `MigrateAsync`.
- Migration: `Project.EntityFramework/Migrations/20260503103033_RefactorWorkflowTables.cs`.
