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
        dashboard_view,

        [Category(PlainPermissionsGenerator.General)]
        request_create,
        [Category(PlainPermissionsGenerator.General)]
        request_view,
        [Category(PlainPermissionsGenerator.General)]
        request_manage,

        [Category(PlainPermissionsGenerator.General)]
        asset_create,
        [Category(PlainPermissionsGenerator.General)]
        asset_view,
        [Category(PlainPermissionsGenerator.General)]
        inventory_view,


        [Category(PlainPermissionsGenerator.General)]
        user_view,
        [Category(PlainPermissionsGenerator.General)]
        role_view,
        [Category(PlainPermissionsGenerator.General)]
        role_edit,


        [Category(PlainPermissionsGenerator.General)]
        CanChangePassword,
        [Category(PlainPermissionsGenerator.General)]
        CanGenerateReport,
        [Category(PlainPermissionsGenerator.General)]
        CanImportData,
        [Category(PlainPermissionsGenerator.General)]
        EmailLogs
    }

}
