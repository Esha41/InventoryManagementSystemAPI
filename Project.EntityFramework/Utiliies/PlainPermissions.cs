using Ettad.EntityFramework.Utiliies;
using System.ComponentModel;

namespace Ettad.Data.Enums
{
    public enum PlainPermissions
    {
        [Category(PlainPermissionsGenerator.Dashboard)]
        dashboard_view,

        [Category(PlainPermissionsGenerator.Dashboard)]
        InventoryDashboard,
        [Category(PlainPermissionsGenerator.Dashboard)]
        Forecast_view,

        [Category(PlainPermissionsGenerator.SystemFeatures)]
        CanChangePassword,
        [Category(PlainPermissionsGenerator.SystemFeatures)]
        CanGenerateReport,
        [Category(PlainPermissionsGenerator.SystemFeatures)]
        CanImportData,
        [Category(PlainPermissionsGenerator.SystemFeatures)]
        EmailLogs,

        [Category(PlainPermissionsGenerator.Workflow)]
        UpdateRequestAndSuggestLots,

        [Category(PlainPermissionsGenerator.Workflow)]
        UpdateRequestAndSupply,

        [Category(PlainPermissionsGenerator.Workflow)]
        SubmitSupply,

        [Category(PlainPermissionsGenerator.Workflow)]
        SetSupplyPickupDate,

        [Category(PlainPermissionsGenerator.Workflow)]
        ConfirmSupplyPickupDate,

        [Category(PlainPermissionsGenerator.Workflow)]
        CannotRejectRequest,

        [Category(PlainPermissionsGenerator.Workflow)]
        ReviewWeaponSupply,

        [Category(PlainPermissionsGenerator.Inventory)]
        AllowanceItemViewAllDepartments,
    }

}
