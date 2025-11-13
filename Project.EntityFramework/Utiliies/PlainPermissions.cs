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
        [Category(PlainPermissionsGenerator.Dashboard)]
        dashboard_view,

        [Category(PlainPermissionsGenerator.SystemFeatures)]
        CanChangePassword,
        [Category(PlainPermissionsGenerator.SystemFeatures)]
        CanGenerateReport,
        [Category(PlainPermissionsGenerator.SystemFeatures)]
        CanImportData,
        [Category(PlainPermissionsGenerator.SystemFeatures)]
        EmailLogs
    }

}
