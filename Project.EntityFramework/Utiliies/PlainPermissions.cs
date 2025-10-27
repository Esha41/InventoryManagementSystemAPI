using Ettad.EntityFramework.Utiliies;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.Data.Enums
{
    public enum PlainPermissions
    {
        [Category(PlainPermissionsGenerator.General)]
        CanChangePassword,

        [Category(PlainPermissionsGenerator.General)]
        EnableRecursiveReportingManager,

        [Category(PlainPermissionsGenerator.General)]
        IncludeEmployeeViewInManagerDashboard,

        [Category(PlainPermissionsGenerator.General)]
        EmailLogs,

        [Category(PlainPermissionsGenerator.Calculation)]
        CanAdjustAttendanceLog,
        [Category(PlainPermissionsGenerator.Calculation)]
        CanRecalculateAttendance,

        [Category(PlainPermissionsGenerator.DataImport)]
        CanImportData,

        [Category(PlainPermissionsGenerator.EmployeeView)]
        ViewAllEmployees,
        [Category(PlainPermissionsGenerator.EmployeeView)]
        BasedOnEntity,

        [Category(PlainPermissionsGenerator.Reports)]
        CanGenerateReport,
        [Category(PlainPermissionsGenerator.Reports)]
        CanSchedulerReport,
        [Category(PlainPermissionsGenerator.Reports)]
        CanMakeReportAsPublic,

        [Category(PlainPermissionsGenerator.Roles)]
        CanAssignRolesToUser,
        [Category(PlainPermissionsGenerator.Roles)]
        CanReadUserRoles,
        [Category(PlainPermissionsGenerator.Roles)]
        CanReadUsersInRole,
        [Category(PlainPermissionsGenerator.Roles)]
        CanDeleteUserFromRole
    }

}
